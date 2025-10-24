<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VerifyEmail.aspx.cs" Inherits="ComplaintSystem.VerifyEmail" %>

<!DOCTYPE html>
<html>
<head>
    <title>Verify Email</title>
    <style>
        .container { max-width: 400px; margin: 50px auto; padding: 20px; }
        .form-group { margin-bottom: 15px; }
        label { display: block; margin-bottom: 5px; }
        input[type="text"] { width: 100%; padding: 8px; }
        .btn { padding: 10px 20px; background: #007bff; color: white; border: none; cursor: pointer; }
        .alert { padding: 10px; margin-bottom: 15px; border-radius: 4px; }
        .alert-success { background: #d4edda; color: #155724; }
        .alert-danger { background: #f8d7da; color: #721c24; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Verify Your Email</h2>
            <asp:Label ID="lblMessage" runat="server" CssClass="alert" Visible="false" />
            
            <div class="form-group">
                <label>Verification Code:</label>
                <asp:TextBox ID="txtVerificationCode" runat="server" MaxLength="6" />
            </div>
            
            <asp:Button ID="btnVerify" runat="server" Text="Verify" CssClass="btn" OnClick="btnVerify_Click" />
            <asp:Button ID="btnResend" runat="server" Text="Resend Code" CssClass="btn" OnClick="btnResend_Click" 
                CausesValidation="false" />
        </div>
    </form>
</body>
</html>
