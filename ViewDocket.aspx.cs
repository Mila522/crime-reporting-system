using System;
using System.Linq;
using System.Web.UI.WebControls;
using ComplaintSystem.Models;
using System.Data.Entity;

namespace ComplaintSystem
{
    public partial class ViewDockets : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                string[] userData = User.Identity.Name.Split('|');
                if (userData.Length != 2 || userData[1] != "ChargeOfficer")
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                try
                {
                    BindDockets();
                }
                catch (Exception ex)
                {
                    string errorMessage = "Error loading dockets: " + ex.Message;
                    if (ex.InnerException != null)
                    {
                        errorMessage += " Inner Exception: " + ex.InnerException.Message;
                        if (ex.InnerException.InnerException != null)
                        {
                            errorMessage += " Inner Inner Exception: " + ex.InnerException.InnerException.Message;
                        }
                    }
                    lblMessage.Text = errorMessage;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                }
            }
        }

        private void BindDockets()
        {
            using (var context = new AppDbContext())
            {
                string identifier = User.Identity.Name.Split('|')[0];
                var user = context.Users.FirstOrDefault(u =>
                    (u.Email == identifier || u.PhoneNumber == identifier) && u.Role == "ChargeOfficer");

                if (user != null)
                {
                    var dockets = context.Dockets
                        .Include(d => d.CaseFile)
                        .Include(d => d.InvestigationEntries)
                        .Include(d => d.AccusedPersons)
                        .Where(d => context.CaseFiles
                            .Where(cf => cf.ChargeOfficerId == user.Id && cf.Id > 0 && cf.ComplaintId > 0)
                            .Select(cf => cf.Id)
                            .Contains(d.CaseFileId))
                        .Select(d => new
                        {
                            d.Id,
                            d.CaseFileId,
                            d.ComplaintId,
                            d.Details,
                            d.CreatedDate,
                            d.DeceasedSurname,
                            d.DeceasedChristianNames,
                            d.DeceasedIdentityNumber,
                            d.DeceasedDateOfDeath,
                            d.SAPS13RefNo,
                            d.SAPS43RefNo,
                            InvestigationEntryCount = d.InvestigationEntries.Count,
                            AccusedCount = d.AccusedPersons.Count
                        })
                        .ToList()
                        .OrderByDescending(d => d.CreatedDate.HasValue ? d.CreatedDate.Value : DateTime.MinValue)
                        .ToList();

                    rptDockets.DataSource = dockets;
                    rptDockets.DataBind();

                    pnlEmptyDockets.Visible = !dockets.Any();
                    if (!dockets.Any())
                    {
                        lblMessage.Text = "No dockets found for this Charge Officer.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                    }
                }
                else
                {
                    pnlEmptyDockets.Visible = true;
                    lblMessage.Text = "Charge Officer not found.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ChargeOfficerDashboard.aspx");
        }
    }
}