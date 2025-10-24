<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CreateCaseFile.aspx.cs" Inherits="ComplaintSystem.CreateCaseFile" %>
<!DOCTYPE html>
<html>
<head>
    <title>Create Case File</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container py-5">
            <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>
            <div class="card mb-4">
                <div class="card-header">
                    <h2 class="mb-0">Create Case File</h2>
                </div>
                <div class="card-body">
                    <h4>Case Information</h4>
                    <div class="mb-3">
                        <label class="form-label">Complaint ID</label>
                        <asp:TextBox ID="txtComplaintId" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Crime Type</label>
                        <asp:TextBox ID="txtCrimeType" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Incident Location</label>
                        <asp:TextBox ID="txtIncidentLocation" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Station</label>
                        <asp:TextBox ID="txtStation" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Details</label>
                        <asp:TextBox ID="txtDetails" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Agencies Involved</label>
                        <asp:TextBox ID="txtAgenciesInvolved" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Obstacles/Challenges</label>
                        <asp:TextBox ID="txtObstacles" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Pending Actions</label>
                        <asp:TextBox ID="txtPendingActions" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                    </div>

                    <h4>Witnesses</h4>
                    <div class="mb-3">
                        <label class="form-label">Witness Name</label>
                        <asp:TextBox ID="txtWitnessName" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Witness Statement</label>
                        <asp:TextBox ID="txtWitnessStatement" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </div>
                    <asp:Button ID="btnAddWitness" runat="server" Text="Add Witness" OnClick="btnAddWitness_Click" CssClass="btn btn-primary btn-sm mb-3" />
                    <asp:GridView ID="gvWitnesses" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered">
                        <Columns>
                            <asp:BoundField DataField="Name" HeaderText="Witness Name" />
                            <asp:BoundField DataField="Statement" HeaderText="Statement" />
                        </Columns>
                    </asp:GridView>

                    <asp:Button ID="btnSave" runat="server" Text="Save Case File" OnClick="btnSave_Click" CssClass="btn btn-primary mt-3" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="btn btn-secondary mt-3" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
