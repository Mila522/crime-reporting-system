<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ComplaintSystem.Login" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="Content/Site.css" rel="stylesheet" type="text/css" />
    <link href="Content/Styles.css" rel="stylesheet" type="text/css" />
    <style>
        .login-container {
            max-width: 600px;
            margin: 50px auto;
            padding: 20px;
            border: 1px solid #ddd;
            border-radius: 5px;
            background-color: rgb(48 43 43);
        }
        .role-buttons {
            margin: 15px 0;
            text-align: center;
        }
        .role-buttons .btn {
            margin: 0 10px;
            padding: 10px 20px;
        }
        .form-group {
            margin: 15px 0;
        }
        .field {
            margin: 10px 0;
        }
        .field label {
            display: block;
            font-weight: bold;
            margin-bottom: 5px;
        }
        .field input[type="text"],
        .field input[type="password"] {
            width: 100%;
            padding: 8px;
            border: 1px solid #ddd;
            border-radius: 4px;
        }
        .action-buttons {
            text-align: center;
            margin: 20px 0;
        }
        .btn-primary, .btn-secondary, .btn-info {
            padding: 10px 20px;
            margin: 0 5px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }
        .btn-primary { background-color: #007bff; color: white; }
        .btn-secondary { background-color: #6c757d; color: white; }
        .btn-info { background-color: #17a2b8; color: white; }
        .error { color: red; margin-top: 10px; display: block; }
        .radio-list { display: flex; gap: 15px; margin: 10px 0; }
        .radio-list label { font-weight: normal; }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="login-container">
            <h2 style="text-align: center;">Login</h2>

            <!-- Role Buttons -->
            <div class="role-buttons">
                <asp:Button ID="btnComplainant" runat="server" Text="Complainant"
                    CommandArgument="Complainant" CausesValidation="false"
                    CssClass="btn btn-info" OnClick="btnRole_Click" />

                <asp:Button ID="btnChargeOfficer" runat="server" Text="Admin"
                    CommandArgument="ChargeOfficer" CausesValidation="false"
                    CssClass="btn btn-primary" OnClick="btnRole_Click" />
            </div>

            <!-- Officer ID (only Charge Officer) -->
            <div class="form-group" id="divOfficerID" runat="server" visible="false">
                <label for="txtOfficerID">Officer ID:</label>
                <asp:TextBox ID="txtOfficerID" runat="server" CssClass="form-control"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvOfficerID" runat="server" ControlToValidate="txtOfficerID" 
                    ErrorMessage="Officer ID required." CssClass="error" Display="Dynamic" />
            </div>

            <!-- Login Method -->
            <div class="form-group">
                <label>Login with:</label>
                <div class="radio-list">
                    <asp:RadioButtonList ID="rblLoginMethod" runat="server" RepeatDirection="Horizontal"
                        AutoPostBack="true" OnSelectedIndexChanged="rblLoginMethod_SelectedIndexChanged">
                        <asp:ListItem Value="Email" Selected="True">Email</asp:ListItem>
                        <asp:ListItem Value="Phone">Phone Number</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>

            <!-- Login Inputs -->
            <div class="form-group">
                <div class="field">
                    <asp:Label ID="lblLoginIdentifier" runat="server" Text="Email:"></asp:Label>
                    <asp:TextBox ID="txtLoginIdentifier" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvLoginIdentifier" runat="server" ControlToValidate="txtLoginIdentifier" 
                        ErrorMessage="Required." CssClass="error" Display="Dynamic" />
                </div>
                
                <div class="field">
                    <label for="txtPassword">Password:</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" 
                        ErrorMessage="Required." CssClass="error" Display="Dynamic" />
                </div>
            </div>

            <!-- Login / Register Buttons -->
            <div class="action-buttons">
                <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn-primary" OnClick="btnLogin_Click" />
                <asp:Button ID="btnRegister" runat="server" Text="Register as Complainant" CssClass="btn-secondary" 
                    OnClick="btnRegister_Click" Visible="false" />
            </div>
            <asp:HyperLink ID="lnkForgotPassword" runat="server" 
    NavigateUrl="~/ForgotPassword.aspx" 
    Text="Forgot Password?" />

            <!-- Messages -->
            <asp:Label ID="lblMessage" runat="server" CssClass="error"></asp:Label>
        </div>
    </form>
</body>
</html>