<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CaptainLogin.aspx.cs" Inherits="ComplaintSystem.CaptainLogin" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Captain Login</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .login-container {
            max-width: 400px;
            margin: 100px auto;
            padding: 30px;
            border: 1px solid #ddd;
            border-radius: 10px;
            background-color: #f8f9fa;
            box-shadow: 0 4px 6px rgba(0,0,0,0.1);
        }
        .captain-header {
            background-color: #ffc107;
            color: #856404;
            padding: 15px;
            border-radius: 5px;
            margin-bottom: 20px;
            text-align: center;
        }
        .form-group {
            margin-bottom: 20px;
        }
        .btn-captain {
            background-color: #ffc107;
            border-color: #ffc107;
            color: #856404;
            font-weight: bold;
        }
        .btn-captain:hover {
            background-color: #e0a800;
            border-color: #e0a800;
        }
        .back-link {
            text-align: center;
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <div class="captain-header">
                <h3></h3>
                <p class="mb-0">Restricted Access - Authorized Personnel Only</p>
            </div>
            
            <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-danger w-100" Visible="false"></asp:Label>
            
            <div class="form-group">
                <label for="txtEmail">Email:</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" 
                    ErrorMessage="Email is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            
            <div class="form-group">
                <label for="txtPassword">Password:</label>
                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" 
                    ErrorMessage="Password is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            
            <div class="form-group">
                <label for="txtOfficerID">Officer ID:</label>
                <asp:TextBox ID="txtOfficerID" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvOfficerID" runat="server" ControlToValidate="txtOfficerID" 
                    ErrorMessage="Officer ID is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>
            
            <asp:Button ID="btnLogin" runat="server" Text="Login as Captain" 
                OnClick="btnLogin_Click" CssClass="btn btn-captain w-100" />
            
            <div class="back-link">
                <asp:HyperLink ID="hlBackToAdmin" runat="server" 
                    NavigateUrl="~/ChargeOfficerDashboard.aspx" Text="← Back to Admin Dashboard"></asp:HyperLink>
            </div>
        </div>
    </form>
</body>
</html>