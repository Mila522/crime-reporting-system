<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewDockets.aspx.cs" Inherits="ComplaintSystem.ViewDockets" %>
<!DOCTYPE html>
<html>
<head>
    <title>View Dockets</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="Content/Styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="container py-5">
            <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false"></asp:Label>
            <div class="card mb-4">
                <div class="card-header">
                    <h2 class="mb-0">View Created Dockets</h2>
                </div>
                <div class="card-body">
                    <asp:Panel ID="pnlEmptyDockets" runat="server" Visible="false" CssClass="empty-section">
                        No dockets found.
                    </asp:Panel>
                    <asp:Repeater ID="rptDockets" runat="server">
                        <HeaderTemplate>
                            <div class="list-group">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div class="list-group-item">
                                <p class="mb-1">
                                    <strong>Docket ID:</strong> <%# Eval("Id") %> | 
                                    <strong>Case File ID:</strong> <%# Eval("CaseFileId") %> | 
                                    <strong>Complaint ID:</strong> <%# Eval("ComplaintId") %>
                                </p>
                                <p class="mb-1"><strong>Details:</strong> <%# string.IsNullOrEmpty(Eval("Details") as string) ? "Not Specified" : Eval("Details") %></p>
                                <p class="mb-1"><strong>Deceased:</strong> <%# string.IsNullOrEmpty(Eval("DeceasedSurname") as string) ? "Not Specified" : Eval("DeceasedSurname") + ", " + Eval("DeceasedChristianNames") %></p>
                                <p class="mb-1"><strong>Identity Number:</strong> <%# string.IsNullOrEmpty(Eval("DeceasedIdentityNumber") as string) ? "Not Specified" : Eval("DeceasedIdentityNumber") %></p>
                                <p class="mb-1"><strong>Date of Death:</strong> <%# Eval("DeceasedDateOfDeath") == null ? "Not Set" : ((DateTime)Eval("DeceasedDateOfDeath")).ToString("yyyy-MM-dd") %></p>
                                <p class="mb-1"><strong>Exhibit References:</strong> SAPS13: <%# string.IsNullOrEmpty(Eval("SAPS13RefNo") as string) ? "Not Specified" : Eval("SAPS13RefNo") %>, SAPS43: <%# string.IsNullOrEmpty(Eval("SAPS43RefNo") as string) ? "Not Specified" : Eval("SAPS43RefNo") %></p>
                                <p class="mb-1"><strong>Investigation Entries:</strong> <%# Eval("InvestigationEntryCount") %></p>
                                <p class="mb-1"><strong>Accused Persons:</strong> <%# Eval("AccusedCount") %></p>
                                <p class="timestamp mb-2"><strong>Created:</strong> <%# Eval("CreatedDate") == null ? "Not Set" : ((DateTime)Eval("CreatedDate")).ToString("yyyy-MM-dd HH:mm:ss") %></p>
                            </div>
                        </ItemTemplate>
                        <FooterTemplate>
                            </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </div>
            <asp:Button ID="btnBack" runat="server" Text="Back to Dashboard" OnClick="btnBack_Click" CssClass="btn btn-secondary" />
        </div>
    </form>
</body>
</html>