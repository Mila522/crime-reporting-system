using System;
using System.Linq;
using ComplaintSystem.Models;

namespace ComplaintSystem
{
    public partial class ViewComplaint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int complaintId;
                if (int.TryParse(Request.QueryString["ComplaintId"], out complaintId))
                {
                    LoadComplaint(complaintId);
                }
                else
                {
                    lblMessage.Text = "Invalid complaint ID.";
                }
            }
        }

        private void LoadComplaint(int id)
        {
            using (var context = new AppDbContext())
            {
                var complaint = context.Complaints
                    .Where(c => c.Id == id)
                    .Select(c => new
                    {
                        c.Id,
                        c.Complainant.FirstNames,
                        c.Complainant.Surname,
                        c.CrimeType,
                        c.Description,
                        c.Status,
                        c.CreatedDate,
                        EvidenceRecords = context.EvidenceRecords
                            .Where(er => er.ComplaintId == c.Id)
                            .Select(er => er.FilePath).ToList()
                    })
                    .FirstOrDefault();

                if (complaint != null)
                {
                    pnlComplaint.Visible = true;

                    lblId.Text = complaint.Id.ToString();
                    lblComplainant.Text = $"{complaint.FirstNames} {complaint.Surname}";
                    lblCrimeType.Text = complaint.CrimeType;
                    lblDescription.Text = complaint.Description;
                    lblStatus.Text = complaint.Status.ToString();
                    lblCreatedAt.Text = complaint.CreatedDate.ToString();

                    rptEvidence.DataSource = complaint.EvidenceRecords;
                    rptEvidence.DataBind();
                }
                else
                {
                    lblMessage.Text = "Complaint not found.";
                }
            }
        }
    }
}

