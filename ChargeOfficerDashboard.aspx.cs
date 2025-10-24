using System;
using System.Linq;
using System.Web.UI.WebControls;
using ComplaintSystem.Models;
using System.Data.Entity;
using System.Web.Security;

namespace ComplaintSystem
{
    public partial class ChargeOfficerDashboard : System.Web.UI.Page
    {
        // Add these protected fields to the partial class to fix missing control references
        // Place these at the top of the ChargeOfficerDashboard class, after the class declaration

        protected global::System.Web.UI.WebControls.DropDownList ddlCrimeTypeFilter;
        protected global::System.Web.UI.WebControls.Repeater rptComplaints;
        protected global::System.Web.UI.WebControls.Panel pnlEmptyComplaints;
        protected global::System.Web.UI.WebControls.Repeater rptCaseFiles;
        protected global::System.Web.UI.WebControls.Panel pnlEmptyCaseFiles;
        protected global::System.Web.UI.WebControls.Repeater rptDockets;
        protected global::System.Web.UI.WebControls.Panel pnlEmptyDockets;
        private bool HasRedirected
        {
            get { return Session["HasRedirected"] != null && (bool)Session["HasRedirected"]; }
            set { Session["HasRedirected"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (HasRedirected)
                {
                    lblMessage.Text = "Redirect loop detected. Please check login status or query parameters.";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                    System.Diagnostics.Trace.WriteLine("Redirect loop detected in ChargeOfficerDashboard.aspx");
                    return;
                }

                if (!User.Identity.IsAuthenticated)
                {
                    System.Diagnostics.Trace.WriteLine("User not authenticated, redirecting to Login.aspx");
                    HasRedirected = true;
                    Response.Redirect("~/Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                string[] userData = User.Identity.Name.Split('|');
                if (userData.Length != 2 || userData[1] != "ChargeOfficer")
                {
                    System.Diagnostics.Trace.WriteLine($"Invalid user data or role: {User.Identity.Name}");
                    HasRedirected = true;
                    Response.Redirect("~/Login.aspx", false);
                    Context.ApplicationInstance.CompleteRequest();
                    return;
                }

                try
                {
                    lblMessage.Text = "Loading dashboard...";
                    lblMessage.ForeColor = System.Drawing.Color.Orange;
                    lblMessage.Visible = true;
                    BindComplaints();
                    BindCaseFiles();
                    BindDockets();
                    lblMessage.Visible = false;
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Error loading dashboard data: {ex.Message}";
                    if (ex.InnerException != null)
                        lblMessage.Text += $" Inner Exception: {ex.InnerException.Message}";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                    System.Diagnostics.Trace.WriteLine($"Error loading dashboard: {ex.Message}");
                }
            }
        }

        protected void ddlCrimeTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Text = $"Filtering complaints by: {ddlCrimeTypeFilter.SelectedValue}";
                lblMessage.ForeColor = System.Drawing.Color.Orange;
                lblMessage.Visible = true;
                BindComplaints();
                lblMessage.Visible = false;
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error filtering complaints: {ex.Message}";
                if (ex.InnerException != null)
                    lblMessage.Text += $" Inner Exception: {ex.InnerException.Message}";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
                System.Diagnostics.Trace.WriteLine($"Error filtering complaints: {ex.Message}");
            }
        }

        protected void btnViewDockets_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Text = "Redirecting to Dockets.aspx...";
                lblMessage.ForeColor = System.Drawing.Color.Orange;
                lblMessage.Visible = true;
                HasRedirected = false;
                Response.Redirect("~/Dockets.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error redirecting to Dockets: {ex.Message}";
                if (ex.InnerException != null)
                    lblMessage.Text += $" Inner Exception: {ex.InnerException.Message}";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
                System.Diagnostics.Trace.WriteLine($"Error redirecting to Dockets: {ex.Message}");
            }
        }

        private void BindComplaints()
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    string crimeTypeFilter = ddlCrimeTypeFilter.SelectedValue;

                    var complaintsQuery = context.Complaints
                        .Include(c => c.Complainant)
                        .Where(c => c.Status == ComplaintStatus.Submitted || c.Status == ComplaintStatus.InProgress);

                    if (!string.IsNullOrEmpty(crimeTypeFilter))
                        complaintsQuery = complaintsQuery.Where(c => c.CrimeType == crimeTypeFilter);

                    var complaints = complaintsQuery
                        .Select(c => new
                        {
                            c.Id,
                            c.Description,
                            CrimeType = c.CrimeType ?? "Other",
                            AccusedNumber = c.AccusedNumber ?? "Not Specified",
                            c.Status,
                            c.CreatedDate,
                            ComplainantName = c.Complainant != null ? c.Complainant.Username ?? "Unknown" : "Unknown"
                        })
                        .ToList();

                    var validComplaints = complaints
                        .Where(c => c.Id > 0)
                        .OrderByDescending(c => c.CreatedDate.HasValue ? c.CreatedDate.Value : DateTime.MinValue)
                        .ToList();

                    if (complaints.Any(c => c.Id <= 0))
                    {
                        lblMessage.Text = "Warning: Some complaints have invalid IDs (≤ 0).";
                        lblMessage.ForeColor = System.Drawing.Color.Orange;
                        lblMessage.Visible = true;
                        System.Diagnostics.Trace.WriteLine("Invalid complaint IDs detected");
                    }

                    rptComplaints.DataSource = validComplaints;
                    rptComplaints.DataBind();

                    pnlEmptyComplaints.Visible = !validComplaints.Any();
                    if (!validComplaints.Any())
                    {
                        lblMessage.Text = "No valid complaints found.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Error binding complaints: {ex.Message}";
                    if (ex.InnerException != null)
                        lblMessage.Text += $" Inner Exception: {ex.InnerException.Message}";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                    System.Diagnostics.Trace.WriteLine($"Error binding complaints: {ex.Message}");
                }
            }
        }

        private void BindCaseFiles()
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    string identifier = User.Identity.Name.Split('|')[0];
                    var user = context.Users.FirstOrDefault(u =>
                        (u.Email == identifier || u.PhoneNumber == identifier) && u.Role == "ChargeOfficer");

                    if (user == null)
                    {
                        lblMessage.Text = $"Charge Officer not found for identifier: {identifier}";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        pnlEmptyCaseFiles.Visible = true;
                        System.Diagnostics.Trace.WriteLine($"Charge Officer not found: {identifier}");
                        return;
                    }

                    var caseFiles = context.CaseFiles
                        .Include(cf => cf.Complaint)
                        .Include(cf => cf.Witnesses)
                        .Include(cf => cf.Dockets)
                        .Where(cf => cf.ChargeOfficerId == user.Id && cf.Id > 0 && cf.ComplaintId > 0 && !cf.Dockets.Any())
                        .Select(cf => new
                        {
                            cf.Id,
                            cf.ComplaintId,
                            cf.Details,
                            cf.CrimeType,
                            cf.ComplaintStatus,
                            cf.CreatedDate,
                            cf.IncidentLocation,
                            cf.Station,
                            cf.AgenciesInvolved,
                            cf.Obstacles,
                            cf.PendingActions,
                            WitnessCount = cf.Witnesses.Count(),
                            ComplaintDescription = cf.Complaint != null ? cf.Complaint.Description ?? "No Description" : "No Complaint"
                        })
                        .ToList();

                    if (!caseFiles.Any())
                    {
                        lblMessage.Text = $"No case files found for ChargeOfficerId: {user.Id}";
                        lblMessage.ForeColor = System.Drawing.Color.Orange;
                        lblMessage.Visible = true;
                    }

                    var validCaseFiles = caseFiles
                        .OrderByDescending(cf => cf.CreatedDate.HasValue ? cf.CreatedDate.Value : DateTime.MinValue)
                        .ToList();

                    rptCaseFiles.DataSource = validCaseFiles;
                    rptCaseFiles.DataBind();

                    pnlEmptyCaseFiles.Visible = !validCaseFiles.Any();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Error binding case files: {ex.Message}";
                    if (ex.InnerException != null)
                        lblMessage.Text += $" Inner Exception: {ex.InnerException.Message}";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                    pnlEmptyCaseFiles.Visible = true;
                    System.Diagnostics.Trace.WriteLine($"Error binding case files: {ex.Message}");
                }
            }
        }

        private void BindDockets()
        {
            using (var context = new AppDbContext())
            {
                try
                {
                    string identifier = User.Identity.Name.Split('|')[0];
                    var user = context.Users.FirstOrDefault(u =>
                        (u.Email == identifier || u.PhoneNumber == identifier) && u.Role == "ChargeOfficer");

                    if (user == null)
                    {
                        lblMessage.Text = $"Charge Officer not found for identifier: {identifier}";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        pnlEmptyDockets.Visible = true;
                        System.Diagnostics.Trace.WriteLine($"Charge Officer not found for dockets: {identifier}");
                        return;
                    }

                    var dockets = context.Dockets
                        .Include(d => d.CaseFile)
                        .Include(d => d.InvestigationEntries)
                        .Include(d => d.AccusedPersons)
                        .Include(d => d.EvidenceRecords)
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
                            InvestigationEntryCount = d.InvestigationEntries != null ? d.InvestigationEntries.Count() : 0,
                            AccusedCount = d.AccusedPersons != null ? d.AccusedPersons.Count() : 0,
                            EvidenceRecordCount = d.EvidenceRecords != null ? d.EvidenceRecords.Count() : 0
                        })
                        .ToList();

                    if (!dockets.Any())
                    {
                        lblMessage.Text = $"No dockets found for ChargeOfficerId: {user.Id}";
                        lblMessage.ForeColor = System.Drawing.Color.Orange;
                        lblMessage.Visible = true;
                    }

                    var validDockets = dockets
                        .OrderByDescending(d => d.CreatedDate.HasValue ? d.CreatedDate.Value : DateTime.MinValue)
                        .ToList();

                    rptDockets.DataSource = validDockets;
                    rptDockets.DataBind();

                    pnlEmptyDockets.Visible = !validDockets.Any();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Error binding dockets: {ex.Message}";
                    if (ex.InnerException != null)
                        lblMessage.Text += $" Inner Exception: {ex.InnerException.Message}";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                    pnlEmptyDockets.Visible = true;
                    System.Diagnostics.Trace.WriteLine($"Error binding dockets: {ex.Message}");
                }
            }
        }

        protected void rptComplaints_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "CreateCaseFile")
            {
                try
                {
                    string commandArg = e.CommandArgument?.ToString();
                    if (string.IsNullOrEmpty(commandArg))
                    {
                        lblMessage.Text = "CommandArgument is null or empty in rptComplaints.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        System.Diagnostics.Trace.WriteLine("Null or empty CommandArgument in rptComplaints_ItemCommand");
                        return;
                    }

                    if (!int.TryParse(commandArg, out int complaintId) || complaintId <= 0)
                    {
                        lblMessage.Text = $"Invalid Complaint ID in CommandArgument: '{commandArg}'.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        System.Diagnostics.Trace.WriteLine($"Invalid Complaint ID: {commandArg}");
                        return;
                    }

                    using (var context = new AppDbContext())
                    {
                        var complaint = context.Complaints.Find(complaintId);
                        if (complaint == null)
                        {
                            lblMessage.Text = $"Complaint with ID {complaintId} not found in database.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Visible = true;
                            System.Diagnostics.Trace.WriteLine($"Complaint not found: {complaintId}");
                            return;
                        }
                    }

                    string redirectUrl = $"~/CreateCaseFile.aspx?complaintId={complaintId}";
                    lblMessage.Text = $"Redirecting to: {redirectUrl}";
                    lblMessage.ForeColor = System.Drawing.Color.Orange;
                    lblMessage.Visible = true;
                    HasRedirected = false;
                    Response.Redirect(redirectUrl, false);
                    Context.ApplicationInstance.CompleteRequest();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Error redirecting to CreateCaseFile: {ex.Message}";
                    if (ex.InnerException != null)
                        lblMessage.Text += $" Inner Exception: {ex.InnerException.Message}";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                    System.Diagnostics.Trace.WriteLine($"Error redirecting to CreateCaseFile: {ex.Message}");
                }
            }
        }

        protected void rptCaseFiles_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "CreateDocket")
            {
                try
                {
                    string commandArg = e.CommandArgument?.ToString();
                    if (string.IsNullOrEmpty(commandArg))
                    {
                        lblMessage.Text = "CommandArgument is null or empty in rptCaseFiles.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        System.Diagnostics.Trace.WriteLine("Null or empty CommandArgument in rptCaseFiles_ItemCommand");
                        return;
                    }

                    if (!int.TryParse(commandArg, out int caseFileId) || caseFileId <= 0)
                    {
                        lblMessage.Text = $"Invalid Case File ID in CommandArgument: '{commandArg}'.";
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Visible = true;
                        System.Diagnostics.Trace.WriteLine($"Invalid Case File ID: {commandArg}");
                        return;
                    }

                    using (var context = new AppDbContext())
                    {
                        var caseFileRecord = context.CaseFiles
                            .Include(cfr => cfr.Complaint)
                            .FirstOrDefault(cfr => cfr.Id == caseFileId);

                        if (caseFileRecord == null)
                        {
                            lblMessage.Text = $"Case File with ID {caseFileId} not found in database.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Visible = true;
                            System.Diagnostics.Trace.WriteLine($"Case File not found: {caseFileId}");
                            return;
                        }

                        if (caseFileRecord.ComplaintId <= 0)
                        {
                            lblMessage.Text = $"Case File {caseFileId} has invalid ComplaintId: {caseFileRecord.ComplaintId}.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Visible = true;
                            System.Diagnostics.Trace.WriteLine($"Invalid ComplaintId for Case File: {caseFileId}");
                            return;
                        }

                        // Check if a docket already exists to prevent unnecessary redirects
                        if (context.Dockets.Any(d => d.CaseFileId == caseFileId))
                        {
                            lblMessage.Text = $"A docket already exists for Case File ID {caseFileId}.";
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Visible = true;
                            System.Diagnostics.Trace.WriteLine($"Docket already exists for Case File: {caseFileId}");
                            return;
                        }

                        string redirectUrl = $"~/DocketEntry.aspx?caseFileId={caseFileId}&complaintId={caseFileRecord.ComplaintId}";
                        lblMessage.Text = $"Redirecting to: {redirectUrl}";
                        lblMessage.ForeColor = System.Drawing.Color.Orange;
                        lblMessage.Visible = true;
                        HasRedirected = false;
                        Response.Redirect(redirectUrl, false);
                        Context.ApplicationInstance.CompleteRequest();
                    }
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Error redirecting to DocketEntry: {ex.Message}";
                    if (ex.InnerException != null)
                        lblMessage.Text += $" Inner Exception: {ex.InnerException.Message}";
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                    System.Diagnostics.Trace.WriteLine($"Error redirecting to DocketEntry: {ex.Message}");
                }
            }
        }
        protected void btnCaptainLogin_Click(object sender, EventArgs e)
        {
            try
            {
                lblMessage.Text = "Redirecting to Captain login...";
                lblMessage.ForeColor = System.Drawing.Color.Orange;
                lblMessage.Visible = true;
                HasRedirected = false;

                // Sign out current user to ensure clean Captain login
                System.Web.Security.FormsAuthentication.SignOut();
                Session.Clear();

                // Redirect to separate Captain login page
                Response.Redirect("~/CaptainLogin.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error redirecting to Captain login: {ex.Message}";
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Visible = true;
                System.Diagnostics.Trace.WriteLine($"Error redirecting to Captain login: {ex.Message}");
            }
        }
        protected void btnLogout_Click(object sender, EventArgs e)
        {
            try
            {
                // Clear authentication cookie
                FormsAuthentication.SignOut();

                // Clear session
                Session.Clear();
                Session.Abandon();

                // Clear redirect flag
                Session.Remove("HasRedirected");

                // Redirect to login page
                Response.Redirect("~/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Logout error: {ex.Message}");
                Response.Redirect("~/Login.aspx");
            }
        }



    }
}