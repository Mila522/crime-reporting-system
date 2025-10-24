<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewComplaint.aspx.cs" Inherits="ComplaintSystem.ViewComplaint" %>
<%@ Import Namespace="ComplaintSystem.Models" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>View Complaint</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .complaint-details {
            border: 1px solid #ddd;
            border-radius: 5px;
            padding: 20px;
            background-color: #f8f9fa;
            margin-top: 30px;
        }
        .complaint-details h3 {
            color: #007bff;
        }
        .evidence-list a {
            display: block;
            margin-bottom: 5px;
        }
        .text-danger {
            color: red;
            font-weight: bold;
        }
    </style>
</head>
<body class="bg-light">
    <form id="form1" runat="server" class="container mt-5">

        <h2 class="text-primary">Complaint Details</h2>
        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>

        <asp:Panel ID="pnlComplaint" runat="server" Visible="false" CssClass="complaint-details">

            <p><strong>Complaint ID:</strong> <asp:Label ID="lblId" runat="server" /></p>
            <p><strong>Complainant:</strong> <asp:Label ID="lblComplainant" runat="server" /></p>
            <p><strong>Crime Type:</strong> <asp:Label ID="lblCrimeType" runat="server" /></p>
            <p><strong>Description:</strong> <asp:Label ID="lblDescription" runat="server" /></p>
            <p><strong>Status:</strong> <asp:Label ID="lblStatus" runat="server" /></p>
            <p><strong>Created At:</strong> <asp:Label ID="lblCreatedAt" runat="server" /></p>

            <h5>Evidence:</h5>
            <div class="evidence-list">
                <asp:Repeater ID="rptEvidence" runat="server">
                    <ItemTemplate>
                        <a href='<%# Container.DataItem %>' target="_blank" class="btn btn-link">View Evidence</a>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </asp:Panel>

    </form>
</body>
</html>
