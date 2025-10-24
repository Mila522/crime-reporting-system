using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using ComplaintSystem.Models;
using System.Data.Entity; // For Include
using System.Collections.Generic; // For List<string>

namespace ComplaintSystem
{
    public partial class ComplainantDashboard : Page
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
                if (userData.Length != 2 || userData[1] != "Complainant")
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                BindComplaints();
            }
        }

        protected void btnSubmitComplaint_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedCrimeType = ddlCrimeType.SelectedValue;

                if (string.IsNullOrEmpty(selectedCrimeType))
                {
                    selectedCrimeType = "Other";
                }

                if (selectedCrimeType == "Other" && string.IsNullOrWhiteSpace(txtDescription.Text))
                {
                    lblComplaintMessage.Text = "Please provide a description for 'Other'.";
                    return;
                }

                using (var context = new AppDbContext())
                {
                    string identifier = User.Identity.Name.Split('|')[0];
                    var user = context.Users.FirstOrDefault(u =>
                        (u.Email == identifier || u.PhoneNumber == identifier) && u.Role == "Complainant");

                    if (user == null)
                    {
                        lblComplaintMessage.Text = "User not found.";
                        return;
                    }

                    var complaint = new Complaint
                    {
                        ComplainantId = user.Id,
                        CrimeType = selectedCrimeType,
                        Description = txtDescription.Text,
                        AccusedNumber = string.IsNullOrWhiteSpace(txtAccusedNumber.Text) ? null : txtAccusedNumber.Text,
                        Status = ComplaintStatus.Submitted,
                        CreatedDate = DateTime.Now
                    };

                    context.Complaints.Add(complaint);
                    context.SaveChanges();

                    if (fuEvidence.HasFiles)
                    {
                        // Ensure the Evidence folder exists
                        string evidenceFolder = Server.MapPath("~/Evidence/");
                        if (!Directory.Exists(evidenceFolder))
                        {
                            Directory.CreateDirectory(evidenceFolder);
                        }

                        foreach (HttpPostedFile uploadedFile in fuEvidence.PostedFiles)
                        {
                            string extension = Path.GetExtension(uploadedFile.FileName).ToLower();

                            // Allow only certain file types
                            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".mp4" };
                            if (!allowedExtensions.Contains(extension))
                            {
                                lblComplaintMessage.Visible = true;
                                lblComplaintMessage.Text = "Unsupported file format. Only .jpg, .jpeg, .png, .mp4 allowed.";
                                continue;
                            }

                            // Generate a unique file name to avoid overwriting
                            string uniqueFileName = Guid.NewGuid().ToString() + extension;
                            string savePath = Path.Combine(evidenceFolder, uniqueFileName);

                            try
                            {
                                uploadedFile.SaveAs(savePath);

                                // Save record to database
                                context.EvidenceRecords.Add(new EvidenceRecord
                                {
                                    ComplaintId = complaint.Id,
                                    FilePath = "~/Evidence/" + uniqueFileName,
                                    FileType = extension.Substring(1),
                                    UploadDate = DateTime.Now
                                });
                            }
                            catch (Exception uploadEx)
                            {
                                lblComplaintMessage.Visible = true;
                                lblComplaintMessage.Text = $"Error uploading file: {uploadEx.Message}";
                            }
                        }

                        context.SaveChanges();
                    }


                    lblComplaintMessage.Text = "Complaint submitted successfully.";
                    txtDescription.Text = "";
                    txtAccusedNumber.Text = "";
                    ddlCrimeType.SelectedIndex = 0;

                    BindComplaints();
                }
            }
            catch (Exception ex)
            {
                lblComplaintMessage.Text = string.Format("Error submitting complaint: {0}", ex.Message);
            }
        }

        private void BindComplaints()
        {
            using (var context = new AppDbContext())
            {
                string identifier = User.Identity.Name.Split('|')[0];
                var user = context.Users.FirstOrDefault(u =>
                    (u.Email == identifier || u.PhoneNumber == identifier) && u.Role == "Complainant");

                if (user != null)
                {
                    var complaints = context.Complaints
                        .Include(c => c.EvidenceRecords) // Eagerly load EvidenceRecords
                        .Where(c => c.ComplainantId == user.Id)
                        .ToList() // Materialize query first
                        .Select(c => new
                        {
                            c.Id,
                            CrimeType = c.CrimeType != null ? c.CrimeType : "Other",
                            c.Description,
                            AccusedNumber = c.AccusedNumber != null ? c.AccusedNumber : "Not Specified",
                            c.Status,
                            CreatedDate = c.CreatedDate,
                            EvidenceRecords = c.EvidenceRecords.Select(er => er.FilePath).ToList()
                        })
                        .OrderByDescending(c => c.CreatedDate != null ? c.CreatedDate : DateTime.MaxValue)
                        .ToList();

                    rptComplaints.DataSource = complaints;
                    rptComplaints.DataBind();
                }
                else
                {
                    lblComplaintMessage.Text = "User not found.";
                }
            }
        }

        protected void ddlCrimeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            divDescription.Visible = ddlCrimeType.SelectedValue == "Other";
        }
    }
}