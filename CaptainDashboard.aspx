<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CaptainDashboard.aspx.cs" Inherits="ComplaintSystem.CaptainDashboard" %>
<!DOCTYPE html>
<html>
<head>
    <title>Captain Dashboard</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
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
            <!-- Add this row for logout button -->
    <div class="row mb-4">
        <div class="col">
            <h2></h2>
        </div>
        <div class="col-auto">
            <asp:Button ID="btnLogout" runat="server" Text="Logout" 
                        OnClick="btnLogout_Click" CssClass="btn btn-outline-danger" />
        </div>
    </div>
    
    <!-- Rest of your existing content -->
    <asp:Label ID="Label1" runat="server" CssClass="alert alert-info w-100" Visible="false"></asp:Label>
            <div class="row mb-4">
                <div class="col">
                    <h2>Captain Dashboard</h2>
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
                    <asp:Button ID="btnExportAll" runat="server" Text="Export All Reports" 
                        OnClick="btnExportAll_Click" CssClass="btn btn-success" />
                </div>
            </div>

            <!-- Case Files Section -->
            <div class="card">
                <div class="card-header section-header">
                    <h4 class="mb-0">Case Files</h4>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvCaseFiles" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-striped table-bordered" OnRowCommand="gvCaseFiles_RowCommand"
                        EmptyDataText="No case files found">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Case File ID" />
                            <asp:BoundField DataField="ComplaintId" HeaderText="Complaint ID" />
                            <asp:BoundField DataField="CrimeType" HeaderText="Crime Type" />
                            <asp:BoundField DataField="IncidentLocation" HeaderText="Incident Location" />
                            <asp:BoundField DataField="Station" HeaderText="Station" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" 
                                DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <asp:Button ID="btnExportCaseFile" runat="server" Text="Export" 
                                        CommandName="ExportCaseFile" CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn btn-sm btn-outline-primary" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <!-- Dockets Section -->
            <div class="card">
                <div class="card-header section-header">
                    <h4 class="mb-0">Dockets</h4>
                </div>
                <div class="card-body">
                    <asp:GridView ID="gvDockets" runat="server" AutoGenerateColumns="False" 
                        CssClass="table table-striped table-bordered" OnRowCommand="gvDockets_RowCommand"
                        EmptyDataText="No dockets found">
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="Docket ID" />
                            <asp:BoundField DataField="CaseFileId" HeaderText="Case File ID" />
                            <asp:BoundField DataField="ComplaintId" HeaderText="Complaint ID" />
                            <asp:BoundField DataField="DeceasedSurname" HeaderText="Deceased Surname" />
                            <asp:BoundField DataField="DeceasedChristianNames" HeaderText="Deceased Names" />
                            <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" 
                                DataFormatString="{0:yyyy-MM-dd HH:mm}" />
                            <asp:TemplateField HeaderText="Actions">
                                <ItemTemplate>
                                    <asp:Button ID="btnExportDocket" runat="server" Text="Export" 
                                        CommandName="ExportDocket" CommandArgument='<%# Eval("Id") %>'
                                        CssClass="btn btn-sm btn-outline-primary" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>
</body>
</html>