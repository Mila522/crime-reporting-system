using System;

using System.Linq;

using System.Web.Security;

using System.Web.UI;

using System.Net;

using ComplaintSystem.Models;

using ComplaintSystem.Services;

using System.Net.Mail;

using System.Threading.Tasks;



namespace ComplaintSystem

{

    public partial class ForgotPassword : System.Web.UI.Page

    {

        protected void Page_Load(object sender, EventArgs e)

        {

        }



        protected void btnSendPassword_Click(object sender, EventArgs e)

        {

            string email = txtEmail.Text.Trim();



            if (string.IsNullOrEmpty(email))

            {

                ShowMessage("Please enter your email address", false);

                return;

            }



            using (var context = new AppDbContext())

            {

                var user = context.Users.FirstOrDefault(u => u.Email == email);

                if (user == null)

                {

                    ShowMessage("No account found with this email address", false);

                    return;

                }



                // Generate temporary password

                string tempPassword = GenerateTemporaryPassword();

                user.PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(tempPassword, "SHA1");

                context.SaveChanges();



                // Send password via email

                if (SendPasswordEmail(user.Email, tempPassword))

                {

                    ShowMessage("Your password has been sent to your email address", true);

                }

                else

                {

                    ShowMessage("Failed to send email. Please try again.", false);

                }

            }

        }



        private string GenerateTemporaryPassword()

        {

            var random = new Random();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            return new string(Enumerable.Repeat(chars, 8)

                .Select(s => s[random.Next(s.Length)]).ToArray());

        }



        private bool SendPasswordEmail(string emailAddress, string password)

        {

            try

            {

                var emailService = new EmailService();

                var subject = "Your Password - Complaint System";

                var body = $@"Hello,



Your password is: {password}



Please login and change your password immediately for security reasons.



Thank you,

Complaint System Team";



                return emailService.SendEmailAsync(emailAddress, subject, body).Result;

            }

            catch (Exception ex)

            {

                System.Diagnostics.Debug.WriteLine($"Email send failed: {ex.Message}");

                return false;

            }

        }



        private void ShowMessage(string message, bool isSuccess)

        {

            lblMessage.Text = message;

            lblMessage.CssClass = isSuccess ? "alert alert-success" : "alert alert-danger";

            lblMessage.Visible = true;

        }


    }

}