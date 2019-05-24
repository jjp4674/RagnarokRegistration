<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Ragnarok.Modules.RagnarokRegistration.Admin.Camps.View" %>

<div class="status">
    <asp:Label ID="lblStatus" runat="server" CssClass="error" Visible="false" />
</div>

<asp:Panel ID="pGrid" runat="server">
    <div class="row">
        <div class="left">
            <asp:DropDownList ID="ddlViewYear" runat="server" OnSelectedIndexChanged="ddlViewYear_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="" Text="All Years" />
                <asp:ListItem Value="2017" Text="2017" />
                <asp:ListItem Value="2018" Text="2018" />
                <asp:ListItem Value="2019" Text="2019" Selected="True" />
            </asp:DropDownList>
        </div>
        <div class="right">
            <asp:Button ID="btnGeneratePrintOut" runat="server" OnClick="btnGeneratePrintOut_Click" Text="Generate Facebook List" />
        </div>
        <div class="right" style="margin-right: 10px;">
            <asp:Button ID="btnAddCamp" runat="server" OnClick="btnAddCamp_Click" Text="Add Camp" />
        </div>
    </div>
    <div class="row pad-b-10 pad-t-10">
        <asp:GridView ID="gvCamps" runat="server" AutoGenerateColumns="false" AllowSorting="true" OnSorting="gvCamps_Sorting" AllowPaging="true" PageSize="50"
            PagerSettings-Mode="NumericFirstLast" PagerSettings-PageButtonCount="5" OnPageIndexChanging="gvCamps_PageIndexChanging" PagerSettings-Visible="true" PagerSettings-Position="Bottom"
            OnRowCommand="gvCamps_RowCommand" DataKeyNames="Id" style="width: 100%;">
            <Columns>
                <asp:BoundField DataField="Id" Visible="false" />
                <asp:BoundField DataField="CampName" HeaderText="Camp Name" ReadOnly="true" SortExpression="CampName" />
                <asp:BoundField DataField="CampLocation" HeaderText="Camp Location" ReadOnly="true" SortExpression="CampLocation" />
                <asp:BoundField DataField="FirstName" HeaderText="First Name" ReadOnly="true" SortExpression="FirstName" />
                <asp:BoundField DataField="LastName" HeaderText="Last Name" ReadOnly="true" SortExpression="LastName" />
                <asp:HyperLinkField DataTextField="Email" DataNavigateUrlFields="Email" DataNavigateUrlFormatString="mailto:{0}" HeaderText="Email" SortExpression="Email" />
                <asp:BoundField DataField="CampCount" HeaderText="Registered" ReadOnly="true" SortExpression="CampCount" />
                <asp:ButtonField Text="Details" ButtonType="Link" CommandName="ViewDetails" />
            </Columns>
        </asp:GridView>
    </div>
</asp:Panel>

<asp:Panel ID="pDetails" runat="server" Visible="false">
    <asp:Label ID="lblCampId" runat="server" Visible="false" />
    <div class="row">
        <div class="col1">
            <h2>Camp Information</h2>
            <div class="item">
                <label for="campName">Camp Name: <span class="required">*</span></label>
                <asp:TextBox ID="txtCampName" runat="server" />
            </div>
            <div class="item">
                <label for="campLocation">Camp Location: </label>
                <asp:TextBox ID="txtCampLocation" runat="server" />
            </div>
            <div class="item">
                <label for="campCount">Event Year: </label>
                <asp:DropDownList ID="ddlYear" runat="server">
                    <asp:ListItem Value="2017" Text="2017" />
                    <asp:ListItem Value="2018" Text="2018" />
                    <asp:ListItem Value="2019" Text="2019" Selected="True" />
                </asp:DropDownList>
            </div>
        </div>
        <div class="col2">
            <h2>Camp Master Information</h2>
            <div class="item">
                <label for="firstName">First Name:</label>
                <asp:TextBox ID="txtFirstName" runat="server" />
            </div>
            <div class="item">
                <label for="lastName">Last Name:</label>
                <asp:TextBox ID="txtLastName" runat="server" />
            </div>
            <div class="item">
                <label for="characterName">Character Name:</label>
                <asp:TextBox ID="txtCharacterName" runat="server" />
            </div>
            <div class="item">
                <label for="email">Email:</label>
                <asp:TextBox ID="txtEmail" runat="server" />
            </div>
        </div>
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

<asp:Panel ID="pPrintOut" runat="server" Visible="false">
    <div class="pad-b-10">
        <asp:Literal ID="litPrintOut" runat="server" />
    </div>
    <div class="row pad-b-10">
        <asp:Button ID="btnPrintCancel" runat="server" Text="Go Back" OnClick="btnPrintCancel_Click" />
    </div>
</asp:Panel>