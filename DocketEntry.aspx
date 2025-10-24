<%@ Page Title="Docket Entry" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocketEntry.aspx.cs" Inherits="ComplaintSystem.DocketEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Case Docket File</h2>
    <asp:Label ID="lblCaseInfo" runat="server" CssClass="case-info"></asp:Label>

    <div class="section">
        <h3>Case Information</h3>
        <p>
            Complaint ID: <asp:Label ID="lblComplaintId" runat="server"></asp:Label>
        </p>
        <p>
            Case File ID: <asp:Label ID="lblCaseFileId" runat="server"></asp:Label>
        </p>
        <p>
            Charge Officer: <asp:Label ID="lblChargeOfficer" runat="server"></asp:Label>
        </p>
        <p>
            Docket Details:
            <asp:TextBox ID="txtDocketDetails" runat="server" TextMode="MultiLine" Width="400px"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvDocketDetails" runat="server"
                ControlToValidate="txtDocketDetails"
                ErrorMessage="Docket Details is required."
                ForeColor="Red"
                Display="Dynamic" />
        </p>
    </div>

    <div class="section">
        <h3>Particulars of Deceased • Besonderhede van Oorledene</h3>
        <p>
            Surname • Van:
            <asp:TextBox ID="txtDeceasedSurname" runat="server"></asp:TextBox>
        </p>
        <p>
            Christian Names • Voorname:
            <asp:TextBox ID="txtDeceasedChristianNames" runat="server"></asp:TextBox>
        </p>
        <p>
            Title • Titel:
            <asp:TextBox ID="txtDeceasedTitle" runat="server"></asp:TextBox>
        </p>
        <p>
            Identity Number • Identiteitsnommer:
            <asp:TextBox ID="txtDeceasedIdentityNumber" runat="server"></asp:TextBox>
        </p>
        <p>
            Date of Birth • Datum van Geboorte:
            <asp:TextBox ID="txtDeceasedDOB" runat="server" TextMode="Date"></asp:TextBox>
        </p>
        <p>
            Age • Ouderdom:
            <asp:TextBox ID="txtDeceasedAge" runat="server"></asp:TextBox>
        </p>
        <p>
            Race • Ras:
            <asp:DropDownList ID="ddlDeceasedRace" runat="server">
                <asp:ListItem Text="Select" Value=""></asp:ListItem>
                <asp:ListItem Text="A" Value="A"></asp:ListItem>
                <asp:ListItem Text="BR" Value="BR"></asp:ListItem>
                <asp:ListItem Text="BL" Value="BL"></asp:ListItem>
                <asp:ListItem Text="W" Value="W"></asp:ListItem>
            </asp:DropDownList>
        </p>
        <p>
            Gender • Geslag:
            <asp:DropDownList ID="ddlDeceasedGender" runat="server">
                <asp:ListItem Text="Select" Value=""></asp:ListItem>
                <asp:ListItem Text="M" Value="M"></asp:ListItem>
                <asp:ListItem Text="F" Value="F"></asp:ListItem>
            </asp:DropDownList>
        </p>
        <h4>Address • Adres</h4>
        <p>
            Building/Farm/Place • Gebou/Plaas/Plek:
            <asp:TextBox ID="txtDeceasedBuilding" runat="server"></asp:TextBox>
        </p>
        <p>
            Street Number • Nommer:
            <asp:TextBox ID="txtDeceasedStreetNo" runat="server"></asp:TextBox>
        </p>
        <p>
            Street Name • Straatnaam:
            <asp:TextBox ID="txtDeceasedStreetName" runat="server"></asp:TextBox>
        </p>
        <p>
            Suburb/Area • Voorstad/Gebied:
            <asp:TextBox ID="txtDeceasedSuburb" runat="server"></asp:TextBox>
        </p>
        <p>
            Town/City • Dorp/Stad:
            <asp:TextBox ID="txtDeceasedTown" runat="server"></asp:TextBox>
        </p>
        <p>
            Date Deceased • Datum Oorlede:
            <asp:TextBox ID="txtDeceasedDateOfDeath" runat="server" TextMode="Date"></asp:TextBox>
        </p>
        <p>
            Place of Death • Plek Oorlede:
            <asp:TextBox ID="txtDeceasedPlaceOfDeath" runat="server"></asp:TextBox>
        </p>
        <p>
            Mortuary • Lykshuis:
            <asp:TextBox ID="txtDeceasedMortuary" runat="server"></asp:TextBox>
        </p>
        <p>
            Ref. No. • Verw. No.:
            <asp:TextBox ID="txtDeceasedMortuaryRefNo" runat="server"></asp:TextBox>
        </p>
    </div>

    <div class="section">
        <h3>Investigation Diary • Onderzoekdagboek</h3>
        <asp:GridView ID="gvInvestigation" runat="server" AutoGenerateColumns="False" CssClass="investigation-grid">
            <Columns>
                <asp:TemplateField HeaderText="Date & Time">
                    <ItemTemplate>
                        <asp:TextBox ID="txtInvDateTime" runat="server" TextMode="DateTimeLocal"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Investigation Details">
                    <ItemTemplate>
                        <asp:TextBox ID="txtInvDetails" runat="server" TextMode="MultiLine" Width="400px"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Plot/Incident No.">
                    <ItemTemplate>
                        <asp:TextBox ID="txtPlotIncidentNo" runat="server"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:Button ID="btnAddInvestigation" runat="server" Text="Add Investigation Entry" 
                    OnClick="btnAddInvestigation_Click" CssClass="aspNetButton" />
    </div>

    <div class="section">
        <h3>Particulars of Accused • Besonderhede van Beskuldigde</h3>
        
        <!-- Remove the Register directive and use inline controls instead -->
        <asp:Panel ID="pnlAccused" runat="server">
            <asp:Repeater ID="rptAccused" runat="server" OnItemDataBound="rptAccused_ItemDataBound">
                <ItemTemplate>
                    <div class="accused-form" style="border: 1px solid #999; padding: 10px; margin: 5px 0;">
                        <h4>Accused <%# Container.ItemIndex + 1 %></h4>
                        <table class="docket-table">
                            <tr>
                                <td class="label-cell">Surname • Van:</td>
                                <td><asp:TextBox ID="txtAccusedSurname" runat="server" CssClass="input-field" Text='<%# Eval("Surname") %>'></asp:TextBox></td>
                                <td class="label-cell">Christian names • Voorname:</td>
                                <td><asp:TextBox ID="txtAccusedNames" runat="server" CssClass="input-field" Text='<%# Eval("ChristianNames") %>'></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="label-cell">Identity number • Identiteitsnommer:</td>
                                <td><asp:TextBox ID="txtAccusedID" runat="server" CssClass="input-field" Text='<%# Eval("IdentityNumber") %>'></asp:TextBox></td>
                                <td class="label-cell">Date of birth • Geboortedatum:</td>
                                <td><asp:TextBox ID="txtAccusedDOB" runat="server" TextMode="Date" CssClass="input-field" Text='<%# Eval("DateOfBirth", "{0:yyyy-MM-dd}") %>'></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td class="label-cell">Age • Ouderdom:</td>
                                <td><asp:TextBox ID="txtAccusedAge" runat="server" CssClass="input-field" Text='<%# Eval("Age") %>'></asp:TextBox></td>
                                <td class="label-cell">Race • Ras:</td>
                                <td>
                                    <asp:DropDownList ID="ddlAccusedRace" runat="server" CssClass="input-field" SelectedValue='<%# Eval("Race") %>'>
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
                                    <asp:DropDownList ID="ddlAccusedGender" runat="server" CssClass="input-field" SelectedValue='<%# Eval("Gender") %>'>
                                        <asp:ListItem Text="Select" Value=""></asp:ListItem>
                                        <asp:ListItem Text="M" Value="M"></asp:ListItem>
                                        <asp:ListItem Text="F" Value="F"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td class="label-cell"></td>
                                <td></td>
                            </tr>
                        </table>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>
        
        <asp:Button ID="btnAddAccused" runat="server" Text="Add Accused" 
                    OnClick="btnAddAccused_Click" CssClass="aspNetButton" />
    </div>

    <div class="section">
        <h3>Exhibits • Bewysstukke</h3>
        <p>
            SAPS 13 Ref. No. • SAPD 13 Verw. No.:
            <asp:TextBox ID="txtSAPS13RefNo" runat="server"></asp:TextBox>
        </p>
        <p>
            SAPS 43 Ref. No. • SAPD 43 Verw. No.:
            <asp:TextBox ID="txtSAPS43RefNo" runat="server"></asp:TextBox>
        </p>
        <p>
            Date to Court • Datum na Hof:
            <asp:TextBox ID="txtExhibitDateToCourt" runat="server" TextMode="Date"></asp:TextBox>
        </p>
        <p>
            Record of Exhibits • Rekord van Bewysstukke:
            <asp:TextBox ID="txtExhibitRecord" runat="server" TextMode="MultiLine"></asp:TextBox>
        </p>
    </div>

    <div class="section">
        <asp:Button ID="btnSaveDocket" runat="server" Text="Save Docket" 
                    OnClick="btnSaveDocket_Click" CssClass="aspNetButton" />
        <asp:Button ID="btnCancel" runat="server" Text="Cancel" 
                    OnClick="btnCancel_Click" CssClass="aspNetButton" />
    </div>
</asp:Content>