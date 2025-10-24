using System;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using ComplaintSystem.Models;

namespace ComplaintSystem
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // REMOVE the automatic redirect for authenticated users
            // This is what's causing the issue - it redirects before user can choose role
            if (!IsPostBack)
            {
                lblLoginIdentifier.Text = "Email:";

                // Clear any existing authentication to prevent auto-redirect
                if (User.Identity.IsAuthenticated)
                {
                    FormsAuthentication.SignOut();
                    Session.Clear();
                }

                string roleQuery = Request.QueryString["role"];
                if (!string.IsNullOrEmpty(roleQuery) && roleQuery == "Captain")
                {
                    ViewState["SelectedRole"] = "Captain";
                    divOfficerID.Visible = true;
                    rfvOfficerID.Enabled = true;
                    btnRegister.Visible = false;

                    // Hide the other role buttons for Captain login
                    btnComplainant.Visible = false;
                    btnChargeOfficer.Visible = false;
                }
                else
                {
                    // Default to no role selected
                    ViewState["SelectedRole"] = "";
                    divOfficerID.Visible = false;
                    rfvOfficerID.Enabled = false;
                    btnRegister.Visible = false;
                }
            }
        }


        protected void btnRole_Click(object sender, EventArgs e)
        {
            var button = (System.Web.UI.WebControls.Button)sender;
            string role = button.CommandArgument;

            // Reset both first
            divOfficerID.Visible = false;
            rfvOfficerID.Enabled = false;
            btnRegister.Visible = false;

            if (role == "ChargeOfficer")
            {
                divOfficerID.Visible = true;
                rfvOfficerID.Enabled = true;
            }
            else if (role == "Complainant")
            {
                btnRegister.Visible = true;
            }

            ViewState["SelectedRole"] = role;

            // Clear any previous messages
            lblMessage.Text = "";
        }

        protected void rblLoginMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblLoginIdentifier.Text = rblLoginMethod.SelectedValue == "Email"
                ? "Email:"
                : "Phone Number:";
        }

        [Obsolete]
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                lblMessage.Text = "Please fill in all required fields.";
                System.Diagnostics.Trace.WriteLine("Login validation failed: Required fields missing");
                return;
            }



            // Check if role is selected
            string role = ViewState["SelectedRole"]?.ToString();
            if (string.IsNullOrEmpty(role))
            {
                lblMessage.Text = "Please select a role (Complainant or Admin) first.";
                System.Diagnostics.Trace.WriteLine("No role selected");
                return;
            }

            string loginIdentifier = txtLoginIdentifier.Text.Trim();
            string password = txtPassword.Text.Trim();
            string officerId = txtOfficerID.Text.Trim();

            try
            {
                using (var context = new AppDbContext())
                {
                    User user = null;

                    // Login by email or phone
                    if (rblLoginMethod.SelectedValue == "Email")
                        user = context.Users.FirstOrDefault(u => u.Email == loginIdentifier && u.Role == role);
                    else
                        user = context.Users.FirstOrDefault(u => u.PhoneNumber == loginIdentifier && u.Role == role);

                    if (user == null)
                    {
                        lblMessage.Text = "Invalid login credentials or role mismatch.";
                        System.Diagnostics.Trace.WriteLine($"User not found for {rblLoginMethod.SelectedValue}: {loginIdentifier}, Role: {role}");
                        return;
                    }

                    // Officer validation
                    if (role == "ChargeOfficer")
                    {
                        if (string.IsNullOrWhiteSpace(officerId))
                        {
                            lblMessage.Text = "Officer ID is required for Admin login.";
                            System.Diagnostics.Trace.WriteLine("Officer ID missing for ChargeOfficer login");
                            return;
                        }
                        if (user.OfficerID != officerId)
                        {
                            lblMessage.Text = "Invalid Officer ID.";
                            System.Diagnostics.Trace.WriteLine($"Invalid Officer ID: {officerId} for user {user.Email}");
                            return;
                        }
                    }

                    // Password validation
                    if (user.PasswordHash != FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1"))
                    {
                        lblMessage.Text = "Invalid password.";
                        System.Diagnostics.Trace.WriteLine($"Invalid password for user {user.Email}");
                        return;
                    }

                    // Store EMAIL in the cookie for consistency
                    string userData = $"{user.Email}|{role}";
                    FormsAuthentication.SetAuthCookie(userData, false);
                    System.Diagnostics.Trace.WriteLine($"Successful login for {user.Email}, Role: {role}");

                    // Clear redirect flag
                    Session["HasRedirected"] = false;

                    // Redirect per role
                    switch (role)
                    {
                        case "Complainant":
                            Response.Redirect("~/ComplainantDashboard.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                            break;
                        case "ChargeOfficer":
                            Response.Redirect("~/ChargeOfficerDashboard.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                            break;
                        case "Captain":
                            Response.Redirect("~/CaptainDashboard.aspx", false);
                            Context.ApplicationInstance.CompleteRequest();
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error during login: {ex.Message}";
                System.Diagnostics.Trace.WriteLine($"Login error: {ex.Message}");
                if (ex.InnerException != null)
                    System.Diagnostics.Trace.WriteLine($"Inner exception: {ex.InnerException.Message}");
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Trace.WriteLine("Redirecting to RegisterComplainant.aspx");
            Session["HasRedirected"] = false;
            Response.Redirect("~/RegisterComplainant.aspx", false);
            Context.ApplicationInstance.CompleteRequest();
        }


    }
}