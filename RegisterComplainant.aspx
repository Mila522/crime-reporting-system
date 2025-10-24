<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegisterComplainant.aspx.cs" Inherits="ComplaintSystem.RegisterComplainant" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register Complainant</title>
    <link href="Content/Styles.css" rel="stylesheet" type="text/css" />
    <style>
        .register-container { max-width: 400px; margin: 50px auto; padding: 20px; border: 1px solid #ccc; border-radius: 5px; background-color: rgb(45, 40, 40) }
        .form-group { margin-bottom: 15px; }
        .form-group label { display: inline-block; width: 120px; }
        .error { color: red; }
        .back-link { margin-top: 10px; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="register-container">
            <h2>Register as Complainant</h2>
            <div class="form-group">
                <label for="txtEmail">Email:</label>
                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" 
                    ErrorMessage="Email is required." CssClass="error" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtPhoneNumber">Phone Number:</label>
                <asp:TextBox ID="txtPhoneNumber" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber" 
                    ErrorMessage="Phone Number is required." CssClass="error" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtPassword">Password:</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" 
                    ErrorMessage="Password is required." CssClass="error" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtFirstNames">First Names:</label>
                <asp:TextBox ID="txtFirstNames" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFirstNames" runat="server" ControlToValidate="txtFirstNames" 
                    ErrorMessage="First Names are required." CssClass="error" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtSurname">Surname:</label>
                <asp:TextBox ID="txtSurname" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvSurname" runat="server" ControlToValidate="txtSurname" 
                    ErrorMessage="Surname is required." CssClass="error" Display="Dynamic" />
            </div>
            <div class="form-group">
                <label for="txtIDNumber">ID Number:</label>
                <asp:TextBox ID="txtIDNumber" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvIDNumber" runat="server" ControlToValidate="txtIDNumber" 
                    ErrorMessage="ID Number is required." CssClass="error" Display="Dynamic" />
            </div>
            <asp:Button ID="btnRegister" runat="server" Text="Register" OnClick="btnRegister_Click" />
            <div class="back-link">
                <asp:HyperLink ID="hlLogin" runat="server" NavigateUrl="~/Login.aspx">Back to Login</asp:HyperLink>
            </div>
            <asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label>
        </div>
    </form>
</body>
</html>