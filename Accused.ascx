<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Accused.ascx.cs" 
    Inherits="ComplaintSystem.UserControls.AccusedFormControl" %>

<div class="accused-form" style="border: 1px solid #999; padding: 10px; margin: 5px 0;">
    <h4>Accused <%# AccusedNumber %></h4>
    <table class="docket-table">
        <tr>
            <td class="label-cell">Surname • Van:</td>
            <td><asp:TextBox ID="txtAccusedSurname" runat="server" CssClass="input-field"></asp:TextBox></td>
            <td class="label-cell">Christian names • Voorname:</td>
            <td><asp:TextBox ID="txtAccusedNames" runat="server" CssClass="input-field"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="label-cell">Identity number • Identiteitsnommer:</td>
            <td><asp:TextBox ID="txtAccusedID" runat="server" CssClass="input-field"></asp:TextBox></td>
            <td class="label-cell">Date of birth • Geboortedatum:</td>
            <td><asp:TextBox ID="txtAccusedDOB" runat="server" TextMode="Date" CssClass="input-field"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="label-cell">Age • Ouderdom:</td>
            <td><asp:TextBox ID="txtAccusedAge" runat="server" CssClass="input-field"></asp:TextBox></td>
            <td class="label-cell">Race • Ras:</td>
            <td>
                <asp:DropDownList ID="ddlAccusedRace" runat="server" CssClass="input-field">
                    <asp:ListItem Text="Select" Value=""></asp:ListItem>
                    <asp:ListItem Text="A" Value="A"></asp:ListItem>
                    <asp:ListItem Text="BR" Value="BR"></asp:ListItem>
                    <asp:ListItem Text="BL" Value="BL"></asp:ListItem>
                    <asp:ListItem Text="W" Value="W"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td class="label-cell">Gender • Geslag:</td>
            <td>
                <asp:DropDownList ID="ddlAccusedGender" runat="server" CssClass="input-field">
                    <asp:ListItem Text="Select" Value=""></asp:ListItem>
                    <asp:ListItem Text="M" Value="M"></asp:ListItem>
                    <asp:ListItem Text="F" Value="F"></asp:ListItem>
                </asp:DropDownList>
            </td>
            <td class="label-cell"></td>
            <td></td>
        </tr>
        <tr>
            <td colspan="4"><h5>Address • Adres</h5></td>
        </tr>
        <tr>
            <td class="label-cell">Building/Farm/Place • Gebou/Plaas/Plek:</td>
            <td><asp:TextBox ID="txtAccusedBuilding" runat="server" CssClass="input-field"></asp:TextBox></td>
            <td class="label-cell">Street Number • Nommer:</td>
            <td><asp:TextBox ID="txtAccusedStreetNo" runat="server" CssClass="input-field"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="label-cell">Street Name • Straatnaam:</td>
            <td><asp:TextBox ID="txtAccusedStreetName" runat="server" CssClass="input-field"></asp:TextBox></td>
            <td class="label-cell">Suburb/Area • Voorstad/Gebied:</td>
            <td><asp:TextBox ID="txtAccusedSuburb" runat="server" CssClass="input-field"></asp:TextBox></td>
        </tr>
        <tr>
            <td class="label-cell">Town/City • Dorp/Stad:</td>
            <td><asp:TextBox ID="txtAccusedTown" runat="server" CssClass="input-field"></asp:TextBox></td>
            <td class="label-cell">Postal Code • Poskode:</td>
            <td><asp:TextBox ID="txtAccusedPostalCode" runat="server" CssClass="input-field"></asp:TextBox></td>
        </tr>
    </table>
</div>