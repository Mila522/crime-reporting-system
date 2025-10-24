<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChargeOfficerDashboard.aspx.cs" Inherits="ComplaintSystem.ChargeOfficerDashboard" %>
<!DOCTYPE html>
<html>
<head>
    <title>Charge Officer Dashboard</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Styles.css" rel="stylesheet" type="text/css" />
    <style>
        .card {
            margin-bottom: 20px;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        }
        .section-header {
            background-color: #f8f9fa;
            border-bottom: 1px solid #dee2e6;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container-fluid py-4">
            <!-- Logout Button Row -->
            <div class="row mb-4">
                <div class="col">
                    <h2></h2>
                </div>
                <div class="col-auto">
                    <asp:Button ID="btnLogout" runat="server" Text="Logout" 
                        OnClick="btnLogout_Click" CssClass="btn btn-outline-danger" />
                </div>
            </div>

            <asp:Label ID="Label1" runat="server" CssClass="alert alert-info w-100" Visible="false"></asp:Label>
            <div class="row mb-4">
                <div class="col">
                    <h2>Charge Officer Dashboard</h2>
                    <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-info w-100" Visible="false"></asp:Label>
                </div>
            </div>

            <!-- Crime Type Filter -->
            <div class="row mb-4">
                <div class="col-md-4">
                    <label class="form-label">Filter by Crime Type:</label>
                    <asp:DropDownList ID="ddlCrimeTypeFilter" runat="server" AutoPostBack="true" 
                        OnSelectedIndexChanged="ddlCrimeTypeFilter_SelectedIndexChanged" CssClass="form-select">
                        <asp:ListItem Text="All Crime Types" Value=""></asp:ListItem>
                        <asp:ListItem Text="Murder" Value="Murder"></asp:ListItem>
                        <asp:ListItem Text="Rape" Value="Rape"></asp:ListItem>
                        <asp:ListItem Text="Assault" Value="Assault"></asp:ListItem>
                        <asp:ListItem Text="Burglary" Value="Burglary"></asp:ListItem>
                        <asp:ListItem Text="Arson" Value="Arson"></asp:ListItem>
                        <asp:ListItem Text="Theft" Value="Theft"></asp:ListItem>
                        <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-md-8 d-flex align-items-end">
                    <asp:Button ID="btnViewDockets" runat="server" Text="View All Dockets" 
                        OnClick="btnViewDockets_Click" CssClass="btn btn-primary" />
                </div>
            </div>

            <!-- Main Content Row: Complaints and Case Files Side by Side, No Gap -->
            <div class="row g-0">
                <!-- Complaints Section -->
                <div class="col-md-6">
                    <div class="card h-100 rounded-0 rounded-start">
                        <div class="card-header section-header">
                            <h4 class="mb-0">Complaints</h4>
                        </div>
                        <div class="card-body">
                            <asp:Repeater ID="rptComplaints" runat="server" OnItemCommand="rptComplaints_ItemCommand">
                                <HeaderTemplate>
                                    <div class="list-group">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="list-group-item">
                                        <h5>Complaint ID: <%# Eval("Id") %></h5>
                                        <p><strong>Crime Type:</strong> <%# Eval("CrimeType") %></p>
                                        <p><strong>Description:</strong> <%# Eval("Description") %></p>
                                        <p><strong>Accused Number:</strong> <%# Eval("AccusedNumber") %></p>
                                        <p><strong>Status:</strong> <%# Eval("Status") %></p>
                                        <p><strong>Complainant:</strong> <%# Eval("ComplainantName") %></p>
                                        <p><strong>Created:</strong> <%# Eval("CreatedDate", "{0:yyyy-MM-dd HH:mm}") %></p>
                                        <asp:Button ID="btnCreateCaseFile" runat="server" Text="Create Case File" 
                                            CommandName="CreateCaseFile" CommandArgument='<%# Eval("Id") %>'
                                            CssClass="btn btn-sm btn-success" />
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:Panel ID="pnlEmptyComplaints" runat="server" Visible="false" CssClass="alert alert-warning">
                                No complaints found.
                            </asp:Panel>
                        </div>
                    </div>
                </div>
                <!-- Case Files Section -->
                <div class="col-md-6">
                    <div class="card h-100 rounded-0 rounded-end">
                        <div class="card-header section-header">
                            <h4 class="mb-0">My Case Files</h4>
                        </div>
                        <div class="card-body">
                            <asp:Repeater ID="rptCaseFiles" runat="server" OnItemCommand="rptCaseFiles_ItemCommand">
                                <HeaderTemplate>
                                    <div class="list-group">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="list-group-item">
                                        <h5>Case File ID: <%# Eval("Id") %></h5>
                                        <p><strong>Complaint ID:</strong> <%# Eval("ComplaintId") %></p>
                                        <p><strong>Crime Type:</strong> <%# Eval("CrimeType") %></p>
                                        <p><strong>Details:</strong> <%# Eval("Details") %></p>
                                        <p><strong>Status:</strong> <%# Eval("ComplaintStatus") %></p>
                                        <p><strong>Witnesses:</strong> <%# Eval("WitnessCount") %></p>
                                        <p><strong>Created:</strong> <%# Eval("CreatedDate", "{0:yyyy-MM-dd HH:mm}") %></p>
                                        <asp:Button ID="btnCreateDocket" runat="server" Text="Create Docket" 
                                            CommandName="CreateDocket" CommandArgument='<%# Eval("Id") %>'
                                            CssClass="btn btn-sm btn-primary" />
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:Panel ID="pnlEmptyCaseFiles" runat="server" Visible="false" CssClass="alert alert-warning">
                                No case files found.
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Captain Access Section Centered Below -->
            <div class="row">
                <div class="col-md-6 offset-md-3">
                    <div class="card mb-4 mt-4">
                        <div class="card-header section-header">
                            <h4 class="mb-0">Captain Access</h4>
                        </div>
                        <div class="card-body text-center">
                            <p>Need to access Captain features?</p>
                            <asp:Button ID="btnCaptainLogin" runat="server" Text="Captain Login" 
                                OnClick="btnCaptainLogin_Click" CssClass="btn btn-warning btn-lg" />
                        </div>
                    </div>
                </div>
            </div>

            <!-- Dockets Section (Full Width) -->
            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-header section-header">
                            <h4 class="mb-0">My Dockets</h4>
                        </div>
                        <div class="card-body">
                            <asp:Repeater ID="rptDockets" runat="server">
                                <HeaderTemplate>
                                    <div class="list-group">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div class="list-group-item">
                                        <h5>Docket ID: <%# Eval("Id") %></h5>
                                        <p><strong>Case File ID:</strong> <%# Eval("CaseFileId") %></p>
                                        <p><strong>Complaint ID:</strong> <%# Eval("ComplaintId") %></p>
                                        <p><strong>Details:</strong> <%# Eval("Details") %></p>
                                        <p><strong>Deceased:</strong> <%# Eval("DeceasedSurname") %>, <%# Eval("DeceasedChristianNames") %></p>
                                        <p><strong>Investigations:</strong> <%# Eval("InvestigationEntryCount") %></p>
                                        <p><strong>Accused:</strong> <%# Eval("AccusedCount") %></p>
                                        <p><strong>Evidence:</strong> <%# Eval("EvidenceRecordCount") %></p>
                                        <p><strong>Created:</strong> <%# Eval("CreatedDate", "{0:yyyy-MM-dd HH:mm}") %></p>
                                    </div>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </div>
                                </FooterTemplate>
                            </asp:Repeater>
                            <asp:Panel ID="pnlEmptyDockets" runat="server" Visible="false" CssClass="alert alert-warning">
                                No dockets found.
                            </asp:Panel>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>