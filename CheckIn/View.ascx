<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Ragnarok.Modules.RagnarokRegistration.CheckIn.View" %>


<asp:UpdatePanel ID="upCheckIn" runat="server" UpdateMode="Always">
    <ContentTemplate>
        <div class="status">
            <asp:Label ID="lblStatus" runat="server" CssClass="error" Visible="false" />
        </div>
        <asp:Panel ID="pSearch" runat="server" Visible="false">
            <div class="row">
                <div class="col-xs-12">
                    <h1>Search for an Attendee or Add New Attendee</h1>
                </div>
            </div>
            <div class="row pad-t-10 pad-b-10">
                <div class="col-xs-6">
                    <div class="row">
                        <div class="col-xs-5">
                            <b>First Name:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtSearchFirstName" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Last Name:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtSearchLastName" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Rag Tag #:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtSearchRagTag" runat="server" /> 
                            <div>
                                <asp:CompareValidator ID="comRagTag" runat="server" ControlToValidate="txtSearchRagTag" Operator="DataTypeCheck" Type="Integer" Display="Dynamic" ErrorMessage="Rag Tag must be a number." CssClass="error" ValidationGroup="Search" />
                            </div>
                        </div>
                    </div>
                    <div class="row pad-t-10">
                        <div class="col-xs-12 text-center">
                            <asp:Button ID="btnSearch" runat="server" Text="Search For Attendee" CssClass="btn btn-lg btn-success" OnClick="btnSearch_Click" ValidationGroup="Search" />
                        </div>
                    </div>
                </div>
                <div class="col-xs-6">
                    <div class="row">
                        <div class="col-xs-12 text-center">
                            <asp:Button ID="btnNewAttendee" runat="server" Text="Add New Attendee" CssClass="btn btn-lg btn-success" OnClick="btnNewAttendee_Click" />
                        </div>
                    </div>
                </div>
            </div>
            <asp:Panel ID="pAttendeesList" runat="server" Visible="false">
                <div class="row pad-b-10">
                    <asp:Repeater ID="repAttendees" runat="server" OnItemCommand="repAttendees_ItemCommand">
                        <HeaderTemplate>
                            <table style="width: 100%;">
                                <tr class="rowHead">
                                    <td style="padding: 5px;">Last Name</td>
                                    <td style="padding: 5px;">First Name</td>
                                    <td style="padding: 5px;">Character</td>
                                    <td style="padding: 5px;">Camp</td>
                                    <td style="padding: 5px;">Status</td>
                                    <td style="padding: 5px; width: 22px;"></td>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                                <tr class="rowMain">
                                    <td style="padding: 5px;"><%#Eval("LastName")%></td>
                                    <td style="padding: 5px;"><%#Eval("FirstName")%></td>
                                    <td style="padding: 5px;"><%#Eval("CharacterName")%></td>
                                    <td style="padding: 5px;"><%#Eval("CampName")%></td>
                                    <td style="padding: 5px;"><%#Eval("Status")%></td>
                                    <td style="padding: 5px;"><asp:ImageButton ID="btnView" runat="server" ImageUrl="/DesktopModules/RagnarokAdmin/CheckIn/images/View.png" AlternateText="View" CommandName="View" CommandArgument='<%#Eval("Id")%>' /></td>
                                </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                                <tr class="rowAlt">
                                    <td style="padding: 5px;"><%#Eval("LastName")%></td>
                                    <td style="padding: 5px;"><%#Eval("FirstName")%></td>
                                    <td style="padding: 5px;"><%#Eval("CharacterName")%></td>
                                    <td style="padding: 5px;"><%#Eval("CampName")%></td>
                                    <td style="padding: 5px;"><%#Eval("Status")%></td>
                                    <td style="padding: 5px;"><asp:ImageButton ID="btnView" runat="server" ImageUrl="/DesktopModules/RagnarokAdmin/CheckIn/images/View.png" AlternateText="View" CommandName="View" CommandArgument='<%#Eval("Id")%>' /></td>
                                </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </div>
            </asp:Panel>
            <asp:Panel ID="pNoneFound" runat="server" Visible="false">
                <div class="row pad-b-10">
                    <div class="col-xs-12">
                        <p class="error">No registrations found for that name.  Please try again.</p>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
        <asp:Panel ID="pConfirm" runat="server">
            <div class="row">
                <div class="col-xs-12">
                    <h1>Please Confirm your Registration Details</h1>
                </div>
            </div>
            <asp:Panel ID="pCheckedIn" runat="server" Visible="false">
                <div class="checkedInBlock">
                    <p>This participant has already checked in!</p>
                </div>
            </asp:Panel>
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
            <asp:Panel ID="pArrivalDate" runat="server" Visible="false">
                <div class="arrivalBlock">
                    <p>This participant is scheduled to arrive on <asp:Literal ID="litArrival" runat="server" />.</p>
                </div>
            </asp:Panel>
            <asp:Panel ID="pNotPaid" runat="server" Visible="false">
                <div class="notPaidBlock">
                    <p>This participant has NOT paid yet.  Please take payment at check-in.</p>
                </div>
            </asp:Panel>
            <div class="row">
                <div class="col-xs-6">
                    <h2>Participant Information</h2>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>First Name:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litFirstName" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Last Name:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litLastName" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Date of Birth:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litDOB" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Address 1:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litAddress1" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Address 2:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litAddress2" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>City:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litCity" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>State:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litState" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Zip Code:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litZip" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Home Phone:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litHomePhone" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Cell Phone:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litCellPhone" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Email:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litEmail" runat="server" />
                        </div>
                    </div>
                </div>
                <div class="col-xs-6">
                    <h2>Character Information</h2>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Character:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litCharacterName" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Unit:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litUnitName" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Chapter:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litChapterName" runat="server" />
                        </div>
                    </div>
                    <h2>Camp Information</h2>
                    <div class="row" id="campInformation" runat="server">
                        <div class="col-xs-5">
                            <b>Camp:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Label ID="lblCamp" runat="server" />
                        </div>
                    </div>
                    <h2>Emergency Contact Information</h2>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Name:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litEmergencyContact" runat="server" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Phone:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:Literal ID="litEmergencyPhone" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <h2>Health Issues</h2>
                    <asp:Literal ID="litHealthIssues" runat="server" />
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <h2>Waiver Signature</h2>
                    <asp:Label ID="lblNoSignature" runat="server" Text="No Signature Provided!" ForeColor="Red" Font-Bold="true" Visible="false" />
                    <asp:Image ID="imgSignature" runat="server" AlternateText="Signature" CssClass="img-responsive" style="max-width: 800px;" />
                </div>
            </div>
            <div class="row pad-b-10">
                <div class="col-xs-6">
                    <asp:Button ID="btnMakeChanges" runat="server" Text="Make Changes" CssClass="btn btn-lg btn-danger" OnClick="btnMakeChanges_Click" />
                </div>
                <div class="col-xs-6">
                    <asp:Button ID="btnInformationCorrect" runat="server" Text="Registration Information Correct" CssClass="btn btn-lg btn-success right" OnClick="btnInformationCorrect_Click" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pEdit" runat="server" Visible="false">
            <asp:Literal ID="litParticipantId" runat="server" Visible="false" />
            <div class="row">
                <div class="col-xs-12">
                    <h1>Enter Your Registration Details</h1>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <asp:ValidationSummary ID="valSummary" runat="server" DisplayMode="BulletList" CssClass="error" ValidationGroup="Edit" />
                    <asp:RequiredFieldValidator ID="reqFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="First name is required." CssClass="error" ValidationGroup="Edit" Display="None" />
                    <asp:RequiredFieldValidator ID="reqLastName" runat="server" ControlToValidate="txtLastName" ErrorMessage="Last name is required." CssClass="error" ValidationGroup="Edit" Display="None" />
                    <asp:RequiredFieldValidator ID="reqDoB" runat="server" ControlToValidate="txtDOB" ErrorMessage="Date of birth is required." CssClass="error" ValidationGroup="Edit" Display="None" /> 
                    <asp:CompareValidator ID="comDoB" runat="server" ControlToValidate="txtDOB" Operator="DataTypeCheck" Type="Date" ErrorMessage="Date of birth must be a valid date (MM/DD/YYYY)." CssClass="error" ValidationGroup="Edit" Display="None" />
                    <asp:RequiredFieldValidator ID="reqAddress1" runat="server" ControlToValidate="txtAddress1" ErrorMessage="Address 1 is required." CssClass="error" ValidationGroup="Edit" Display="None" />
                    <asp:RequiredFieldValidator ID="reqCity" runat="server" ControlToValidate="txtCity" ErrorMessage="City is required." CssClass="error" ValidationGroup="Edit" Display="None" />
                    <asp:RequiredFieldValidator ID="reqState" runat="server" ControlToValidate="ddlState" InitialValue="" ErrorMessage="State/province is required." CssClass="error" ValidationGroup="Edit" Display="None" />
                    <asp:RequiredFieldValidator ID="reqZip" runat="server" ControlToValidate="txtZip" ErrorMessage="Zip code is required." CssClass="error" ValidationGroup="Edit" Display="None" />
                    <asp:RegularExpressionValidator ID="regZip" runat="server" ControlToValidate="txtZip" ValidationExpression="(^\d{5}(-\d{4})?$)|(^[ABCEGHJKLMNPRSTVXY]{1}\d{1}[A-Z]{1} *\d{1}[A-Z]{1}\d{1}$)" ErrorMessage="Please provide a valid zip code." CssClass="error" ValidationGroup="Edit" Display="None" />
                    <asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." CssClass="error" ValidationGroup="Edit" Display="None" /> 
                    <asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$" ErrorMessage="Email must be a valid email address." CssClass="error" ValidationGroup="Edit" Display="None" />
                    <asp:RequiredFieldValidator ID="reqCharacterName" runat="server" ControlToValidate="txtCharacterName" ErrorMessage="Character name is required." CssClass="error" ValidationGroup="Edit" Display="None" />
                    <asp:RequiredFieldValidator ID="reqCamp" runat="server" ControlToValidate="ddlCamp" InitialValue="0" ErrorMessage="Camp is required." CssClass="error" ValidationGroup="Edit" Display="None" />
                    <asp:RequiredFieldValidator ID="reqEmergencyContactName" runat="server" ControlToValidate="txtEmergencyContactName" ErrorMessage="Emergency contact name is required." CssClass="error" ValidationGroup="Edit" Display="None" />
                    <asp:RequiredFieldValidator ID="reqEmergencyContactPhone" runat="server" ControlToValidate="txtEmergencyContactPhone" ErrorMessage="Emergency contact phone is required." CssClass="error" ValidationGroup="Edit" Display="None" />
                    <asp:RegularExpressionValidator ID="regEmergencyContactPhone" runat="server" ControlToValidate="txtEmergencyContactPhone" ValidationExpression="^(\([2-9]|[2-9])(\d{2}|\d{2}\))(-|.|\s)?\d{3}(-|.|\s)?\d{4}$" ErrorMessage="Please enter a valid phone number." CssClass="error" ValidationGroup="Edit" Display="None" />
                    <asp:CustomValidator ID="cusSignature" runat="server" ClientValidationFunction="checkSignature" ErrorMessage="Signature is required." CssClass="error" ValidationGroup="Edit" Display="None" />
                </div>
            </div>
            <div class="row">
                <div class="col-xs-6">
                    <h2>Participant Information</h2>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>First Name:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtFirstName" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Last Name:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtLastName" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Date of Birth:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtDOB" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Minor:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:CheckBox ID="cbxIsMinor" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Address 1:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtAddress1" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Address 2:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtAddress2" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>City:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtCity" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>State:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:DropDownList ID="ddlState" runat="server" style="width: 100%;">
                                <asp:ListItem Value="">-- States --</asp:ListItem>
                                <asp:ListItem Value="AL">Alabama</asp:ListItem>
	                            <asp:ListItem Value="AK">Alaska</asp:ListItem>
	                            <asp:ListItem Value="AZ">Arizona</asp:ListItem>
	                            <asp:ListItem Value="AR">Arkansas</asp:ListItem>
	                            <asp:ListItem Value="CA">California</asp:ListItem>
	                            <asp:ListItem Value="CO">Colorado</asp:ListItem>
	                            <asp:ListItem Value="CT">Connecticut</asp:ListItem>
                                <asp:ListItem Value="DC">District of Columbia</asp:ListItem>
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
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Zip Code:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtZip" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Home Phone:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtHomePhone" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Cell Phone:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtCellPhone" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Email:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtEmail" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                </div>
                <div class="col-xs-6">
                    <h2>Character Information</h2>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Character:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtCharacterName" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Unit:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtUnitName" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Chapter:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtChapterName" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <h2>Camp Information</h2>
                    <div class="row" id="Div1" runat="server">
                        <div class="col-xs-5">
                            <b>Camp:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:DropDownList ID="ddlCamp" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Merchant:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:CheckBox ID="cbxIsMerchant" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <h2>Emergency Contact Information</h2>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Name:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtEmergencyContactName" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-5">
                            <b>Phone:</b>
                        </div>
                        <div class="col-xs-7">
                            <asp:TextBox ID="txtEmergencyContactPhone" runat="server" style="width: 100%;" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row pad-b-10">
                <div class="col-xs-12">
                    <h2>Health Issues</h2>
                    <asp:TextBox ID="txtHealthIssues" runat="server" TextMode="MultiLine" style="width: 100%; height: 50px;" />
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <h2>Waiver</h2>
                    <p>WAIVER: In consideration of receiving permission from DAGORHIR BATTLE GAMES ASSOCIATION INC. and DAGORHIR RAGNAROK LLC, (Ragnarok) to participate in any Ragnarok 
                    sponsored activity, event, tournament, contest, or meeting, the undersigned assumes full responsibility for any bodily injury and. Or property damage arising out 
                    of or related to my attendance and/or participation. I fully release Ragnarok, its members, participants, observers, officials, and Cooper’s Lake, and/or administering 
                    emergency medical assistance from liability to myself, my assigns, heirs and next of kin for any injury to myself or damage to my property arising out of my 
                    attending/participating in a Ragnarok event/activity. I hereby agree that if at any time I feel any Ragnarok activity/event to be unsafe or if I observe unsafe behavior 
                    on the part of other participants/observers, I will immediately notify the appropriate Ragnarok officials and/or refuse to participate in or observe any further 
                    activities/events. The undersigned is aware  of the risks and hazards inherent in participating in any activity, event, tournament, contest, or meeting of Ragnarok 
                    and elects voluntarily to participate, knowing that participation involves significant physical contact by others to his or her person and that such participation 
                    may entail a risk of injury.</p>
                    <p>By signing below I acknowledge:</p>
                    <ul>
                        <li>I accept the above release.</li>
                        <li>I am over 18 years and of sound mind.</li>
                        <li>I have no known physical or mental defects that would increase the likelihood of serious injury if I intend to participate in Ragnarok activities.</li>
                    </ul>
                    <h2>Waiver Signature</h2>
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pCheckIn" runat="server" Visible="false">
            <div class="row">
                <div class="col-xs-12">
                    <h1>Enter Rag Tag # and Check In</h1>
                </div>
            <div class="row">
                <div class="col-xs-12">
                    <asp:ValidationSummary ID="valSummary2" runat="server" DisplayMode="BulletList" CssClass="error" ValidationGroup="CheckIn" />
                    <asp:RequiredFieldValidator ID="reqTag" runat="server" ControlToValidate="txtTagNumber" ErrorMessage="Tag Number is required." CssClass="error" ValidationGroup="CheckIn" Display="None" /> 
                    <asp:CompareValidator ID="comTag" runat="server" ControlToValidate="txtTagNumber" Operator="DataTypeCheck" Type="Integer" ErrorMessage="Tag Number must be a valid number." CssClass="error" ValidationGroup="CheckIn" Display="None" />
                </div>
            </div>
                <div class="col-xs-12">
                    <h2>Rag Tag #</h2>
                    <asp:TextBox ID="txtTagNumber" runat="server" ValidationGroup="CheckIn" />
                    
                </div>
            </div>
            <div class="row pad-b-10 pad-t-10">
                <div class="col-xs-6">
                    <asp:Button ID="btnCheckIn" runat="server" Text="Check In" CssClass="btn btn-lg btn-success" OnClick="btnCheckIn_Click" ValidationGroup="CheckIn" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pThankYou" runat="server" Visible="false">
            <div class="row">
                <div class="col-xs-12">
                    <h1>Thank You For Checking In</h1>
                </div>
                <div class="col-xs-12">
                    <p>You have now checked in to Ragnarok XXXII!  We hope you have a great time!</p>
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnCancel" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>

            <div id="signatureDiv" class="signatureDiv" runat="server" style="height:0; overflow: hidden;">
                <div id="signature-pad" class="m-signature-pad">
                    <div class="m-signature-pad--body">
                        <canvas id="signaturePad" width="500" height="318"></canvas>
                    </div>
                    <div class="m-signature-pad--footer">
                        <div id="clearSignature" onclick="signaturePad.clear();">
                            <img src="/DesktopModules/RagnarokRegistration/images/clearsignature.png" alt="Clear" />
                        </div>
                    </div>
                </div>
            </div>

<asp:UpdatePanel ID="upCheckInBottom" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pEditBottom" runat="server" Visible="false">
            <input type="hidden" ID="signatureCode" runat="server" class="signatureCode" />
            <div class="row">
                <div class="col-xs-6">
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-lg btn-default" OnClick="btnCancel_Click" />
                </div>
                <div class="col-xs-6">
                    <asp:Button ID="btnSave" runat="server" Text="Save Registration" CssClass="btn btn-lg btn-success right" OnClientClick="writeSignature();" OnClick="btnSave_Click" ValidationGroup="Edit" />
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/signature_pad/1.5.3/signature_pad.js"></script>
<script type="text/javascript" src="/DesktopModules/RagnarokAdmin/CheckIn/js/signature.js"></script>

<script type="text/javascript">
    var sf = $.ServicesFramework(<%:ModuleContext.ModuleId%>);

    function showSignature()
    {
        $('.signatureDiv').removeAttr('style');
    }

    function hideSignature() {
        $('.signatureDiv').attr('style', 'height:0; overflow: hidden;');
    }

    function checkSignature(source, arguments) {
        arguments.IsValid = true;

        if (signaturePad.isEmpty())
        {
            arguments.IsValid = false;
        }
    }

    function writeSignature() {
        var signatureData = signaturePad.toDataURL();

        $('.signatureCode').val(signatureData);

        //alert($('.signatureCode').val());
    }

    function showOverlay() {
        $('#overlay').show();
    }

    function hideOverlay() {
        $('#overlay').hide();
    }
</script>