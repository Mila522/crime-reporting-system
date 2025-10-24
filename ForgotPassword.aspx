<<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="ComplaintSystem.ForgotPassword" Async="true" %>


<!DOCTYPE html>
<html>
<head>
    <title>Forgot Password</title>
    <style>
        .container { max-width: 400px; margin: 50px auto; padding: 20px; }
        .form-group { margin-bottom: 15px; }
        label { display: block; margin-bottom: 5px; }
        input[type="email"] { width: 100%; padding: 8px; }
        .btn { padding: 10px 20px; background: #007bff; color: white; border: none; cursor: pointer; }
        .alert { padding: 10px; margin-bottom: 15px; border-radius: 4px; }
        .alert-success { background: #d4edda; color: #155724; }
        .alert-danger { background: #f8d7da; color: #721c24; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Forgot Password</h2>
            <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false" />
            
            <div class="form-group">
                <label>Email Address:</label>
                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" required="true" />
            </div>
            
            <asp:Button ID="btnSendPassword" runat="server" Text="Send My Password" CssClass="btn" 
                OnClick="btnSendPassword_Click" />
            <asp:HyperLink ID="lnkLogin" runat="server" NavigateUrl="~/Login.aspx" Text="Back to Login" />
        </div>
    </form>
</body>
</html>
