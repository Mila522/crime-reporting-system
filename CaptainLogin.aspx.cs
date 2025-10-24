using System;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using ComplaintSystem.Models;

namespace ComplaintSystem
{
    public partial class CaptainLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Clear any existing authentication
                if (User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    Session.Clear();
                }
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                lblMessage.Text = "Please fill in all required fields.";
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
                return;
            }

            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text.Trim();
            string officerId = txtOfficerID.Text.Trim();

            try
            {
                // Validate user directly against your custom Users table
                using (var context = new AppDbContext())
                {
                    var user = context.Users.FirstOrDefault(u =>
                        u.Email == email && u.Role == "Captain");

                    if (user == null)
                    {
                        lblMessage.Text = "Invalid Captain credentials.";
                        lblMessage.CssClass = "alert alert-danger";
                        lblMessage.Visible = true;
                        return;
                    }

                    // Validate Officer ID
                    if (user.OfficerID != officerId)
                    {
                        lblMessage.Text = "Invalid Officer ID.";
                        lblMessage.CssClass = "alert alert-danger";
                        lblMessage.Visible = true;
                        return;
                    }

                    // Password validation
                    string hashedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
                    if (user.PasswordHash != hashedPassword)
                    {
                        lblMessage.Text = "Invalid password.";
                        lblMessage.CssClass = "alert alert-danger";
                        lblMessage.Visible = true;
                        return;
                    }

                    // Set authentication cookie - use the same format as your main login
                    string userData = $"{user.Email}|Captain";
                    FormsAuthentication.SetAuthCookie(userData, false);


                    // Clear redirect flag
                    Session["HasRedirected"] = false;

                    // Redirect to CaptainDashboard
                    Response.Redirect("~/CaptainDashboard.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error during Captain login: {ex.Message}";
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }
        }
    }
}