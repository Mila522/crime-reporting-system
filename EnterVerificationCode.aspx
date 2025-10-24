<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnterVerificationCode.aspx.cs" Inherits="ComplaintSystem.EnterVerificationCode" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Verify Email</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <style>
        .verification-container {
            max-width: 500px;
            margin: 50px auto;
            padding: 30px;
            border: 1px solid #ddd;
            border-radius: 10px;
            background-color: rgb(44, 38, 38);
        }
        body {
            background: url('BACKGROUND FOR CASEDOCKETSYSTEM/fdacf9e1-0b4b-4d11-abc5-304ba73533b8.jpg') no-repeat center center fixed;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="verification-container">
            <h2 class="text-center mb-4">Verify Your Email</h2>
            
            <div class="mb-3">
                <label class="form-label">Enter Verification Code</label>
                <asp:TextBox ID="txtVerificationCode" runat="server" CssClass="form-control" 
                             placeholder="Enter 6-digit code"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvCode" runat="server" ControlToValidate="txtVerificationCode" 
                    ErrorMessage="Verification code is required" CssClass="text-danger" Display="Dynamic"></asp:RequiredFieldValidator>
            </div>

            <asp:Button ID="btnVerify" runat="server" Text="Verify Email" 
                        OnClick="btnVerify_Click" CssClass="btn btn-primary w-100 mb-3" />
            
            <asp:Button ID="btnResend" runat="server" Text="Resend Code" 
                        OnClick="btnResend_Click" CssClass="btn btn-outline-secondary w-100" />

            <asp:Label ID="lblMessage" runat="server" CssClass="alert alert-info w-100 mt-3" 
                      Visible="false"></asp:Label>

            <div class="text-center mt-3">
                <asp:HyperLink ID="hlBackToLogin" runat="server" NavigateUrl="~/Login.aspx" 
                              CssClass="text-decoration-none">Back to Login</asp:HyperLink>
            </div>
        </div>
    </form>
</body>
</html>