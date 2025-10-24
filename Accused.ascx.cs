using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ComplaintSystem.UserControls
{
    public partial class AccusedFormControl : UserControl
    {
        public string AccusedNumber { get; set; }

        public string Surname
        {
            get { return txtAccusedSurname?.Text ?? string.Empty; }
            set { if (txtAccusedSurname != null) txtAccusedSurname.Text = value; }
        }

        public string Names
        {
            get { return txtAccusedNames?.Text ?? string.Empty; }
            set { if (txtAccusedNames != null) txtAccusedNames.Text = value; }
        }

        public string IdentityNumber
        {
            get { return txtAccusedID?.Text ?? string.Empty; }
            set { if (txtAccusedID != null) txtAccusedID.Text = value; }
        }

        public string DateOfBirth
        {
            get { return txtAccusedDOB?.Text ?? string.Empty; }
            set { if (txtAccusedDOB != null) txtAccusedDOB.Text = value; }
        }

        public string Age
        {
            get { return txtAccusedAge?.Text ?? string.Empty; }
            set { if (txtAccusedAge != null) txtAccusedAge.Text = value; }
        }

        public string Race
        {
            get { return ddlAccusedRace?.SelectedValue ?? string.Empty; }
            set
            {
                if (ddlAccusedRace != null && !string.IsNullOrEmpty(value))
                    ddlAccusedRace.SelectedValue = value;
            }
        }

        public string Gender
        {
            get { return ddlAccusedGender?.SelectedValue ?? string.Empty; }
            set
            {
                if (ddlAccusedGender != null && !string.IsNullOrEmpty(value))
                    ddlAccusedGender.SelectedValue = value;
            }
        }

        public string Building
        {
            get { return txtAccusedBuilding?.Text ?? string.Empty; }
            set { if (txtAccusedBuilding != null) txtAccusedBuilding.Text = value; }
        }

        public string StreetNo
        {
            get { return txtAccusedStreetNo?.Text ?? string.Empty; }
            set { if (txtAccusedStreetNo != null) txtAccusedStreetNo.Text = value; }
        }

        public string StreetName
        {
            get { return txtAccusedStreetName?.Text ?? string.Empty; }
            set { if (txtAccusedStreetName != null) txtAccusedStreetName.Text = value; }
        }

        public string Suburb
        {
            get { return txtAccusedSuburb?.Text ?? string.Empty; }
            set { if (txtAccusedSuburb != null) txtAccusedSuburb.Text = value; }
        }

        public string Town
        {
            get { return txtAccusedTown?.Text ?? string.Empty; }
            set { if (txtAccusedTown != null) txtAccusedTown.Text = value; }
        }

        public string PostalCode
        {
            get { return txtAccusedPostalCode?.Text ?? string.Empty; }
            set { if (txtAccusedPostalCode != null) txtAccusedPostalCode.Text = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataBind();
            }
        }
    }
}