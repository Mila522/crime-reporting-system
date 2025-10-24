using System;
using System.Linq;
using System.Web.UI.WebControls;
using ComplaintSystem.Models;
using System.Data.Entity;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Security;

namespace ComplaintSystem
{
    public partial class CaptainDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Debug: Check what's in the authentication
            System.Diagnostics.Trace.WriteLine($"CaptainDashboard - Authenticated: {User.Identity.IsAuthenticated}");
            System.Diagnostics.Trace.WriteLine($"CaptainDashboard - User Name: {User.Identity.Name}");

            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("CaptainLogin.aspx");
                return;
            }

            // Parse the user data from authentication cookie
            string[] userData = User.Identity.Name.Split('|');
            if (userData.Length != 2 || userData[1] != "Captain")
            {
                System.Diagnostics.Trace.WriteLine($"Invalid role in CaptainDashboard. UserData: {User.Identity.Name}");
                Response.Redirect("CaptainLogin.aspx");
                return;
            }

            // User is authenticated as Captain - load the dashboard
            if (!IsPostBack)
            {
                try
                {
                    BindCaseFiles();
                    BindDockets();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Error loading data: {ex.Message}";
                    lblMessage.CssClass = "alert alert-danger";
                    lblMessage.Visible = true;
                }
            }
        }

        protected void ddlCrimeTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindCaseFiles();
            BindDockets();
        }

        private void BindCaseFiles()
        {
            using (var context = new AppDbContext())
            {
                var caseFilesQuery = context.CaseFiles
                    .Include(cf => cf.Complaint)
                    .AsQueryable();

                string crimeTypeFilter = ddlCrimeTypeFilter.SelectedValue;
                if (!string.IsNullOrEmpty(crimeTypeFilter))
                {
                    caseFilesQuery = caseFilesQuery.Where(cf => cf.CrimeType == crimeTypeFilter);
                }

                var caseFiles = caseFilesQuery
                    .Select(cf => new
                    {
                        cf.Id,
                        cf.ComplaintId,
                        cf.CrimeType,
                        cf.IncidentLocation,
                        cf.Station,
                        cf.CreatedDate,
                        cf.AgenciesInvolved,
                        cf.Obstacles,
                        cf.PendingActions
                    })
                    .ToList();

                gvCaseFiles.DataSource = caseFiles;
                gvCaseFiles.DataBind();
            }
        }

        private void BindDockets()
        {
            using (var context = new AppDbContext())
            {
                var docketsQuery = context.Dockets
                    .Include(d => d.CaseFile)
                    .AsQueryable();

                string crimeTypeFilter = ddlCrimeTypeFilter.SelectedValue;
                if (!string.IsNullOrEmpty(crimeTypeFilter))
                {
                    docketsQuery = docketsQuery.Where(d => d.CaseFile.CrimeType == crimeTypeFilter);
                }

                var dockets = docketsQuery
                    .Select(d => new
                    {
                        d.Id,
                        d.CaseFileId,
                        d.ComplaintId,
                        d.DeceasedSurname,
                        d.DeceasedChristianNames,
                        d.CreatedDate,
                        d.SAPS13RefNo,
                        d.SAPS43RefNo
                    })
                    .ToList();

                gvDockets.DataSource = dockets;
                gvDockets.DataBind();
            }
        }

        protected void gvCaseFiles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ExportCaseFile")
            {
                int caseFileId = Convert.ToInt32(e.CommandArgument);
                ExportCaseFile(caseFileId);
            }
        }

        protected void gvDockets_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ExportDocket")
            {
                int docketId = Convert.ToInt32(e.CommandArgument);
                ExportDocket(docketId);
            }
        }

        protected void btnExportAll_Click(object sender, EventArgs e)
        {
            ExportAllReports();
        }

        private void ExportCaseFile(int caseFileId)
        {
            using (var context = new AppDbContext())
            {
                var caseFile = context.CaseFiles
                    .Include(cf => cf.Complaint)
                    .Include(cf => cf.Witnesses)
                    .FirstOrDefault(cf => cf.Id == caseFileId);

                if (caseFile != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("CASE FILE REPORT");
                    sb.AppendLine("=================");
                    sb.AppendLine($"Case File ID: {caseFile.Id}");
                    sb.AppendLine($"Complaint ID: {caseFile.ComplaintId}");
                    sb.AppendLine($"Crime Type: {caseFile.CrimeType}");
                    sb.AppendLine($"Incident Location: {caseFile.IncidentLocation}");
                    sb.AppendLine($"Station: {caseFile.Station}");
                    sb.AppendLine($"Created Date: {caseFile.CreatedDate}");
                    sb.AppendLine($"Agencies Involved: {caseFile.AgenciesInvolved}");
                    sb.AppendLine($"Obstacles: {caseFile.Obstacles}");
                    sb.AppendLine($"Pending Actions: {caseFile.PendingActions}");

                    sb.AppendLine("\nWITNESSES:");
                    foreach (var witness in caseFile.Witnesses)
                    {
                        sb.AppendLine($"- {witness.Name}: {witness.Statement}");
                    }

                    DownloadFile(sb.ToString(), $"CaseFile_{caseFileId}.txt");
                }
            }
        }

        private void ExportDocket(int docketId)
        {
            using (var context = new AppDbContext())
            {
                var docket = context.Dockets
                    .Include(d => d.InvestigationEntries)
                    .Include(d => d.AccusedPersons)
                    .FirstOrDefault(d => d.Id == docketId);

                if (docket != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("DOCKET REPORT");
                    sb.AppendLine("=============");
                    sb.AppendLine($"Docket ID: {docket.Id}");
                    sb.AppendLine($"Case File ID: {docket.CaseFileId}");
                    sb.AppendLine($"Complaint ID: {docket.ComplaintId}");
                    sb.AppendLine($"Deceased: {docket.DeceasedSurname}, {docket.DeceasedChristianNames}");
                    sb.AppendLine($"SAPS 13 Ref: {docket.SAPS13RefNo}");
                    sb.AppendLine($"SAPS 43 Ref: {docket.SAPS43RefNo}");

                    sb.AppendLine("\nINVESTIGATION ENTRIES:");
                    foreach (var entry in docket.InvestigationEntries)
                    {
                        sb.AppendLine($"- {entry.EntryDateTime}: {entry.InvestigationDetails}");
                    }

                    sb.AppendLine("\nACCUSED PERSONS:");
                    foreach (var accused in docket.AccusedPersons)
                    {
                        sb.AppendLine($"- {accused.ChristianNames} {accused.Surname}");
                    }

                    DownloadFile(sb.ToString(), $"Docket_{docketId}.txt");
                }
            }
        }

        private void ExportAllReports()
        {
            using (var context = new AppDbContext())
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("COMPREHENSIVE CASE MANAGEMENT REPORT");
                sb.AppendLine("====================================");

                var caseFiles = context.CaseFiles.Include(cf => cf.Witnesses).ToList();
                var dockets = context.Dockets.Include(d => d.InvestigationEntries).Include(d => d.AccusedPersons).ToList();

                sb.AppendLine($"\nTOTAL CASE FILES: {caseFiles.Count}");
                sb.AppendLine($"TOTAL DOCKETS: {dockets.Count}");

                sb.AppendLine("\nCASE FILES SUMMARY:");
                foreach (var cf in caseFiles)
                {
                    sb.AppendLine($"- Case File {cf.Id}: {cf.CrimeType} at {cf.IncidentLocation}");
                }

                sb.AppendLine("\nDOCKETS SUMMARY:");
                foreach (var docket in dockets)
                {
                    sb.AppendLine($"- Docket {docket.Id}: {docket.DeceasedSurname}, {docket.DeceasedChristianNames}");
                }

                DownloadFile(sb.ToString(), "Complete_Report.txt");
            }
        }

        private void DownloadFile(string content, string filename)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + filename);
            Response.Charset = "";
            Response.ContentType = "application/text";
            Response.Output.Write(content);
            Response.Flush();
            Response.End();
        }

        // Add this debug method to help troubleshoot
        private void LogDebug(string message)
        {
            try
            {
                string debugFile = HttpContext.Current.Server.MapPath("~/debug_captain.txt");
                File.AppendAllText(debugFile, $"{DateTime.Now}: {message}\n");
            }
            catch
            {
                // Ignore debug errors
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

                // Redirect to login page
                Response.Redirect("~/Login.aspx", false);
                Context.ApplicationInstance.CompleteRequest();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Captain logout error: {ex.Message}");
                Response.Redirect("~/Login.aspx");
            }
        }
    }
}