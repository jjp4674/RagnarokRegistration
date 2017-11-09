<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Ragnarok.Modules.RagnarokRegistration.RagU.View" %>

<asp:UpdatePanel ID="upMain" runat="server">
    <ContentTemplate>
        <asp:Panel ID="pTrack" runat="server">
            <h1>Ragnarok XXXII Ragnarok University Track Pre-Registration</h1>
            <p>To pre-register for a Ragnarok University Track, simply select which you'd like to do and provide your payment information.</p>
            <div class="item">
                <h2><asp:CheckBox ID="cbxLeather" runat="server" OnCheckedChanged="cbxLeather_CheckedChanged" Text="Leatherworking Track - $55" AutoPostBack="true" /></h2>
                <p>Taught By: Timmourne<br />
                    Spaces Remaining: <asp:Label ID="lblLeatherRemaining" runat="server" Font-Bold="true" />
                </p>
                <p>The class will be a guided workshop into basic leatherworking. Students will be provided with the leather, eyelets, beveler, swivel knife and hammer 
                    necessary to create a pair of bracers, and will be instructed in their use.  Some basic tooling will also be taught.  At the end of the class, you 
                    will have a pair of bracers, and will keep the beveler, swivel knife and hammer you used as well to start your toolkit!
                </p>
                <p><b>Hours:</b><br />
                    Monday: 1PM - 3PM<br />
                    Tuesday: 1PM - 3PM<br />
                    Friday: 1PM - 3PM
                </p>
                <p><b>Supplies Needed:</b><br />
                    None
                </p>
            </div>
            <div class="item">
                <h2><asp:CheckBox ID="cbxPlastiDip" runat="server" OnCheckedChanged="cbxPlastiDip_CheckedChanged" Text="Plastidip Track - $100" AutoPostBack="true" /></h2>
                <p>Taught By: Severing Meanmug<br />
                    Spaces Remaining: <asp:Label ID="lblPlastiRemaining" runat="server" Font-Bold="true" /></p>
                <p>The class will be a guided workshop into plastidipping techniques for shields. The first two days we will go over carving and shaping techniques 
                    and attach the foam shapes to your board, Wednesday is a day of rest, then the last two days will be plastidip coats and painting techniques. 
                    Extracurricular work time on your shields is highly encouraged, but the goal is to have a finished buckler by the end of week.</p>
                <p><b>Hours:</b><br />
                    Monday: 5PM - 7PM<br />
                    Tuesday: 5PM - 7PM<br />
                    Thursday: 5PM - 7PM<br />
                    Friday: 5PM - 7PM
                </p>
                <p><b>Supplies Needed:</b><br />
                    None<br />
                    <b>NOTE: </b> You will be provided a shield to work with by Gorg.  There are 4 strap shields and 6 punch shields available, which will be claimed at the first class 
                    on a first come, first served basis.
                </p>
            </div>
            <asp:CustomValidator ID="cusChecks" runat="server" OnServerValidate="cusChecks_ServerValidate" Display="Dynamic" ErrorMessage="You must select at least one track." CssClass="error" />
            <h2>Total Cost</h2>
            <div class="item">
                Total Cost: <asp:Label ID="lblCost" runat="server" Font-Bold="true" /> <asp:Literal ID="litCost" runat="server" Visible="false" />
            </div>
            <h2>Participant Information</h2>
            <div class="item">
                <label for="firstName">First Name: <span class="required">*</span></label>
                <asp:TextBox ID="txtFirstName" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqFirstName" runat="server" ControlToValidate="txtFirstName" Display="Dynamic" ErrorMessage="First name is required." CssClass="error" />
                </div>
            </div>
            <div class="item">
                <label for="lastName">Last Name: <span class="required">*</span></label>
                <asp:TextBox ID="txtLastName" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqLastName" runat="server" ControlToValidate="txtLastName" Display="Dynamic" ErrorMessage="Last name is required." CssClass="error" />
                </div>
            </div>
            <div class="item">
                <label for="lastName">Email: <span class="required">*</span></label>
                <asp:TextBox ID="txtEmail" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="txtEmail" Display="Dynamic" ErrorMessage="Email is required." CssClass="error" /> 
                    <asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$" Display="Dynamic" ErrorMessage="Email must be a valid email address." CssClass="error" />
                </div>
            </div>
            <h2>Payment Information</h2>
            <p>Please provide your payment information. Payment is handled through a secure connection to a third party merchant portal, and no payment information is stored by us.</p>
            <span class="required">* indicates required fields</span><br />
            <div class="item">
                <label for="creditCardNumber">Credit Card Number: <span class="required">*</span></label>
                <asp:TextBox ID="txtCreditCardNumber" runat="server" />
                <div>
                    <asp:RequiredFieldValidator ID="reqCCNumber" runat="server" ControlToValidate="txtCreditCardNumber" Display="Dynamic" ErrorMessage="Credit card number is required." CssClass="error" />
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
            <div id="register">
                <asp:ImageButton ID="btnRegister" runat="server" ImageUrl="/DesktopModules/RagnarokRegistration/images/registerRagU.png" AlternateText="Register for Track(s)" 
                    OnClick="btnRegister_Click" />
            </div>
        </asp:Panel>
        <asp:Panel ID="pConfirmation" runat="server" Visible="false">
            <h1>Thank you for registering for Ragnarok University</h1>
            <p>Your registration has been successfully submitted.  Thank you for your interest in Ragnarok University!</p>
            <p>We look forward to seeing you at Ragnarok this year!</p>
            <asp:Label ID="lblTest" runat="server" />
        </asp:Panel>
        <asp:Panel ID="pError" runat="server" Visible="false">
            <h1>An error occurred with your registration</h1>
            <p>We're sorry, but an error occurred with your Ragnarok University Track registration!  Please contact Ragnarok University at <a href="mailto:ragnarokuniversity@dagorhirragnarok.com">ragnarokuniversity@dagorhirragnarok.com</a> 
            and we'll resolve the issue as quickly as possible for you!</p>
            <p>We look forward to seeing you at Ragnarok this year!</p>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>