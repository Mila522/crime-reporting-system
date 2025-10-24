<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ComplainantDashboard.aspx.cs" Inherits="ComplaintSystem.ComplainantDashboard" %>
<!DOCTYPE html>
<html>
<head>
    <title>Complainant Dashboard</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Styles.css" rel="stylesheet" type="text/css" />
    <style>
        body {
            background-color: #f4f6f9;
        }
        .card {
            border: none;
            border-radius: 10px;
            box-shadow: 0 4px 8px rgba(0,0,0,0.1);
        }
        .card-header {
            background-color: #007bff;
            color: white;
            border-radius: 10px 10px 0 0;
            padding: 15px;
        }
        .card-body {
            padding: 20px;
        }
        .form-label {
            font-weight: 600;
            margin-bottom: 8px;
        }
        .list-group-item {
            border: none;
            border-bottom: 1px solid #e9ecef;
            padding: 15px;
            background-color: #fff;
            border-radius: 5px;
            margin-bottom: 10px;
        }
        .evidence-link {
            color: #007bff;
            text-decoration: none;
            font-size: 0.9rem;
        }
        .evidence-link:hover {
            text-decoration: underline;
        }
        .timestamp {
            font-size: 0.9rem;
            color: #6c757d;
            font-style: italic;
        }
        .btn-primary {
            background-color: #007bff;
            border-color: #007bff;
            border-radius: 5px;
            transition: background-color 0.3s;
        }
        .btn-primary:hover {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <!-- ✨ enctype added here so file uploads actually work -->
    <form id="form1" runat="server" enctype="multipart/form-data">
        <div class="container py-5">
            <div class="card mb-4">
                <div class="card-header">
                    <h2 class="mb-0">Submit a Complaint</h2>
                </div>
                <div class="card-body">
                    <asp:Label ID="lblComplaintMessage" runat="server" CssClass="alert alert-danger w-100 mb-3" Visible="false"></asp:Label>
                    <div class="mb-3">
                        <label for="<%= ddlCrimeType.ClientID %>" class="form-label">Type of Crime</label>
                        <asp:DropDownList ID="ddlCrimeType" runat="server" AutoPostBack="true" 
                                          OnSelectedIndexChanged="ddlCrimeType_SelectedIndexChanged" 
                                          CssClass="form-select">
                            <asp:ListItem Text="Select Crime Type" Value=""></asp:ListItem>
                            <asp:ListItem Text="Murder" Value="Murder"></asp:ListItem>
                            <asp:ListItem Text="Rape" Value="Rape"></asp:ListItem>
                            <asp:ListItem Text="Assault" Value="Assault"></asp:ListItem>
                            <asp:ListItem Text="Burglary" Value="Burglary"></asp:ListItem>
                            <asp:ListItem Text="Arson" Value="Arson"></asp:ListItem>
                            <asp:ListItem Text="Theft" Value="Theft"></asp:ListItem>
                            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <asp:Panel ID="divDescription" runat="server" Visible="false" CssClass="mb-3">
                        <label for="<%= txtDescription.ClientID %>" class="form-label">Describe the Crime</label>
                        <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" 
                                     CssClass="form-control" Rows="5" placeholder="Enter details of the crime"></asp:TextBox>
                    </asp:Panel>
                    <div class="mb-3">
                        <label for="<%= txtAccusedNumber.ClientID %>" class="form-label">Accused Number (Optional)</label>
                        <asp:TextBox ID="txtAccusedNumber" runat="server" CssClass="form-control" 
                                     placeholder="Enter accused number"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="<%= fuEvidence.ClientID %>" class="form-label">Attach Evidence (Optional)</label>
                        <asp:FileUpload ID="fuEvidence" runat="server" AllowMultiple="true" CssClass="form-control" />
                    </div>
                    <asp:Button ID="btnSubmitComplaint" runat="server" Text="Submit Complaint" 
                                OnClick="btnSubmitComplaint_Click" CssClass="btn btn-primary" />
                </div>
            </div>

            <div class="card mb-4">
                <div class="card-header">
                    <h2 class="mb-0">My Complaints</h2>
                </div>
                <div class="card-body">
                    <asp:Repeater ID="rptComplaints" runat="server">
                        <HeaderTemplate>
                            <div class="list-group">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="list-group-item">
                                <h4 class="mb-2">Accused: <%# string.IsNullOrEmpty(Eval("AccusedNumber") as string) ? "Not Specified" : Eval("AccusedNumber") %></h4>
                                <p class="mb-1">
                                    <strong>Complaint ID:</strong> <%# Eval("Id") %> | 
                                    <strong>Crime Type:</strong> <%# Eval("CrimeType") %>
                                </p>
                                <p class="mb-1"><strong>Description:</strong> <%# Eval("Description") %></p>
                                <p class="mb-1"><strong>Status:</strong> <%# Eval("Status") %></p>
                                <p class="timestamp mb-2">Created: <%# Eval("CreatedDate") == null ? "Not Set" : ((DateTime)Eval("CreatedDate")).ToString("yyyy-MM-dd HH:mm:ss") %></p>
                                <asp:Repeater ID="rptEvidence" runat="server" DataSource='<%# Eval("EvidenceRecords") %>'>
                                    <ItemTemplate>
                                        <a href='<%# Container.DataItem %>' class="evidence-link" target="_blank">View Evidence</a><br />
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
