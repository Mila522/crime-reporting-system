using System;
using System.Linq;
using System.Web.UI;
using ComplaintSystem.Models;
using ComplaintSystem.Services;

namespace ComplaintSystem
{
    public partial class VerifyEmail : System.Web.UI.Page
    {
        private string UserEmail
        {
            get { return Session["VerificationEmail"] as string; }
            set { Session["VerificationEmail"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(UserEmail))
                {
                    Response.Redirect("~/RegisterComplainant.aspx");
                }
                else
                {
                    // Check if we need to send initial verification code
                    // This handles the case where the initial email failed during registration
                    using (var context = new AppDbContext())
                    {
                        var user = context.Users.FirstOrDefault(u => u.Email == UserEmail && !u.IsVerified);
                        if (user != null && string.IsNullOrEmpty(user.VerificationCode))
                        {
                            // User exists but has no verification code, generate and send one
                            SendVerificationCode(UserEmail);
                            ShowMessage("Verification code sent to your email", true);
                        }
                        else if (user != null)
                        {
                            // User has a verification code already
                            ShowMessage("Please check your email for the verification code", true);
                        }
                    }
                }
            }
        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            string code = txtVerificationCode.Text.Trim();

            if (string.IsNullOrEmpty(code))
            {
                ShowMessage("Please enter verification code", false);
                return;
            }

            using (var context = new AppDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == UserEmail);

                if (user == null || user.VerificationCode != code || user.VerificationCodeExpiry < DateTime.Now)
                {
                    ShowMessage("Invalid or expired verification code", false);
                    return;
                }

                // Mark user as verified
                user.IsVerified = true;
                user.VerificationCode = null;
                user.VerificationCodeExpiry = null;
                context.SaveChanges();

                ShowMessage("Email verified successfully! You can now login.", true);

                // Redirect to login after 3 seconds
                ScriptManager.RegisterStartupScript(this, GetType(), "redirect",
                    "setTimeout(function(){ window.location.href = 'Login.aspx'; }, 3000);", true);
            }
        }

        protected void btnResend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(UserEmail))
            {
                SendVerificationCode(UserEmail);
                ShowMessage("New verification code sent to your email", true);
            }
        }

        private void SendVerificationCode(string email)
        {
            using (var context = new AppDbContext())
            {
                var user = context.Users.FirstOrDefault(u => u.Email == email);
                if (user != null)
                {
                    var verificationCode = GenerateVerificationCode();
                    user.VerificationCode = verificationCode;
                    user.VerificationCodeExpiry = DateTime.Now.AddMinutes(30);
                    context.SaveChanges();

                    var emailService = new EmailService();
                    var subject = "Email Verification Code";
                    var body = $"Your verification code is: {verificationCode}\nThis code will expire in 30 minutes.";

                    emailService.SendEmailAsync(email, subject, body);
                }
            }
        }

        private string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";
            lblMessage.Visible = true;
        }
    }
}