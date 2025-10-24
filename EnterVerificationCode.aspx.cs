using System;
using System.Linq;
using System.Web.Security;
using ComplaintSystem.Models;
using System.Net;
using System.Net.Mail;
using System.Web.UI;

namespace ComplaintSystem
{
    public partial class EnterVerificationCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["PendingUser"] == null || Session["VerificationCode"] == null)
                {
                    Response.Redirect("RegisterComplainant.aspx");
                }
            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            string enteredCode = txtVerificationCode.Text.Trim();

            if (string.IsNullOrEmpty(enteredCode))
            {
                ShowMessage("Please enter the verification code.", "danger");
                return;
            }

            string storedCode = Session["VerificationCode"]?.ToString();
            DateTime? expiry = Session["CodeExpiry"] as DateTime?;

            if (expiry.HasValue && DateTime.Now > expiry.Value)
            {
                ShowMessage("Verification code has expired. Please request a new one.", "danger");
                ClearSessions();
                return;
            }

            if (enteredCode == storedCode)
            {
                // Code is valid - save user to database
                var pendingUser = Session["PendingUser"] as User;

                using (var context = new AppDbContext())
                {
                    context.Users.Add(pendingUser);
                    context.SaveChanges();
                }

                ShowMessage("Email verified successfully! You can now login.", "success");
                ClearSessions();

                // Redirect to login after 3 seconds
                ClientScript.RegisterStartupScript(this.GetType(), "redirect",
                    "setTimeout(function() { window.location = 'Login.aspx'; }, 3000);", true);
            }
            else
            {
                ShowMessage("Invalid verification code. Please try again.", "danger");
            }
        }

        protected void btnResend_Click(object sender, EventArgs e)
        {
            var pendingUser = Session["PendingUser"] as User;
            if (pendingUser == null)
            {
                Response.Redirect("RegisterComplainant.aspx");
                return;
            }

            // Generate new verification code
            Random random = new Random();
            string newVerificationCode = random.Next(100000, 999999).ToString();

            Session["VerificationCode"] = newVerificationCode;
            Session["CodeExpiry"] = DateTime.Now.AddMinutes(10);

            if (SendVerificationEmail(pendingUser.Email, newVerificationCode))
            {
                ShowMessage("New verification code sent to your email.", "success");
            }
            else
            {
                ShowMessage("Failed to send verification email. Please try again.", "danger");
            }
        }

        private bool SendVerificationEmail(string emailAddress, string code)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential("mphathisitole@gmail.com", "uxebmuuqppvnadjl"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("mphathisitole@gmail.com"),
                    Subject = "New Verification Code - Complaint System",
                    Body = $@"
Hello,

Your new verification code is: {code}

This code will expire in 10 minutes.

Thank you,
Complaint System Team
                    ",
                    IsBodyHtml = false,
                };
                mailMessage.To.Add(emailAddress);

                smtpClient.Send(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Email send failed: {ex.Message}");
                return false;
            }
        }

        private void ShowMessage(string message, string type)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = type == "success" ? "alert alert-success" : "alert alert-danger";
            lblMessage.Visible = true;
        }

        private void ClearSessions()
        {
            Session.Remove("PendingUser");
            Session.Remove("VerificationCode");
            Session.Remove("CodeExpiry");
        }
    }
}