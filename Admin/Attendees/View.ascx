<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Ragnarok.Modules.RagnarokRegistration.Admin.Attendees.View" %>

<div class="status">
    <asp:Label ID="lblStatus" runat="server" CssClass="error" Visible="false" />
</div>

<asp:Panel ID="pGrid" runat="server">
    <div class="row">
        <div class="left">
            <asp:DropDownList ID="ddlViewYear" runat="server" OnSelectedIndexChanged="ddlViewYear_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="" Text="All Years" />
                <asp:ListItem Value="2017" Text="2017" />
                <asp:ListItem Value="2018" Text="2018" Selected="True" />
            </asp:DropDownList>
        </div>
        <div class="right">
            <asp:Button ID="btnCampMasterUpdate" runat="server" OnClick="btnCampMasterUpdate_Click" Text="Send Camp Master Lists" />
        </div>
    </div>
   <div class="row pad-b-10 pad-t-10">
        <asp:GridView ID="gvAttendees" runat="server" AutoGenerateColumns="false" AllowSorting="true" OnSorting="gvAttendees_Sorting" AllowPaging="true" PageSize="50" 
            PagerSettings-Mode="NumericFirstLast" PagerSettings-PageButtonCount="10" OnPageIndexChanging="gvAttendees_PageIndexChanging" PagerSettings-Visible="true" PagerSettings-Position="Bottom"
            OnRowCommand="gvAttendees_RowCommand" DataKeyNames="Id">
            <Columns>
                <asp:BoundField DataField="Id" Visible="false" />
                <asp:BoundField DataField="TagNumber" HeaderText="Tag #" ReadOnly="true" SortExpression="TagNumber" />
                <asp:BoundField DataField="LastName" HeaderText="Last Name" ReadOnly="true" SortExpression="LastName" />
                <asp:BoundField DataField="FirstName" HeaderText="First Name" ReadOnly="true" SortExpression="FirstName" />
                <asp:BoundField DataField="CharacterName" HeaderText="Character" ReadOnly="true" SortExpression="CharacterName" />
                <asp:BoundField DataField="CampName" HeaderText="Camp" ReadOnly="true" SortExpression="CampName" />
                <asp:BoundField DataField="RegistrationDate" HeaderText="Arrival Date" DataFormatString = "{0:MM/dd/yyyy}" ReadOnly="true" SortExpression="RegistrationDate" />
                <asp:CheckBoxField DataField="IsMinor" HeaderText="Minor" ReadOnly="true" SortExpression="IsMinor" ItemStyle-HorizontalAlign="Center" />
                <asp:CheckBoxField DataField="IsMerchant" HeaderText="Merchant" ReadOnly="true" SortExpression="IsMerchant" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="Status" HeaderText="Status" ReadOnly="true" SortExpression="Status" />
                <asp:ButtonField Text="Details" ButtonType="Link" CommandName="ViewDetails" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Panel>
<asp:Panel ID="pDetails" runat="server" Visible="false">
    <asp:Label ID="lblParticipantId" runat="server" Visible="false" />
    <div class="row pad-b-10">
        <asp:Button ID="btnResendConfirmation" runat="server" OnClick="btnResendConfirmation_Click" Text="Resend Confirmation Email" />
    </div>
    <asp:Panel ID="pIsMinor" runat="server" Visible="false">
        <div class="minorBlock">
            <p>This participant is a minor.</p>
        </div>
    </asp:Panel>
    <asp:Panel ID="pIsMerchant" runat="server" Visible="false">
        <div class="merchantBlock">
            <p>This participant is a merchant.</p>
        </div>
    </asp:Panel>
    <span class="required">* indicates required fields</span><br />
    <div class="row">
        <div class="col1">
            <h2>Participant Information</h2>
            <div class="item">
                <label for="ragTag">Rag Tag #: <span class="required">*</span></label>
                <asp:TextBox ID="txtRagTag" runat="server" />
            </div>
            <div class="item">
                <label for="firstName">First Name: <span class="required">*</span></label>
                <asp:TextBox ID="txtFirstName" runat="server" />
            </div>
            <div class="item">
                <label for="lastName">Last Name: <span class="required">*</span></label>
                <asp:TextBox ID="txtLastName" runat="server" />
            </div>
            <div class="item">
                <label for="dateOfBirth">Date of Birth: <span class="required">*</span></label>
                <asp:TextBox ID="txtDateOfBirth" runat="server" />
            </div>
            <div class="item">
                <label for="isMinor">Is Minor: </label>
                <asp:CheckBox ID="cbxIsMinor" runat="server" />
            </div>
            <div class="item">
                <label for="address1">Address 1: <span class="required">*</span></label>
                <asp:TextBox ID="txtAddress1" runat="server" />
            </div>
            <div class="item">
                <label for="address2">Address 2: </label>
                <asp:TextBox ID="txtAddress2" runat="server" />
            </div>
            <div class="item">
                <label for="city">City: <span class="required">*</span></label>
                <asp:TextBox ID="txtCity" runat="server" />
            </div>
            <div class="item">
                <label for="state">State/Province: <span class="required">*</span></label>
                <asp:DropDownList runat="server" ID="ddlState">
                    <asp:ListItem Value="">-- States --</asp:ListItem>
                    <asp:ListItem Value="AL">Alabama</asp:ListItem>
	                <asp:ListItem Value="AK">Alaska</asp:ListItem>
	                <asp:ListItem Value="AZ">Arizona</asp:ListItem>
	                <asp:ListItem Value="AR">Arkansas</asp:ListItem>
	                <asp:ListItem Value="CA">California</asp:ListItem>
	                <asp:ListItem Value="CO">Colorado</asp:ListItem>
	                <asp:ListItem Value="CT">Connecticut</asp:ListItem>
	                <asp:ListItem Value="DE">Delaware</asp:ListItem>
	                <asp:ListItem Value="FL">Florida</asp:ListItem>
	                <asp:ListItem Value="GA">Georgia</asp:ListItem>
	                <asp:ListItem Value="HI">Hawaii</asp:ListItem>
	                <asp:ListItem Value="ID">Idaho</asp:ListItem>
	                <asp:ListItem Value="IL">Illinois</asp:ListItem>
	                <asp:ListItem Value="IN">Indiana</asp:ListItem>
	                <asp:ListItem Value="IA">Iowa</asp:ListItem>
	                <asp:ListItem Value="KS">Kansas</asp:ListItem>
	                <asp:ListItem Value="KY">Kentucky</asp:ListItem>
	                <asp:ListItem Value="LA">Louisiana</asp:ListItem>
	                <asp:ListItem Value="ME">Maine</asp:ListItem>
	                <asp:ListItem Value="MD">Maryland</asp:ListItem>
	                <asp:ListItem Value="MA">Massachusetts</asp:ListItem>
	                <asp:ListItem Value="MI">Michigan</asp:ListItem>
	                <asp:ListItem Value="MN">Minnesota</asp:ListItem>
	                <asp:ListItem Value="MS">Mississippi</asp:ListItem>
	                <asp:ListItem Value="MO">Missouri</asp:ListItem>
	                <asp:ListItem Value="MT">Montana</asp:ListItem>
	                <asp:ListItem Value="NE">Nebraska</asp:ListItem>
	                <asp:ListItem Value="NV">Nevada</asp:ListItem>
	                <asp:ListItem Value="NH">New Hampshire</asp:ListItem>
	                <asp:ListItem Value="NJ">New Jersey</asp:ListItem>
	                <asp:ListItem Value="NM">New Mexico</asp:ListItem>
	                <asp:ListItem Value="NY">New York</asp:ListItem>
	                <asp:ListItem Value="NC">North Carolina</asp:ListItem>
	                <asp:ListItem Value="ND">North Dakota</asp:ListItem>
	                <asp:ListItem Value="OH">Ohio</asp:ListItem>
	                <asp:ListItem Value="OK">Oklahoma</asp:ListItem>
	                <asp:ListItem Value="OR">Oregon</asp:ListItem>
	                <asp:ListItem Value="PA">Pennsylvania</asp:ListItem>
	                <asp:ListItem Value="RI">Rhode Island</asp:ListItem>
	                <asp:ListItem Value="SC">South Carolina</asp:ListItem>
	                <asp:ListItem Value="SD">South Dakota</asp:ListItem>
	                <asp:ListItem Value="TN">Tennessee</asp:ListItem>
	                <asp:ListItem Value="TX">Texas</asp:ListItem>
	                <asp:ListItem Value="UT">Utah</asp:ListItem>
	                <asp:ListItem Value="VT">Vermont</asp:ListItem>
	                <asp:ListItem Value="VA">Virginia</asp:ListItem>
	                <asp:ListItem Value="WA">Washington</asp:ListItem>
	                <asp:ListItem Value="WV">West Virginia</asp:ListItem>
	                <asp:ListItem Value="WI">Wisconsin</asp:ListItem>
	                <asp:ListItem Value="WY">Wyoming</asp:ListItem>
                    <asp:ListItem Value=""></asp:ListItem>
                    <asp:ListItem Value="">-- Provinces --</asp:ListItem>
                    <asp:ListItem Value="AB">Alberta</asp:ListItem>
                    <asp:ListItem Value="BC">British Columbia</asp:ListItem>
                    <asp:ListItem Value="LB">Labrador</asp:ListItem>
                    <asp:ListItem Value="MB">Manitoba</asp:ListItem>
                    <asp:ListItem Value="NB">New Brunswick</asp:ListItem>
                    <asp:ListItem Value="NF">Newfoundland</asp:ListItem>
                    <asp:ListItem Value="NS">Nova Scotia</asp:ListItem>
                    <asp:ListItem Value="NU">Nunavut</asp:ListItem>
                    <asp:ListItem Value="NW">Northwest Territories</asp:ListItem>
                    <asp:ListItem Value="ON">Ontario</asp:ListItem>
                    <asp:ListItem Value="PE">Prince Edward Island</asp:ListItem>
                    <asp:ListItem Value="QC">Quebec</asp:ListItem>
                    <asp:ListItem Value="SK">Saskatchewan</asp:ListItem>
                    <asp:ListItem Value="YU">Yukon</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="item">
                <label for="zip">Zip Code: <span class="required">*</span></label>
                <asp:TextBox ID="txtZip" runat="server" />
            </div>
            <div class="item">
                <label for="homePhone">Home Phone: </label>
                <asp:TextBox ID="txtHomePhone" runat="server" />
            </div>
            <div class="item">
                <label for="cellPhone">Cell Phone: </label>
                <asp:TextBox ID="txtCellPhone" runat="server" />
            </div>
            <div class="item">
                <label for="email">Email: <span class="required">*</span></label>
                <asp:TextBox ID="txtEmail" runat="server" />
            </div>
        </div>
        <div class="col2">
            <h2>Character Information</h2>
            <div class="item">
                <label for="characterName">Character Name: <span class="required">*</span></label>
                <asp:TextBox ID="txtCharacterName" runat="server" />
            </div>
            <div class="item">
                <label for="chapterName">Chapter Name:</label>
                <asp:TextBox ID="txtChapterName" runat="server" />
            </div>
            <div class="item">
                <label for="unitName" id="lblUnitName">Unit Name:</label>
                <asp:TextBox ID="txtUnitName" runat="server" />
            </div>
            <h2>Camp Information</h2>
            <div class="item">
                <label for="camp">Camp: <span class="required">*</span></label>
                <asp:DropDownList runat="server" ID="ddlCamp"></asp:DropDownList>
            </div>
            <div class="item">
                <label for="camp">Is Merchant: <span class="required">*</span></label>
                <asp:CheckBox ID="cbxIsMerchant" runat="server" />
            </div>
            <h2>Payment Information</h2>
            <div class="item">
                <label for="authCode">Auth Code: </label>
                <asp:TextBox ID="txtAuthCode" runat="server" Enabled="false" />
            </div>
            <div class="item">
                <label for="transId">Transaction Id: </label>
                <asp:TextBox ID="txtTransactionId" runat="server" Enabled="false" />
            </div>
            <div class="item">
                <label for="amount">Amount: </label>
                <asp:TextBox ID="txtAmount" runat="server" Enabled="false" />
            </div>
            <div class="item">
                <label for="status">Status: </label>
                <asp:DropDownList ID="ddlStatus" runat="server">
                    <asp:ListItem Value="Not Paid" Text="Not Paid" />
                    <asp:ListItem Value="Paid" Text="Paid" />
                    <asp:ListItem Value="Declined" Text="Declined" />
                    <asp:ListItem Value="Checked In" Text="Checked In" />
                    <asp:ListItem Value="Duplicate" Text="Duplicate" />
                    <asp:ListItem Value="Test" Text="Test" />
                </asp:DropDownList>
            </div>
        </div>
    </div>
    <div class="row">
        <h2>Emergency Contact Information</h2>
        <div class="item">
            <label for="emergencyContactName">Emergency Contact Name: <span class="required">*</span></label>
            <asp:TextBox ID="txtEmergencyContactName" runat="server" />
        </div>
        <div class="item">
            <label for="emergencyContactPhone">Emergency Contact Phone: <span class="required">*</span></label>
            <asp:TextBox ID="txtEmergencyContactPhone" runat="server" />
        </div>
        <h2>Health Issues</h2>
        <asp:Repeater ID="repHealthIssues" runat="server">
            <ItemTemplate>
                <asp:TextBox ID="txtHealthIssue" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Issue") %>' CssClass="fullWidth" />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div class="row">
        <h2>Signature</h2>
        <asp:Image ID="imgSignature" runat="server" AlternateText="Signature" />
    </div>
    <div class="row pad-t-10">
        <div class="left">
            <asp:Button ID="btnCancel" runat="server" Text="Cancel Changes" OnClick="btnCancel_Click" />
        </div>
        <div class="right">
            <asp:Button ID="btnSave" runat="server" Text="Save Changes" OnClick="btnSave_Click" />
        </div>
    </div>
    <div class="row pad-t-10"></div>
</asp:Panel>