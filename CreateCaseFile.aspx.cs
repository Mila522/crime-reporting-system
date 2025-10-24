using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using ComplaintSystem.Models;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;
using ComplaintSystem.Services;

namespace ComplaintSystem
{
    public partial class CreateCaseFile : System.Web.UI.Page
    {
        private List<Witness> TempWitnesses
        {
            get
            {
                return Session["TempWitnesses"] as List<Witness> ?? new List<Witness>();
            }
            set
            {
                Session["TempWitnesses"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!User.Identity.IsAuthenticated || User.Identity.Name.Split('|')[1] != "ChargeOfficer")
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                try
                {
                    string complaintIdStr = Request.QueryString["complaintId"];
                    if (string.IsNullOrEmpty(complaintIdStr) || !int.TryParse(complaintIdStr, out int complaintId) || complaintId <= 0)
                    {
                        lblMessage.Text = $"Invalid or missing Complaint ID: '{complaintIdStr ?? "null"}'. Redirecting to dashboard in 3 seconds.";
                        lblMessage.CssClass = "alert alert-danger";
                        lblMessage.Visible = true;
                        Response.AddHeader("REFRESH", "3;URL=~/ChargeOfficerDashboard.aspx");
                        return;
                    }

                    using (var context = new AppDbContext())
                    {
                        var complaint = context.Complaints
                            .Include(c => c.Complainant)
                            .FirstOrDefault(c => c.Id == complaintId);
                        if (complaint == null)
                        {
                            lblMessage.Text = $"Complaint with ID {complaintId} not found in database. Redirecting to dashboard in 3 seconds.";
                            lblMessage.CssClass = "alert alert-danger";
                            lblMessage.Visible = true;
                            Response.AddHeader("REFRESH", "3;URL=~/ChargeOfficerDashboard.aspx");
                            return;
                        }

                        txtComplaintId.Text = complaint.Id.ToString();
                        txtCrimeType.Text = complaint.CrimeType ?? "Other";
                        txtDetails.Text = $"Case file for {complaint.CrimeType ?? "Other"} complaint {complaint.Id}";
                    }

                    TempWitnesses = new List<Witness>();
                    BindWitnesses();
                }
                catch (Exception ex)
                {
                    lblMessage.Text = $"Error loading complaint: {ex.Message}";
                    if (ex.InnerException != null)
                        lblMessage.Text += $" Inner Exception: {ex.InnerException.Message}";
                    lblMessage.CssClass = "alert alert-danger";
                    lblMessage.Visible = true;
                    Response.AddHeader("REFRESH", "3;URL=~/ChargeOfficerDashboard.aspx");
                }
            }
        }

        protected void btnAddWitness_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtWitnessName.Text) || !string.IsNullOrEmpty(txtWitnessStatement.Text))
            {
                var witnesses = TempWitnesses;
                witnesses.Add(new Witness
                {
                    Name = txtWitnessName.Text,
                    Statement = txtWitnessStatement.Text,
                    CreatedDate = DateTime.Now
                });
                TempWitnesses = witnesses;

                txtWitnessName.Text = "";
                txtWitnessStatement.Text = "";
                BindWitnesses();
            }
        }

        private void BindWitnesses()
        {
            gvWitnesses.DataSource = TempWitnesses;
            gvWitnesses.DataBind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int complaintId = int.Parse(txtComplaintId.Text);
                using (var context = new AppDbContext())
                {
                    string identifier = User.Identity.Name.Split('|')[0];
                    var user = context.Users.FirstOrDefault(u =>
                        (u.Email == identifier || u.PhoneNumber == identifier) && u.Role == "ChargeOfficer");

                    if (user == null)
                    {
                        lblMessage.Text = "Charge Officer not found.";
                        lblMessage.CssClass = "alert alert-danger";
                        lblMessage.Visible = true;
                        return;
                    }

                    var complaint = context.Complaints.Find(complaintId);
                    if (complaint == null)
                    {
                        lblMessage.Text = "Complaint not found.";
                        lblMessage.CssClass = "alert alert-danger";
                        lblMessage.Visible = true;
                        return;
                    }

                    var caseFile = new CaseFile
                    {
                        ComplaintId = complaintId,
                        ChargeOfficerId = user.Id,
                        Details = txtDetails.Text,
                        CrimeType = txtCrimeType.Text,
                        ComplaintStatus = ComplaintStatus.UnderInvestigation,
                        CreatedDate = DateTime.Now,
                        IncidentLocation = txtIncidentLocation.Text,
                        Station = txtStation.Text,
                        AgenciesInvolved = txtAgenciesInvolved.Text,
                        Obstacles = txtObstacles.Text,
                        PendingActions = txtPendingActions.Text
                    };

                    context.CaseFiles.Add(caseFile);
                    complaint.Status = ComplaintStatus.UnderInvestigation;
                    context.SaveChanges();

                    // Save witnesses
                    foreach (var witness in TempWitnesses)
                    {
                        witness.CaseFileId = caseFile.Id;
                        context.Witnesses.Add(witness);
                    }
                    context.SaveChanges();
                    SendCaseFileCreatedEmail(complaintId, caseFile.Id);


                    lblMessage.Text = "Case file created successfully.";
                    lblMessage.CssClass = "alert alert-success";
                    lblMessage.Visible = true;

                    TempWitnesses = new List<Witness>();
                    BindWitnesses();
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = $"Error saving case file: {ex.Message}";
                if (ex.InnerException != null)
                    lblMessage.Text += $" Inner Exception: {ex.InnerException.Message}";
                lblMessage.CssClass = "alert alert-danger";
                lblMessage.Visible = true;
            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ChargeOfficerDashboard.aspx");
        }
        private void SendCaseFileCreatedEmail(int complaintId, int caseFileId)
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
                        var subject = "Case File Created - Your Complaint";
                        var body = $@"Dear {complaint.Complainant.FirstNames},

Your case file has been created successfully.

Complaint ID: {complaint.Id}
Case File ID: {caseFileId}

You will be updated on further developments.

Thank you,
Complaint System Team";

                        emailService.SendEmailAsync(complaint.Complainant.Email, subject, body);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to send case file email: {ex.Message}");
            }
        }

    }
}
