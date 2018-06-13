<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Ragnarok.Modules.RagnarokRegistration.New.View" %>

<asp:UpdatePanel ID="upMain" runat="server">
    <ContentTemplate>
        <asp:Label ID="lblDebug" runat="server" />
        <asp:Panel ID="pRegistrationInfo" runat="server">
            <h1>Ragnarok XXXIII Pre-Registration</h1>
            <p>Welcome to the Ragnarok XXXIII Pre-Registration page.</p>
            <p>To pre-register for Ragnarok XXXIII, please provide the requested information in the following sections.</p>
            <h2>Registration Type</h2>
            <p>Please select the type of registration you wish to submit.</p>
            <p><b>You will not be permitted on site before the day you select for arrival.</b></p>
            <p><b>NOTE:</b> if you are pre-registering a minor, you will have to complete a waiver in person at Troll when you check in, in addition to the information in the following sections.</p>
            <div>
                <div class="regArea">
                    <h2>Adult Registrations</h2>
                    <asp:RadioButton ID="rbAdultSaturday" runat="server" Text="Saturday (06/16/2018) - $75" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbAdultSunday" runat="server" Text="Sunday (06/17/2018) - $60" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbAdultMonday" runat="server" Text="Monday (06/18/2018) - $60" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbAdultTuesday" runat="server" Text="Tuesday (06/19/2018) - $60" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbAdultWednesday" runat="server" Text="Wednesday (06/20/2018) - $60" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbAdultThursday" runat="server" Text="Thursday (06/21/2018) - $55" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbAdultFriday" runat="server" Text="Friday (06/22/2018) - $45" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbAdultSaturday2" runat="server" Text="Saturday (06/23/2018) - $35" CssClass="radioButton" GroupName="RegType" />
                </div>
                <div class="regArea">
                    <h2>Child Registrations</h2>
                    <asp:RadioButton ID="rbChildSaturday" runat="server" Text="Saturday (06/16/2018) - $50" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbChildSunday" runat="server" Text="Sunday (06/17/2018) - $45" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbChildMonday" runat="server" Text="Monday (06/18/2018) - $45" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbChildTuesday" runat="server" Text="Tuesday (06/19/2018) - $45" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbChildWednesday" runat="server" Text="Wednesday (06/20/2018) - $45" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbChildThursday" runat="server" Text="Thursday (06/21/2018) - $30" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbChildFriday" runat="server" Text="Friday (06/22/2018) - $20" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbChildSaturday2" runat="server" Text="Saturday (06/23/2018) - $15" CssClass="radioButton" GroupName="RegType" />
                </div>
                <div class="regArea">
                    <h2>Merchant Registrations</h2>
                    <asp:RadioButton ID="rbMerchant2020" runat="server" Text="20x20 Booth - $90" CssClass="radioButton" GroupName="RegType" /><br />
                    <asp:RadioButton ID="rbMerchant4020" runat="server" Text="40x20 Booth - $100" CssClass="radioButton" GroupName="RegType" />
                </div>
                <div class="clearBoth"></div>
            </div>
            <div>
                <asp:CustomValidator ID="cusType" runat="server" ClientValidationFunction="valType" Display="Dynamic" ErrorMessage="<br />Registration type is required." CssClass="error" ValidationGroup="Type" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pParticipantInfo" runat="server" Visible="false">
            <h1>Ragnarok XXXII Pre-Registration</h1>
            <h2>Participant Information</h2>
            <p>Please provide some information about yourself (or your minor) for our records.</p>
            <span class="required">* indicates required fields</span><br />
            <div ID="businessName" runat="server" Visible="false" class="item">
                <label for="firstName">Business Name: <span class="required">*</span></label>
                <asp:TextBox ID="txtBusinessName" runat="server" />
            </div>
            <div class="item">
                <label for="firstName">First Name: <span class="required">*</span></label>
                <asp:TextBox ID="txtFirstName" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqFirstName" runat="server" ControlToValidate="txtFirstName" Display="Dynamic" ErrorMessage="First name is required." CssClass="error" ValidationGroup="Participant" />
                </div>
            </div>
            <div class="item">
                <label for="lastName">Last Name: <span class="required">*</span></label>
                <asp:TextBox ID="txtLastName" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqLastName" runat="server" ControlToValidate="txtLastName" Display="Dynamic" ErrorMessage="Last name is required." CssClass="error" ValidationGroup="Participant" />
                </div>
            </div>
            <div class="item">
                <label for="dateOfBirth">Date of Birth: <span class="required">*</span></label>
                <asp:TextBox ID="txtDateOfBirth" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqDoB" runat="server" ControlToValidate="txtDateOfBirth" Display="Dynamic" ErrorMessage="Date of birth is required." CssClass="error" ValidationGroup="Participant" /> 
                    <asp:CompareValidator ID="comDoB" runat="server" ControlToValidate="txtDateOfBirth" Operator="DataTypeCheck" Type="Date" Display="Dynamic" ErrorMessage="Date of birth must be a valid date (MM/DD/YYYY)." CssClass="error" ValidationGroup="Participant" />
                </div>
            </div>
            <div class="item">
                <label for="address1">Address 1: <span class="required">*</span></label>
                <asp:TextBox ID="txtAddress1" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqAddress1" runat="server" ControlToValidate="txtAddress1" Display="Dynamic" ErrorMessage="Address 1 is required." CssClass="error" ValidationGroup="Participant" />
                </div>
            </div>
            <div class="item">
                <label for="address2">Address 2: </label>
                <asp:TextBox ID="txtAddress2" runat="server" />
            </div>
            <div class="item">
                <label for="city">City: <span class="required">*</span></label>
                <asp:TextBox ID="txtCity" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqCity" runat="server" ControlToValidate="txtCity" Display="Dynamic" ErrorMessage="City is required." CssClass="error" ValidationGroup="Participant" />
                </div>
            </div>
            <div class="item">
                <label for="state">State/Province: <span class="required">*</span></label>
                <asp:DropDownList ID="ddlState" runat="server">
                    <asp:ListItem Value="" Text="-- States --" />
                    <asp:ListItem Value="AL" Text="Alabama" />
                    <asp:ListItem Value="AK" Text="Alaska" />
                    <asp:ListItem Value="AZ" Text="Arizona" />
                    <asp:ListItem Value="AR" Text="Arkansas" />
                    <asp:ListItem Value="CA" Text="California" />
                    <asp:ListItem Value="CO" Text="Colorado" />
                    <asp:ListItem Value="CT" Text="Connecticut" />
                    <asp:ListItem Value="DE" Text="Delaware" />
                    <asp:ListItem Value="DC" Text="District of Columbia" />
                    <asp:ListItem Value="FL" Text="Florida" />
                    <asp:ListItem Value="GA" Text="Georgia" />
                    <asp:ListItem Value="HI" Text="Hawaii" />
                    <asp:ListItem Value="ID" Text="Idaho" />
                    <asp:ListItem Value="IL" Text="Illinois" />
                    <asp:ListItem Value="IN" Text="Indiana" />
                    <asp:ListItem Value="IA" Text="Iowa" />
                    <asp:ListItem Value="KS" Text="Kansas" />
                    <asp:ListItem Value="KY" Text="Kentucky" />
                    <asp:ListItem Value="LA" Text="Louisiana" />
                    <asp:ListItem Value="ME" Text="Maine" />
                    <asp:ListItem Value="MD" Text="Maryland" />
                    <asp:ListItem Value="MA" Text="Massachusetts" />
                    <asp:ListItem Value="MI" Text="Michigan" />
                    <asp:ListItem Value="MN" Text="Minnesota" />
                    <asp:ListItem Value="MS" Text="Mississippi" />
                    <asp:ListItem Value="MO" Text="Missouri" />
                    <asp:ListItem Value="MT" Text="Montana" />
                    <asp:ListItem Value="NE" Text="Nebraska" />
                    <asp:ListItem Value="NV" Text="Nevada" />
                    <asp:ListItem Value="NH" Text="New Hampshire" />
                    <asp:ListItem Value="NJ" Text="New Jersey" />
                    <asp:ListItem Value="NM" Text="New Mexico" />
                    <asp:ListItem Value="NY" Text="New York" />
                    <asp:ListItem Value="NC" Text="North Carolina" />
                    <asp:ListItem Value="ND" Text="North Dakota" />
                    <asp:ListItem Value="OH" Text="Ohio" />
                    <asp:ListItem Value="OK" Text="Oklahoma" />
                    <asp:ListItem Value="OR" Text="Oregon" />
                    <asp:ListItem Value="PA" Text="Pennsylvania" />
                    <asp:ListItem Value="RI" Text="Rhode Island" />
                    <asp:ListItem Value="SC" Text="South Carolina" />
                    <asp:ListItem Value="SD" Text="South Dakota" />
                    <asp:ListItem Value="TN" Text="Tennessee" />
                    <asp:ListItem Value="TX" Text="Texas" />
                    <asp:ListItem Value="UT" Text="Utah" />
                    <asp:ListItem Value="VT" Text="Vermont" />
                    <asp:ListItem Value="VA" Text="Virginia" />
                    <asp:ListItem Value="WA" Text="Washington" />
                    <asp:ListItem Value="WV" Text="West Virginia" />
                    <asp:ListItem Value="WI" Text="Wisconsin" />
                    <asp:ListItem Value="WY" Text="Wyoming" />
                    <asp:ListItem Value="" Text="" />
                    <asp:ListItem Value="" Text="-- Provinces --" />
                    <asp:ListItem Value="AB" Text="Alberta" />
                    <asp:ListItem Value="BC" Text="British Columbia" />
                    <asp:ListItem Value="LB" Text="Labrador" />
                    <asp:ListItem Value="MB" Text="Manitoba" />
                    <asp:ListItem Value="NB" Text="New Brunswick" />
                    <asp:ListItem Value="NF" Text="Newfoundland" />
                    <asp:ListItem Value="NS" Text="Nova Scotia" />
                    <asp:ListItem Value="NU" Text="Nunavut" />
                    <asp:ListItem Value="NW" Text="Northwest Territories" />
                    <asp:ListItem Value="ON" Text="Ontario" />
                    <asp:ListItem Value="PE" Text="Prince Edward Island" />
                    <asp:ListItem Value="QC" Text="Quebec" />
                    <asp:ListItem Value="SK" Text="Saskatchewan" />
                    <asp:ListItem Value="YU" Text="Yukon" />
                </asp:DropDownList>
                <div>
                    <asp:RequiredFieldValidator ID="reqState" runat="server" ControlToValidate="ddlState" InitialValue="" Display="Dynamic" ErrorMessage="State/province is required." CssClass="error" ValidationGroup="Participant" />
                </div>
            </div>
            <div class="item">
                <label for="zip">Zip Code: <span class="required">*</span></label>
                <asp:TextBox ID="txtZip" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqZip" runat="server" ControlToValidate="txtZip" Display="Dynamic" ErrorMessage="Zip code is required." CssClass="error" ValidationGroup="Participant" />
                    <asp:RegularExpressionValidator ID="regZip" runat="server" ControlToValidate="txtZip" ValidationExpression="(^\d{5}(-\d{4})?$)|(^[ABCEGHJKLMNPRSTVXY]{1}\d{1}[A-Z]{1} *\d{1}[A-Z]{1}\d{1}$)" Display="Dynamic" ErrorMessage="Please provide a valid zip code." CssClass="error" ValidationGroup="Participant" />
                </div>
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
                <div>
                    <asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Email is required." CssClass="error" ValidationGroup="Participant" /> 
                    <asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$" Display="Dynamic" ErrorMessage="Email must be a valid email address." CssClass="error" ValidationGroup="Participant" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pCharacterInfo" runat="server" Visible="false">
            <h1>Ragnarok XXXII Pre-Registration</h1>
            <h2>Character Information</h2>
            <p>Please provide some information about your (or your minor's) in-game character.</p>
            <span class="required">* indicates required fields</span><br />
            <div class="item">
                <label for="characterName">Character Name: <span class="required">*</span></label>
                <asp:TextBox ID="txtCharacterName" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqCharacterName" runat="server" ControlToValidate="txtCharacterName" Display="Dynamic" ErrorMessage="Character name is required." CssClass="error" ValidationGroup="Character" />
                </div>
            </div>
            <div class="item">
                <label for="chapterName">Chapter Name:</label>
                <asp:TextBox ID="txtChapterName" runat="server" />
            </div>
            <div ID="unitName" runat="server" class="item">
                <label for="unitName" id="lblUnitName">Unit Name:</label>
                <asp:TextBox ID="txtUnitName" runat="server" />
            </div>
            <h2>Camp Information</h2>
            <p>If your camp is not listed, please have your camp master contact <a href="mailto:troll@dagorhirragnarok.com">troll@dagorhirragnarok.com</a> to register your camp. If you 
                wish to register without selecting your camp, you can select "Camp Unknown - Will Select Later". You will be required to select your camp when you check in at Troll.</p>
            <div class="item">
                <label for="camp">Camp: <span class="required">*</span></label>
                <asp:DropDownList ID="ddlCamp" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqCamp" runat="server" ControlToValidate="ddlCamp" InitialValue="" Display="Dynamic" ErrorMessage="Camp is required." CssClass="error" ValidationGroup="Character" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pEmergencyInfo" runat="server" Visible="false">
            <h1>Ragnarok XXXII Pre-Registration</h1>
            <h2>Emergency Contact Information</h2>
            <p>Please provide a person who should be contacted in the case of emergency.</p>
            <span class="required">* indicates required fields</span><br />
            <div class="item">
                <label for="emergencyContactName">Emergency Contact Name: <span class="required">*</span></label>
                <asp:TextBox ID="txtEmergencyContactName" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqEmergencyContactName" runat="server" ControlToValidate="txtEmergencyContactName" Display="Dynamic" ErrorMessage="Emergency contact name is required." CssClass="error" ValidationGroup="Emergency" />
                </div>
            </div>
            <div class="item">
                <label for="emergencyContactPhone">Emergency Contact Phone: <span class="required">*</span></label>
                <asp:TextBox ID="txtEmergencyContactPhone" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqEmergencyContactPhone" runat="server" ControlToValidate="txtEmergencyContactPhone" Display="Dynamic" ErrorMessage="Emergency contact phone is required." CssClass="error" ValidationGroup="Emergency" />
                    <asp:RegularExpressionValidator ID="regEmergencyContactPhone" runat="server" ControlToValidate="txtEmergencyContactPhone" ValidationExpression="^(\([2-9]|[2-9])(\d{2}|\d{2}\))(-|.|\s)?\d{3}(-|.|\s)?\d{4}$" Display="Dynamic" ErrorMessage="Please enter a valid phone number." CssClass="error" ValidationGroup="Emergency" />
                </div>
            </div>
            <h2>Health Issues</h2>
            <p>Any health issues indicated here will be kept confidential and will only be used if aid is administered.</p>
            <div id="healthIssues">
                <div class="item healthIssue">
                    <label>Health Issues: </label>
                    <asp:TextBox ID="txtHealthIssue" runat="server" TextMode="MultiLine" CssClass="healthIssueText" />
                </div>
            </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pWaiverInfo" runat="server" Visible="false">
            <h1>Ragnarok XXXII Pre-Registration</h1>
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
            <p><input type="checkbox" ID="cbxAcceptRelease" runat="server" /> I understand that by registering for Ragnarok, I accept the above release.</p>
            <p><input type="checkbox" ID="cbxAcceptOver18" runat="server" /> I acknowledge that I am over 18 years and of sound mind.</p>
            <p><input type="checkbox" ID="cbxAcceptActivities" runat="server" /> I acknowledge that I have no known physical or mental defects that would increase the likelihood of serious injury if I intend to participate in Ragnarok activities.</p>
            <div>
                <asp:Label ID="lblCheckboxError" runat="server" Text="You must agree to all of the above statements to proceed." CssClass="error" Visible="false" />
            </div>
            <h2>Sign Here</h2>
            <div>
                <asp:Label ID="lblSignatureError" runat="server" Text="Your signature is required." CssClass="error" Visible="false" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pPaymentInfo" runat="server" Visible="false">
            <h1>Ragnarok XXXII Pre-Registration</h1>
            <h2>Payment Information</h2>
            <p>Please provide your payment information. Payment is handled through a secure connection to a third party merchant portal, and no payment information is stored by us.</p>
            <span class="required">* indicates required fields</span><br />
            <div class="item">
                <label for="creditCardNumber">Credit Card Number: <span class="required">*</span></label>
                <asp:TextBox ID="txtCreditCardNumber" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqCCNumber" runat="server" ControlToValidate="txtCreditCardNumber" Display="Dynamic" ErrorMessage="Credit card number is required." CssClass="error" ValidationGroup="Payment" />
                </div>
            </div>
            <div class="item">
                <label for="creditCardExpirationMonth">Expiration Month: <span class="required">*</span></label>
                <asp:DropDownList ID="ddlExpirationMonth" runat="server">
                    <asp:ListItem Value="01" Text="January" />
                    <asp:ListItem Value="02" Text="Februray" />
                    <asp:ListItem Value="03" Text="March" />
                    <asp:ListItem Value="04" Text="April" />
                    <asp:ListItem Value="05" Text="May" />
                    <asp:ListItem Value="06" Text="June" />
                    <asp:ListItem Value="07" Text="July" />
                    <asp:ListItem Value="08" Text="August" />
                    <asp:ListItem Value="09" Text="September" />
                    <asp:ListItem Value="10" Text="October" />
                    <asp:ListItem Value="11" Text="November" />
                    <asp:ListItem Value="12" Text="December" />
                </asp:DropDownList>
            </div>
            <div class="item">
                <label for="creditCardExpirationYear">Expiration Year: <span class="required">*</span></label>
                <asp:DropDownList ID="ddlExpirationYear" runat="server">
                    <asp:ListItem Value="17" Text="2017" />
                    <asp:ListItem Value="18" Text="2018" />
                    <asp:ListItem Value="19" Text="2019" />
                    <asp:ListItem Value="20" Text="2020" />
                    <asp:ListItem Value="21" Text="2021" />
                    <asp:ListItem Value="22" Text="2022" />
                    <asp:ListItem Value="23" Text="2023" />
                    <asp:ListItem Value="24" Text="2024" />
                    <asp:ListItem Value="25" Text="2025" />
                    <asp:ListItem Value="26" Text="2026" />
                    <asp:ListItem Value="27" Text="2027" />
                    <asp:ListItem Value="28" Text="2028" />
                    <asp:ListItem Value="29" Text="2029" />
                    <asp:ListItem Value="30" Text="2030" />
                    <asp:ListItem Value="31" Text="2031" />
                    <asp:ListItem Value="32" Text="2032" />
                    <asp:ListItem Value="33" Text="2033" />
                    <asp:ListItem Value="34" Text="2034" />
                    <asp:ListItem Value="35" Text="2035" />
                    <asp:ListItem Value="36" Text="2036" />
                </asp:DropDownList>
            </div>
            <asp:Label ID="cardError" runat="server" CssClass="error" />
        </asp:Panel>
        <asp:Panel ID="pConfirmation" runat="server" Visible="false">
            <h1>Thank you for registering for Ragnarok</h1>
            <p>Your registration has been successfully submitted.  You will receive an email with your registration confirmation and a QR code which you can bring 
            with you to check in at Troll for expedited event check in.</p>
            <p>We look forward to seeing you at Ragnarok this year!</p>
            <asp:Label ID="lblTest" runat="server" />
        </asp:Panel>
        <asp:Panel ID="pError" runat="server" Visible="false">
            <h1>An error occurred with your registration</h1>
            <p>We're sorry, but an error occurred with your registration!  Please contact Troll at <a href="mailto:troll@dagorhirragnarok.com">troll@dagorhirragnarok.com</a> 
            and we'll resolve the issue as quickly as possible for you!</p>
            <p>We look forward to seeing you at Ragnarok this year!</p>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="btnNextParticipantInfo" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnPreviousRegistrationInfo" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnNextCharacterInfo" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnPreviousParticipationInfo" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnNextEmergencyInfo" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnPreviousCharacterInfo" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnNextWaiverInfo" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnPreviousEmergencyInfo" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnNextPaymentInfo" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnPreviousWaiverInfo" EventName="Click" />
        <asp:AsyncPostBackTrigger ControlID="btnRegister" EventName="Click" />
    </Triggers>
</asp:UpdatePanel>


        <div id="signatureDiv" class="signatureDiv" runat="server" style="height:0; overflow: hidden;">
            <div id="signature-pad" class="m-signature-pad">
                <div class="m-signature-pad--body">
                    <canvas id="signaturePad" width="658" height="318"></canvas>
                </div>
                <div class="m-signature-pad--footer">
                    <div id="clearSignature" onclick="signaturePad.clear();">
                        <img src="/DesktopModules/RagnarokRegistration/images/clearsignature.png" alt="Clear" />
                    </div>
                </div>
            </div>
        </div>
        <input type="hidden" ID="signatureCode" runat="server" class="signatureCode" />

<asp:UpdatePanel ID="upFooter" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pRegistrationFooter" runat="server">
            <div class="footer">
                <div class="next">
                    <asp:ImageButton ID="btnNextParticipantInfo" runat="server" ImageUrl="/DesktopModules/RagnarokRegistration/images/next.png" AlternateText="Next" 
                        OnClick="btnNextParticipantInfo_Click" ValidationGroup="Type" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pParticipantFooter" runat="server" Visible="false">
            <div class="footer">
                <div class="previous">
                    <asp:ImageButton ID="btnPreviousRegistrationInfo" runat="server" ImageUrl="/DesktopModules/RagnarokRegistration/images/previous.png" AlternateText="Next" 
                        OnClick="btnPreviousRegistrationInfo_Click" />
                </div>
                <div class="next">
                    <asp:ImageButton ID="btnNextCharacterInfo" runat="server" ImageUrl="/DesktopModules/RagnarokRegistration/images/next.png" AlternateText="Next" 
                        OnClick="btnNextCharacterInfo_Click" ValidationGroup="Participant" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pCharacterFooter" runat="server" Visible="false">
            <div class="footer">
                <div class="previous">
                    <asp:ImageButton ID="btnPreviousParticipationInfo" runat="server" ImageUrl="/DesktopModules/RagnarokRegistration/images/previous.png" AlternateText="Next" 
                        OnClick="btnPreviousParticipationInfo_Click" />
                </div>
                <div class="next">
                    <asp:ImageButton ID="btnNextEmergencyInfo" runat="server" ImageUrl="/DesktopModules/RagnarokRegistration/images/next.png" AlternateText="Next" 
                        OnClick="btnNextEmergencyInfo_Click" ValidationGroup="Character" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pEmergencyFooter" runat="server" Visible="false">
            <div class="footer">
                <div class="previous">
                    <asp:ImageButton ID="btnPreviousCharacterInfo" runat="server" ImageUrl="/DesktopModules/RagnarokRegistration/images/previous.png" AlternateText="Next" 
                        OnClick="btnPreviousCharacterInfo_Click" />
                </div>
                <div class="next">
                    <asp:ImageButton ID="btnNextWaiverInfo" runat="server" ImageUrl="/DesktopModules/RagnarokRegistration/images/next.png" AlternateText="Next" 
                        OnClick="btnNextWaiverInfo_Click" ValidationGroup="Emergency" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pWaiverFooter" runat="server" Visible="false">
            <div class="footer">
                <div class="previous">
                    <asp:ImageButton ID="btnPreviousEmergencyInfo" runat="server" ImageUrl="/DesktopModules/RagnarokRegistration/images/previous.png" AlternateText="Next" 
                        OnClientClick="writeSignature();" OnClick="btnPreviousEmergencyInfo_Click" />
                </div>
                <div class="next">
                    <asp:ImageButton ID="btnNextPaymentInfo" runat="server" ImageUrl="/DesktopModules/RagnarokRegistration/images/next.png" AlternateText="Next" 
                        OnClientClick="writeSignature();" OnClick="btnNextPaymentInfo_Click" />
                </div>
            </div>
        </asp:Panel>
        <asp:Panel ID="pPaymentFooter" runat="server" Visible="false">
            <div class="footer">
                <div class="previous">
                    <asp:ImageButton ID="btnPreviousWaiverInfo" runat="server" ImageUrl="/DesktopModules/RagnarokRegistration/images/previous.png" AlternateText="Next" 
                        OnClick="btnPreviousWaiverInfo_Click" />
                </div>
                <div id="register" class="next">
                    <asp:ImageButton ID="btnRegister" runat="server" ImageUrl="/DesktopModules/RagnarokRegistration/images/register.png" AlternateText="Register for Ragnarok" 
                        OnClientClick="showOverlay();" OnClick="btnRegister_Click" ValidationGroup="Payment" />
                </div>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/signature_pad/1.5.3/signature_pad.js"></script>
<script type="text/javascript" src="/DesktopModules/RagnarokRegistration/js/signature.js"></script>
<script type="text/javascript" src="/DesktopModules/RagnarokRegistration/New/js/validation.js"></script>

<script type="text/javascript">
    var sf = $.ServicesFramework(<%:ModuleContext.ModuleId%>);

    function showSignature()
    {
        $('.signatureDiv').removeAttr('style');
    }

    function hideSignature() {
        $('.signatureDiv').attr('style', 'height:0; overflow: hidden;');
    }

    function writeSignature() {
        var signatureData = signaturePad.toDataURL();

        $('.signatureCode').val(signatureData);
    }

    function showOverlay() {
        $('#overlay').show();
    }

    function hideOverlay() {
        $('#overlay').hide();
    }
</script>