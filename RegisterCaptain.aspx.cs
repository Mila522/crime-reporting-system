using System;
using System.Linq;
using System.Web.Security;
using System.Web.UI;
using ComplaintSystem.Models;

namespace ComplaintSystem
{
    public partial class RegisterCaptain : Page
    {
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                lblMessage.Text = "Please fill in all required fields.";
                return;
            }

            string email = txtEmail.Text;
            string phoneNumber = txtPhoneNumber.Text;
            string password = txtPassword.Text;
            string officerID = txtOfficerID.Text;
            string firstNames = txtFirstNames.Text;
            string surname = txtSurname.Text;
            string idNumber = txtIDNumber.Text;

            try
            {
                using (var context = new AppDbContext())
                {
                    // Generate unique username
                    string username = email.Split('@')[0] + "_" + Guid.NewGuid().ToString().Substring(0, 4);

                    // Check for existing user
                    var existingUser = context.Users.FirstOrDefault(u => u.Email == email || u.PhoneNumber == phoneNumber || u.IDNumber == idNumber || u.OfficerID == officerID);
                    if (existingUser != null)
                    {
                        lblMessage.Text = "A user with this email, phone number, ID number, or Officer ID already exists.";
                        System.Diagnostics.Debug.WriteLine("Duplicate user detected.");
                        return;
                    }

                    // Create user in Users table
                    var user = new User
                    {
                        Username = username,
                        PasswordHash = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1"),
                        Role = "Captain",
                        OfficerID = officerID,
                        PhoneNumber = phoneNumber,
                        Email = email,
                        ResidentialAddress = "",
                        FirstNames = firstNames,
                        Surname = surname,
                        IDNumber = idNumber
                    };
                    context.Users.Add(user);
                    context.SaveChanges();

                    // Create ASP.NET Membership user
                    MembershipCreateStatus status;
                    Membership.CreateUser(username, password, email, null, null, true, out status);
                    if (status != MembershipCreateStatus.Success)
                    {
                        lblMessage.Text = $"Failed to create user: {status}";
                        System.Diagnostics.Debug.WriteLine($"Membership creation failed: {status}");
                        return;
                    }

                    // Add user to Captain role
                    if (!Roles.RoleExists("Captain"))
                        Roles.CreateRole("Captain");
                    if (!Roles.IsUserInRole(username, "Captain"))
                        Roles.AddUsersToRoles(new[] { username }, new[] { "Captain" });

                    lblMessage.Text = "Registration successful. Please log in.";
                    System.Diagnostics.Debug.WriteLine($"User registered: {username}, Role=Captain");
                    Response.Redirect("~/Login.aspx", true);
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error registering user: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Error in btnRegister_Click: {ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}