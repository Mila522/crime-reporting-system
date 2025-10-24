
using System;
using System.Linq;
using System.Web.Security;
using ComplaintSystem.Models;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Net.Sockets;
using ComplaintSystem.Services;
using System.Configuration;

namespace ComplaintSystem
{
    public partial class RegisterComplainant : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                lblMessage.Text = "Please fill in all required fields.";
                return;
            }

            try
            {
                using (var context = new AppDbContext())
                {
                    // Check if email or phone already exists
                    var existingUser = context.Users.FirstOrDefault(u => u.Email == txtEmail.Text || u.PhoneNumber == txtPhoneNumber.Text);
                    if (existingUser != null)
                    {
                        lblMessage.Text = "A user with this email or phone number already exists.";
                        return;
                    }

                    // Generate verification code
                    var verificationCode = GenerateVerificationCode();

                    // Create new user
                    var newUser = new User
                    {
                        Email = txtEmail.Text,
                        PhoneNumber = txtPhoneNumber.Text,
                        FirstNames = txtFirstNames.Text,
                        Surname = txtSurname.Text,
                        IDNumber = txtIDNumber.Text,
                        Role = "Complainant",
                        PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text, "SHA1"),
                        VerificationCode = verificationCode,
                        VerificationCodeExpiry = DateTime.Now.AddMinutes(30),
                        IsVerified = false
                    };

                    // Save to database
                    context.Users.Add(newUser);
                    context.SaveChanges();
                    

                    lblMessage.Text = "User registered successfully. ";

                    // Try to send verification email
                    bool emailSent = SendVerificationEmail(newUser.Email, verificationCode);

                    if (emailSent)
                    {
                        Session["VerificationEmail"] = newUser.Email;
                        Response.Redirect("~/VerifyEmail.aspx");
                    }
                    else
                    {
                        // Email failed, but user is registered
                        // You can either:
                        // Option A: Redirect to login anyway (skip verification)
                        // Option B: Show message and let them try to resend

                        // For now, let's redirect to login but mark as unverified
                        lblMessage.Text += " You can login but please verify your email later.";
                        Response.Redirect("~/Login.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error during registration: {ex.Message}";
                if (ex.InnerException != null)
                {
                    lblMessage.Text += " | Inner: " + ex.InnerException.Message;
                    if (ex.InnerException.InnerException != null)
                    {
                        lblMessage.Text += " | Inner Inner: " + ex.InnerException.InnerException.Message;
                    }
                }
            }
        }

        private string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private bool SendVerificationEmail(string emailAddress, string code)
        {
            try
            {
                // Debug: Check if we have the email parameters
                if (string.IsNullOrEmpty(emailAddress))
                {
                    lblMessage.Text += " | ERROR: Email address is empty";
                    return false;
                }

                if (string.IsNullOrEmpty(code))
                {
                    lblMessage.Text += " | ERROR: Verification code is empty";
                    return false;
                }

                // Debug: Check Web.config settings
                string fromEmail = ConfigurationManager.AppSettings["Email"];
                string fromPassword = ConfigurationManager.AppSettings["EmailPassword"];

                if (string.IsNullOrEmpty("mphathisitole@gmail.com"))
                {
                    lblMessage.Text += " | ERROR: From email is missing in Web.config";
                    return false;
                }

                if (string.IsNullOrEmpty("bufymiapthkeajvw"))
                {
                    lblMessage.Text += " | ERROR: Email password is missing in Web.config";
                    return false;
                }

                lblMessage.Text += $" | Debug: Sending from {fromEmail} to {emailAddress}";

                // Try sending with basic SMTP (bypass EmailService for testing)
                using (var smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("mphathisitole@gmail.com", "bufymiapthkeajvw");
                    smtp.EnableSsl = true;
                    smtp.Timeout = 15000;

                    var mail = new MailMessage();
                    mail.From = new MailAddress("mphathisitole@gmail.com");
                    mail.To.Add(emailAddress);
                    mail.Subject = "Email Verification - Complaint System";
                    mail.Body = $"Your verification code is: {code}. This code will expire in 30 minutes.";
                    mail.IsBodyHtml = false;

                    smtp.Send(mail);

                    lblMessage.Text += " | SUCCESS: Email sent directly via SMTP";
                    return true;
                }
            }
            catch (Exception ex)
            {
                // Show detailed error information
                lblMessage.Text += $" | EMAIL ERROR: {ex.Message}";
                if (ex.InnerException != null)
                {
                    lblMessage.Text += $" | INNER: {ex.InnerException.Message}";
                }
                return false;
            }
        }


    }
}
