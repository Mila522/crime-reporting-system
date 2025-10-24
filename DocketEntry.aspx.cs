using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using ComplaintSystem.Models;
using System.Data.Entity;
using System.Data.Entity.Validation;
using ComplaintSystem.Services;
using System.Data.Entity.Infrastructure;

namespace ComplaintSystem
{
    public partial class DocketEntry : System.Web.UI.Page
    {
        private int ComplaintId
        {
            get
            {
                int.TryParse(Request.QueryString["complaintId"], out int id);
                return id;
            }
        }

        private int CaseFileId
        {
            get
            {
                int.TryParse(Request.QueryString["caseFileId"], out int id);
                return id;
            }
        }

        // Use simple DTOs instead of EF models for session storage
        private List<InvestigationEntryDTO> InvestigationEntries
        {
            get
            {
                var entries = Session[$"InvestigationEntries_{CaseFileId}"] as List<InvestigationEntryDTO>;
                return entries ?? new List<InvestigationEntryDTO>();
            }
            set { Session[$"InvestigationEntries_{CaseFileId}"] = value; }
        }

        private List<AccusedDTO> AccusedPersons
        {
            get
            {
                var accused = Session[$"AccusedPersons_{CaseFileId}"] as List<AccusedDTO>;
                return accused ?? new List<AccusedDTO>();
            }
            set { Session[$"AccusedPersons_{CaseFileId}"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (ComplaintId <= 0 || CaseFileId <= 0)
                {
                    ShowError("Invalid or missing query parameters.");
                    Response.Redirect("~/ChargeOfficerDashboard.aspx", false);
                    return;
                }

                try
                {
                    LoadCaseInformation();
                    InitializeInvestigationGrid();
                    InitializeAccusedRepeater();
                }
                catch (Exception ex)
                {
                    ShowError($"Error loading case information: {ex.Message}");
                }
            }
        }

        private void LoadCaseInformation()
        {
            using (var context = new AppDbContext())
            {
                var caseFile = context.CaseFiles
                    .Include(cf => cf.Complaint)
                    .Include(cf => cf.ChargeOfficer)
                    .FirstOrDefault(cf => cf.Id == CaseFileId && cf.ComplaintId == ComplaintId);

                if (caseFile != null)
                {
                    lblComplaintId.Text = caseFile.ComplaintId.ToString();
                    lblCaseFileId.Text = caseFile.Id.ToString();
                    lblChargeOfficer.Text = $"{caseFile.ChargeOfficer?.FirstNames} {caseFile.ChargeOfficer?.Surname}".Trim();
                    lblCaseInfo.Text = $"Complaint: {caseFile.ComplaintId} - Case File: {caseFile.Id}";
                }
                else
                {
                    ShowError("Case file not found.");
                    Response.Redirect("~/ChargeOfficerDashboard.aspx", false);
                }
            }
        }

        private void InitializeInvestigationGrid()
        {
            if (InvestigationEntries.Count == 0)
            {
                // Add two empty entries by default
                InvestigationEntries.Add(new InvestigationEntryDTO { EntryDateTime = DateTime.Now });
                InvestigationEntries.Add(new InvestigationEntryDTO { EntryDateTime = DateTime.Now });
            }
            BindInvestigationGrid();
        }

        private void BindInvestigationGrid()
        {
            gvInvestigation.DataSource = InvestigationEntries;
            gvInvestigation.DataBind();
        }

        private void InitializeAccusedRepeater()
        {
            if (AccusedPersons.Count == 0)
            {
                AccusedPersons.Add(new AccusedDTO { AccusedNumber = 1 });
            }
            BindAccusedRepeater();
        }

        private void BindAccusedRepeater()
        {
            rptAccused.DataSource = AccusedPersons;
            rptAccused.DataBind();
        }

        protected void btnAddAccused_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAccusedFromRepeater();
                int nextNumber = AccusedPersons.Count + 1;
                AccusedPersons.Add(new AccusedDTO { AccusedNumber = nextNumber });
                BindAccusedRepeater();
            }
            catch (Exception ex)
            {
                ShowError($"Error adding accused: {ex.Message}");
            }
        }

        protected void btnAddInvestigation_Click(object sender, EventArgs e)
        {
            try
            {
                SaveInvestigationFromGrid();
                InvestigationEntries.Add(new InvestigationEntryDTO { EntryDateTime = DateTime.Now });
                BindInvestigationGrid();
            }
            catch (Exception ex)
            {
                ShowError($"Error adding investigation entry: {ex.Message}");
            }
        }

        private void SaveInvestigationFromGrid()
        {
            var updatedEntries = new List<InvestigationEntryDTO>();

            for (int i = 0; i < gvInvestigation.Rows.Count; i++)
            {
                var row = gvInvestigation.Rows[i];
                var txtDateTime = (TextBox)row.FindControl("txtInvDateTime");
                var txtDetails = (TextBox)row.FindControl("txtInvDetails");
                var txtPlotNo = (TextBox)row.FindControl("txtPlotIncidentNo");

                var entry = new InvestigationEntryDTO();

                if (!string.IsNullOrEmpty(txtDateTime?.Text))
                {
                    entry.EntryDateTime = DateTime.Parse(txtDateTime.Text);
                }
                else
                {
                    entry.EntryDateTime = DateTime.Now;
                }

                entry.InvestigationDetails = txtDetails?.Text ?? "";
                entry.PlotIncidentNo = txtPlotNo?.Text ?? "";

                updatedEntries.Add(entry);
            }

            InvestigationEntries = updatedEntries;
        }

        private void SaveAccusedFromRepeater()
        {
            var updatedAccused = new List<AccusedDTO>();

            for (int i = 0; i < rptAccused.Items.Count; i++)
            {
                var item = rptAccused.Items[i];
                var txtSurname = (TextBox)item.FindControl("txtAccusedSurname");
                var txtNames = (TextBox)item.FindControl("txtAccusedNames");
                var txtID = (TextBox)item.FindControl("txtAccusedID");
                var txtDOB = (TextBox)item.FindControl("txtAccusedDOB");
                var txtAge = (TextBox)item.FindControl("txtAccusedAge");
                var ddlRace = (DropDownList)item.FindControl("ddlAccusedRace");
                var ddlGender = (DropDownList)item.FindControl("ddlAccusedGender");

                var accused = new AccusedDTO();

                accused.Surname = txtSurname?.Text ?? "";
                accused.ChristianNames = txtNames?.Text ?? "";
                accused.IdentityNumber = txtID?.Text ?? "";
                accused.DateOfBirth = TryParseDate(txtDOB?.Text);
                accused.Age = txtAge?.Text ?? "";
                accused.Race = ddlRace?.SelectedValue ?? "";
                accused.Gender = ddlGender?.SelectedValue ?? "";
                accused.AccusedNumber = i + 1;

                // Initialize the new fields with empty values
                accused.MaritalStatus = "";
                accused.Occupation = "";
                accused.Education = "";
                accused.PlaceOfBirth = "";
                accused.Nationality = "";
                accused.ResidentialAddress = "";
                accused.Language = "";
                accused.BusinessAddress = "";
                accused.BusinessTelNo = 0;
                accused.ArrestDateTime = 0.0;
                accused.StreetNo = "";
                accused.StreetName = "";
                accused.Suburb = "";
                accused.Town = "";
                accused.PostalCode = "";

                updatedAccused.Add(accused);
            }

            AccusedPersons = updatedAccused;
        }

        protected void rptAccused_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var accused = (AccusedDTO)e.Item.DataItem;
                var ddlRace = (DropDownList)e.Item.FindControl("ddlAccusedRace");
                var ddlGender = (DropDownList)e.Item.FindControl("ddlAccusedGender");

                if (ddlRace != null && !string.IsNullOrEmpty(accused.Race))
                    ddlRace.SelectedValue = accused.Race;

                if (ddlGender != null && !string.IsNullOrEmpty(accused.Gender))
                    ddlGender.SelectedValue = accused.Gender;
            }
        }

        protected void btnSaveDocket_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                ShowError("Please fill in all required fields.");
                return;
            }

            // Validate required fields
            var requiredFields = new List<(string Name, string Value)>
            {
                ("Docket Details", txtDocketDetails?.Text),
                ("Deceased Surname", txtDeceasedSurname?.Text),
                ("Deceased Christian Names", txtDeceasedChristianNames?.Text),
                ("Deceased Title", txtDeceasedTitle?.Text),
                ("Deceased Identity Number", txtDeceasedIdentityNumber?.Text),
                ("Deceased Race", ddlDeceasedRace?.SelectedValue),
                ("Deceased Gender", ddlDeceasedGender?.SelectedValue)
            };

            var missingFields = requiredFields
                .Where(f => string.IsNullOrWhiteSpace(f.Value))
                .Select(f => f.Name)
                .ToList();

            if (missingFields.Any())
            {
                string message = "Please fill in all required fields: " + string.Join(", ", missingFields);
                ShowError(message);
                return;
            }

            try
            {
                SaveInvestigationFromGrid();
                SaveAccusedFromRepeater();

                using (var context = new AppDbContext())
                {
                    // Check if docket already exists
                    if (context.Dockets.Any(d => d.CaseFileId == CaseFileId))
                    {
                        ShowError("A docket already exists for this case file.");
                        return;
                    }

                    var docket = new Docket
                    {
                        CaseFileId = CaseFileId,
                        ComplaintId = ComplaintId,
                        Details = txtDocketDetails.Text.Trim(),
                        CreatedDate = DateTime.Now,

                        // Deceased information
                        DeceasedSurname = txtDeceasedSurname?.Text?.Trim() ?? "",
                        DeceasedChristianNames = txtDeceasedChristianNames?.Text?.Trim() ?? "",
                        DeceasedTitle = txtDeceasedTitle?.Text?.Trim() ?? "",
                        DeceasedIdentityNumber = txtDeceasedIdentityNumber?.Text?.Trim() ?? "",
                        DeceasedDateOfBirth = TryParseDate(txtDeceasedDOB?.Text),
                        DeceasedAge = txtDeceasedAge?.Text?.Trim() ?? "",
                        DeceasedRace = ddlDeceasedRace.SelectedValue,
                        DeceasedGender = ddlDeceasedGender.SelectedValue,
                        DeceasedBuilding = txtDeceasedBuilding?.Text?.Trim() ?? "",
                        DeceasedStreetName = txtDeceasedStreetName?.Text?.Trim() ?? "",
                        DeceasedStreetNo = txtDeceasedStreetNo?.Text?.Trim() ?? "",
                        DeceasedSuburb = txtDeceasedSuburb?.Text?.Trim() ?? "",
                        DeceasedTown = txtDeceasedTown?.Text?.Trim() ?? "",
                        DeceasedDateOfDeath = TryParseDate(txtDeceasedDateOfDeath?.Text),
                        DeceasedPlaceOfDeath = txtDeceasedPlaceOfDeath?.Text?.Trim() ?? "",
                        DeceasedMortuary = txtDeceasedMortuary?.Text?.Trim() ?? "",
                        DeceasedMortuaryRefNo = txtDeceasedMortuaryRefNo?.Text?.Trim() ?? "",

                        // Exhibits
                        SAPS13RefNo = txtSAPS13RefNo?.Text?.Trim() ?? "",
                        SAPS43RefNo = txtSAPS43RefNo?.Text?.Trim() ?? "",
                        ExhibitDateToCourt = TryParseDate(txtExhibitDateToCourt?.Text),
                        ExhibitRecord = txtExhibitRecord?.Text?.Trim() ?? ""
                    };

                    context.Dockets.Add(docket);
                    context.SaveChanges(); // Save to get the Docket ID
                   

                    // Convert DTOs to Entity Framework models and save investigation entries
                    foreach (var invDto in InvestigationEntries.Where(inv => !string.IsNullOrWhiteSpace(inv.InvestigationDetails)))
                    {
                        var investigationEntry = new InvestigationEntry
                        {
                            DocketId = docket.Id,
                            EntryDateTime = invDto.EntryDateTime,
                            InvestigationDetails = invDto.InvestigationDetails,
                            PlotIncidentNo = invDto.PlotIncidentNo
                        };
                        context.InvestigationEntries.Add(investigationEntry);
                    }

                    // Convert DTOs to Entity Framework models and save accused persons
                    foreach (var accDto in AccusedPersons.Where(acc => !string.IsNullOrWhiteSpace(acc.ChristianNames) || !string.IsNullOrWhiteSpace(acc.Surname)))
                    {
                        var accused = new Accused
                        {
                            DocketId = docket.Id,
                            AccusedNumber = accDto.AccusedNumber,
                            Surname = accDto.Surname ?? "",
                            ChristianNames = accDto.ChristianNames ?? "",
                            IdentityNumber = accDto.IdentityNumber ?? "",
                            DateOfBirth = accDto.DateOfBirth,
                            Age = accDto.Age ?? "",
                            Race = accDto.Race ?? "",
                            Gender = accDto.Gender ?? "",

                            // Add all the new required fields - FIXED PROPERTY NAMES
                            MaritalStatus = accDto.MaritalStatus ?? "",
                            Occupation = accDto.Occupation ?? "",
                            Education = accDto.Education ?? "",
                            PlaceOfBirth = accDto.PlaceOfBirth ?? "",
                            Nationality = accDto.Nationality ?? "",
                            ResidentialAddress = accDto.ResidentialAddress ?? "", // Fixed - removed incorrect comment
                            Language = accDto.Language ?? "",
                            BusinessAddress = accDto.BusinessAddress ?? "",
                            BusinessTelNo = accDto.BusinessTelNo,
                            ArrestDateTime = accDto.ArrestDateTime,
                            StreetNo = accDto.StreetNo ?? "",
                            StreetName = accDto.StreetName ?? "",
                            Suburb = accDto.Suburb ?? "",
                            Town = accDto.Town ?? "",
                            PostalCode = accDto.PostalCode ?? ""
                        };
                        context.Accused.Add(accused);
                    }


                    context.SaveChanges();

                    // Clear session data
                    Session.Remove($"InvestigationEntries_{CaseFileId}");
                    Session.Remove($"AccusedPersons_{CaseFileId}");

                    // Send email notification
                    SendDocketCreatedEmail(ComplaintId);

                    ShowSuccess($"Docket saved successfully! Docket ID: {docket.Id}");

                    // Use ClientScript to redirect after alert
                    ScriptManager.RegisterStartupScript(this, GetType(), "redirect",
                        "setTimeout(function() { window.location.href = 'ChargeOfficerDashboard.aspx'; }, 1000);", true);
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var errorMessages = dbEx.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                ShowError($"Database validation error: {fullErrorMessage}");
                System.Diagnostics.Trace.WriteLine($"Entity Validation Error: {fullErrorMessage}");
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException dbUpdateEx)
            {
                HandleDbUpdateException(dbUpdateEx);
                System.Diagnostics.Trace.WriteLine($"DbUpdate Error: {dbUpdateEx}");
            }
            catch (Exception ex)
            {
                ShowError($"Error saving docket: {ex.Message}");
                // Log the full exception details for debugging
                System.Diagnostics.Trace.WriteLine($"Error saving docket: {ex}");
            }
        }
        private void HandleDbUpdateException(DbUpdateException dbUpdateEx)
        {
            var innerException = dbUpdateEx.InnerException;
            var fullErrorMessage = "";

            while (innerException != null)
            {
                fullErrorMessage += $" -> {innerException.Message}";
                System.Diagnostics.Trace.WriteLine($"INNER EXCEPTION: {innerException.Message}");

                if (innerException.Message.Contains("column") ||
                    innerException.Message.Contains("invalid column") ||
                    innerException.Message.Contains("ResidentialAddress"))
                {
                    // This will tell us exactly which column is causing the issue
                    ShowError($"Database column error detected: {innerException.Message}");
                    return;
                }

                innerException = innerException.InnerException;
            }

            ShowError($"Database update error: {fullErrorMessage}");
        }

        private DateTime? TryParseDate(string dateString)
        {
            if (string.IsNullOrEmpty(dateString))
                return null;
            return DateTime.TryParse(dateString, out DateTime date) ? date : (DateTime?)null;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Clear session data when canceling
            Session.Remove($"InvestigationEntries_{CaseFileId}");
            Session.Remove($"AccusedPersons_{CaseFileId}");
            Response.Redirect("~/ChargeOfficerDashboard.aspx", false);
        }

        private void SendDocketCreatedEmail(int complaintId)
        {
            try
            {
                using (var context = new AppDbContext())
                {
                    var complaint = context.Complaints
                        .Include("Complainant")
                        .FirstOrDefault(c => c.Id == complaintId);

                    if (complaint?.Complainant != null && !string.IsNullOrEmpty(complaint.Complainant.Email))
                    {
                        var emailService = new EmailService();
                        var subject = "Case Under Investigation";
                        var body = $@"Dear {complaint.Complainant.FirstNames},

We want to inform you that your case is now under investigation.

Complaint ID: {complaint.Id}

Our team is working on your case and we will keep you updated on the progress.

Thank you for your patience.

Sincerely,
Complaint System Team";

                        emailService.SendEmailAsync(complaint.Complainant.Email, subject, body);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log email failure but don't prevent docket save
                System.Diagnostics.Trace.WriteLine($"Failed to send docket email: {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "error",
                $"alert('{message.Replace("'", "\\'")}');", true);
        }

        private void ShowSuccess(string message)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "success",
                $"alert('{message.Replace("'", "\\'")}');", true);
        }
    }

    // Data Transfer Objects (DTOs) that are serializable
    [Serializable]
    public class InvestigationEntryDTO
    {
        public DateTime EntryDateTime { get; set; }
        public string InvestigationDetails { get; set; }
        public string PlotIncidentNo { get; set; }
    }

    [Serializable]
    public class AccusedDTO
    {
        public int AccusedNumber { get; set; }
        public string Surname { get; set; }
        public string ChristianNames { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Age { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }

        // Add these new properties with default values
        public string MaritalStatus { get; set; } = "";
        public string Occupation { get; set; } = "";
        public string Education { get; set; } = "";
        public string PlaceOfBirth { get; set; } = "";
        public string Nationality { get; set; } = "";
        public string ResidentialAddress { get; set; } = ""; // Note: Keep the typo to match your model
        public string Language { get; set; } = "";
        public string BusinessAddress { get; set; } = "";
        public int BusinessTelNo { get; set; } = 0;
        public double ArrestDateTime { get; set; } = 0.0;
        public string StreetNo { get; set; } = "";
        public string StreetName { get; set; } = "";
        public string Suburb { get; set; } = "";
        public string Town { get; set; } = "";
        public string PostalCode { get; set; } = "";
    }


}