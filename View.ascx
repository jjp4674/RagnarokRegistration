<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Ragnarok.Modules.RagnarokRegistration.View" %>

<div id="registrationInfo">
    <h1>Ragnarok XXXII Pre-Registration</h1>
    <p>Welcome to the Ragnarok XXXII Pre-Registration page.</p>
    <p>To pre-register for Ragnarok XXXII, please provide the requested information in the following sections.</p>
    <h2>Registration Type</h2>
    <p>Please select the type of registration you wish to submit.</p>
    <p><b>NOTE:</b> if you are pre-registering a minor, you will have to complete a waiver in person at Troll when you check in, in addition to the information in the following sections.</p>
    <div class="item">
        <label for="registrationCost">Registration Type: <span class="required">*</span></label>
        <select id="registrationCost" name="registrationCost">
            <option value="75">Adult Saturday - $75</option>
            <option value="60">Adult Sunday - $60</option>
            <option value="50">Child Saturday - $50</option>
            <option value="40">Child Sunday - $40</option>
            <option value=""></option>
            <option value="">--------- Merchants ---------</option>
            <option value="90">Merchant - 20x20 Booth - $90</option>
            <option value="100">Merchant - 40x20 Booth - $100</option>
        </select>
    </div>
    <div class="footer">
        <div id="nextParticipantInfo" class="next"><img src="/DesktopModules/RagnarokRegistration/images/next.png" alt="Next" /></div>
    </div>
</div>
<div id="participantInfo">
    <h1>Ragnarok XXXII Pre-Registration</h1>
    <h2>Participant Information</h2>
    <p>Please provide some information about yourself (or your minor) for our records.</p>
    <span class="required">* indicates required fields</span><br />
    <div class="item">
        <label for="firstName">First Name: <span class="required">*</span></label>
        <input id="firstName" name="firstName" type="text" />
    </div>
    <div class="item">
        <label for="lastName">Last Name: <span class="required">*</span></label>
        <input id="lastName" name="lastName" type="text" />
    </div>
    <div class="item">
        <label for="dateOfBirth">Date of Birth: <span class="required">*</span></label>
        <input id="dateOfBirth" name="dateOfBirth" type="text" />
    </div>
    <div class="item">
        <label for="address1">Address 1: <span class="required">*</span></label>
        <input id="address1" name="address1" type="text" />
    </div>
    <div class="item">
        <label for="address2">Address 2: </label>
        <input id="address2" name="address2" type="text" />
    </div>
    <div class="item">
        <label for="city">City: <span class="required">*</span></label>
        <input id="city" name="city" type="text" />
    </div>
    <div class="item">
        <label for="state">State/Province: <span class="required">*</span></label>
        <select id="state" name="state">
            <option value="">-- States --</option>
            <option value="AL">Alabama</option>
            <option value="AK">Alaska</option>
            <option value="AZ">Arizona</option>
            <option value="AR">Arkansas</option>
            <option value="CA">California</option>
            <option value="CO">Colorado</option>
            <option value="CT">Connecticut</option>
            <option value="DE">Delaware</option>
            <option value="FL">Florida</option>
            <option value="GA">Georgia</option>
            <option value="HI">Hawaii</option>
            <option value="ID">Idaho</option>
            <option value="IL">Illinois</option>
            <option value="IN">Indiana</option>
            <option value="IA">Iowa</option>
            <option value="KS">Kansas</option>
            <option value="KY">Kentucky</option>
            <option value="LA">Louisiana</option>
            <option value="ME">Maine</option>
            <option value="MD">Maryland</option>
            <option value="MA">Massachusetts</option>
            <option value="MI">Michigan</option>
            <option value="MN">Minnesota</option>
            <option value="MS">Mississippi</option>
            <option value="MO">Missouri</option>
            <option value="MT">Montana</option>
            <option value="NE">Nebraska</option>
            <option value="NV">Nevada</option>
            <option value="NH">New Hampshire</option>
            <option value="NJ">New Jersey</option>
            <option value="NM">New Mexico</option>
            <option value="NY">New York</option>
            <option value="NC">North Carolina</option>
            <option value="ND">North Dakota</option>
            <option value="OH">Ohio</option>
            <option value="OK">Oklahoma</option>
            <option value="OR">Oregon</option>
            <option value="PA">Pennsylvania</option>
            <option value="RI">Rhode Island</option>
            <option value="SC">South Carolina</option>
            <option value="SD">South Dakota</option>
            <option value="TN">Tennessee</option>
            <option value="TX">Texas</option>
            <option value="UT">Utah</option>
            <option value="VT">Vermont</option>
            <option value="VA">Virginia</option>
            <option value="WA">Washington</option>
            <option value="WV">West Virginia</option>
            <option value="WI">Wisconsin</option>
            <option value="WY">Wyoming</option>
            <option value=""></option>
            <option value="">-- Provinces --</option>
            <option value="AB">Alberta</option>
            <option value="BC">British Columbia</option>
            <option value="LB">Labrador</option>
            <option value="MB">Manitoba</option>
            <option value="NB">New Brunswick</option>
            <option value="NF">Newfoundland</option>
            <option value="NS">Nova Scotia</option>
            <option value="NU">Nunavut</option>
            <option value="NW">Northwest Territories</option>
            <option value="ON">Ontario</option>
            <option value="PE">Prince Edward Island</option>
            <option value="QC">Quebec</option>
            <option value="SK">Saskatchewan</option>
            <option value="YU">Yukon</option>
        </select>
    </div>
    <div class="item">
        <label for="zip">Zip Code: <span class="required">*</span></label>
        <input id="zip" name="zip" type="text" />
    </div>
    <div class="item">
        <label for="homePhone">Home Phone: </label>
        <input id="homePhone" name="homePhone" type="text" />
    </div>
    <div class="item">
        <label for="cellPhone">Cell Phone: </label>
        <input id="cellPhone" name="cellPhone" type="text" />
    </div>
    <div class="item">
        <label for="email">Email: <span class="required">*</span></label>
        <input id="email" name="email" type="email" />
    </div>
    <div class="footer">
        <div id="prevRegistrationInfo" class="previous"><img src="/DesktopModules/RagnarokRegistration/images/previous.png" alt="Previous" /></div>
        <div id="nextCharacterInfo" class="next"><img src="/DesktopModules/RagnarokRegistration/images/next.png" alt="Next" /></div>
    </div>
</div>
<div id="characterInfo">
    <h1>Ragnarok XXXII Pre-Registration</h1>
    <h2>Character Information</h2>
    <p>Please provide some information about your (or your minor's) in-game character.</p>
    <span class="required">* indicates required fields</span><br />
    <div class="item">
        <label for="characterName">Character Name: <span class="required">*</span></label>
        <input id="characterName" name="characterName" type="text" />
    </div>
    <div class="item">
        <label for="chapterName">Chapter Name:</label>
        <input id="chapterName" name="chapterName" type="text" />
    </div>
    <div class="item">
        <label for="unitName" id="lblUnitName">Unit Name:</label>
        <input id="unitName" name="unitName" type="text" />
    </div>
    <h2>Camp Information</h2>
    <p>If your camp is not listed, please have your camp master contact <a href="mailto:troll@dagorhirragnarok.com">troll@dagorhirragnarok.com</a> to register your camp. If you 
        wish to register without selecting your camp, you can select "Camp Unknown - Will Select Later". You will be required to select your camp when you check in at Troll.</p>
    <div class="item">
        <label for="camp">Camp: <span class="required">*</span></label>
        <select id="camp" name="camp"></select>
    </div>
    <div class="footer">
        <div id="prevParticipantInfo" class="previous"><img src="/DesktopModules/RagnarokRegistration/images/previous.png" alt="Previous" /></div>
        <div id="nextEmergencyInfo" class="next"><img src="/DesktopModules/RagnarokRegistration/images/next.png" alt="Next" /></div>
    </div>
</div>
<div id="emergencyInfo">
    <h1>Ragnarok XXXII Pre-Registration</h1>
    <h2>Emergency Contact Information</h2>
    <p>Please provide a person who should be contacted in the case of emergency.</p>
    <span class="required">* indicates required fields</span><br />
    <div class="item">
        <label for="emergencyContactName">Emergency Contact Name: <span class="required">*</span></label>
        <input id="emergencyContactName" name="emergencyContactName" type="text" />
    </div>
    <div class="item">
        <label for="emergencyContactPhone">Emergency Contact Phone: <span class="required">*</span></label>
        <input id="emergencyContactPhone" name="emergencyContactPhone" type="text" />
    </div>
    <h2>Health Issues</h2>
    <p>Any health issues indicated here will be kept confidential and will only be used if aid is administered.</p>
    <div id="healthIssues">
    </div>
    <div id="btnHealthIssue"><img src="/DesktopModules/RagnarokRegistration/images/healthissue.png" alt="Add Health Issue" /></div>
    <div class="footer">
        <div id="prevCharacterInfo" class="previous"><img src="/DesktopModules/RagnarokRegistration/images/previous.png" alt="Previous" /></div>
        <div id="nextWaiverInfo" class="next"><img src="/DesktopModules/RagnarokRegistration/images/next.png" alt="Next" /></div>
    </div>
</div>
<div id="waiverInfo">
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
    <p>I understand that by registering for Ragnarok, I accept the above release. <input id="acceptRelease" name="acceptRelease" type="checkbox" /></p>
    <p>I acknowledge that I am over 18 years and of sound mind. <input id="acceptOver18" name="acceptOver18" type="checkbox" /></p>
    <p>I acknowledge that I have no known physical or mental defects that would increase the likelihood of serious injury if I intend to participate in Ragnarok activities. 
    <input id="acceptActivities" name="acceptActivities" type="checkbox" /></p>
    <h2>Sign Here</h2>
    <div id="signature-pad" class="m-signature-pad">
        <div class="m-signature-pad--body">
            <canvas id="signaturePad" width="658" height="318"></canvas>
        </div>
        <div class="m-signature-pad--footer">
            <div id="clearSignature">
                <img src="/DesktopModules/RagnarokRegistration/images/clearsignature.png" alt="Clear" />
            </div>
        </div>
    </div>
    <div class="footer">
        <div id="prevEmergencyInfo" class="previous"><img src="/DesktopModules/RagnarokRegistration/images/previous.png" alt="Previous" /></div>
        <!--<div id="nextPaymentInfo" class="next"><img src="/DesktopModules/RagnarokRegistration/images/next.png" alt="Next" /></div>-->
        <div id="register" class="next"><img src="/DesktopModules/RagnarokRegistration/images/register.png" alt="Register for Ragnarok" /></div>
    </div>
</div>
<div id="paymentInfo">
    <h1>Ragnarok XXXII Pre-Registration</h1>
    <h2>Payment Information</h2>
    <p>Please provide your payment information. Payment is handled through a secure connection to a third party merchant portal, and no payment information is stored by us.</p>
    <span class="required">* indicates required fields</span><br />
    <div class="item">
        <label for="creditCardNumber">Credit Card Number: <span class="required">*</span></label>
        <input id="creditCardNumber" name="creditCardNumber" type="text" />
    </div>
    <div class="item">
        <label for="creditCardExpirationMonth">Expiration Month: <span class="required">*</span></label>
        <select id="creditCardExpirationMonth" name="creditCardExpirationMonth">
            <option value="1">January</option>
            <option value="2">Februray</option>
            <option value="3">March</option>
            <option value="4">April</option>
            <option value="5">May</option>
            <option value="6">June</option>
            <option value="7">July</option>
            <option value="8">August</option>
            <option value="9">September</option>
            <option value="10">October</option>
            <option value="11">November</option>
            <option value="12">December</option>
        </select>
    </div>
    <div class="item">
        <label for="creditCardExpirationYear">Expiration Year: <span class="required">*</span></label>
        <select id="creditCardExpirationYear" name="creditCardExpirationYear">
            <option value="2017">2017</option>
            <option value="2018">2018</option>
            <option value="2019">2019</option>
            <option value="2020">2020</option>
            <option value="2021">2021</option>
            <option value="2022">2022</option>
            <option value="2023">2023</option>
            <option value="2024">2024</option>
            <option value="2025">2025</option>
            <option value="2026">2026</option>
            <option value="2027">2027</option>
            <option value="2028">2028</option>
            <option value="2029">2029</option>
            <option value="2030">2030</option>
            <option value="2031">2031</option>
            <option value="2032">2032</option>
            <option value="2033">2033</option>
            <option value="2034">2034</option>
            <option value="2035">2035</option>
            <option value="2036">2036</option>
        </select>
    </div>
    <label id="cardError" class="error"></label>
    <div class="footer">
        <div id="prevWaiverInfo" class="previous"><img src="/DesktopModules/RagnarokRegistration/images/previous.png" alt="Previous" /></div>
        
    </div>
</div>
<div id="confirmation">
    <h1>Thank you for registering for Ragnarok</h1>
    <p>Your registration has been successfully submitted.  You will receive an email with your registration confirmation and a QR code which you can bring 
    with you to check in at Troll for expedited event check in.</p>
    <p>We look forward to seeing you at Ragnarok this year!</p>
</div>
<div id="error">
    <h1>An error occurred with your registration</h1>
    <p>We're sorry, but an error occurred with your registration!  Please contact Troll at <a href="mailto:troll@dagorhirragnarok.com">troll@dagorhirragnarok.com</a> 
    and we'll resolve the issue as quickly as possible for you!</p>
    <p>We look forward to seeing you at Ragnarok this year!</p>
</div>

<script type="text/javascript" src="/DesktopModules/RagnarokRegistration/js/signature_pad.min.js"></script>
<script type="text/javascript" src="/DesktopModules/RagnarokRegistration/js/signature.js"></script>
<script type="text/javascript" src="/DesktopModules/RagnarokRegistration/js/jquery.validate.min.js"></script>
<script type="text/javascript" src="/DesktopModules/RagnarokRegistration/js/date.js"></script>
<script type="text/javascript" src="/DesktopModules/RagnarokRegistration/js/phoneUS.js"></script>
<script type="text/javascript" src="/DesktopModules/RagnarokRegistration/js/validation.js?v=0.0.27"></script>
<script src="https://cdn.jsdelivr.net/jquery.validation/1.15.0/additional-methods.min.js"></script>
<script type="text/javascript" src="https://js.authorize.net/v1/Accept.js" charset="utf-8"></script>
<!--script type="text/javascript" src="https://jstest.authorize.net/v1/Accept.js" charset="utf-8"></script-->

<script type="text/javascript">
    var issue = 0;
    var sf = $.ServicesFramework(<%:ModuleContext.ModuleId%>);

    $.getJSON(
        "/DesktopModules/RagnarokRegistration/API/ModuleCamp/GetCamps",
        function (result) {
            var parsedCampJSONObject = jQuery.parseJSON(result);
            $('#camp').append('<option value="">-- Select a Camp --</option>');
            $('#camp').append('<option value="0">Camp Unknown - Will Select Later</option>');
            $('#camp').append('<option value="">-------------------</option>');
            $.each(parsedCampJSONObject, function () {
                $('#camp').append('<option value="' + this.Id + '">' + this.CampName + ' - ' + this.CampMaster.FirstName + ' ' + this.CampMaster.LastName + '</option>');
            });
        }
    );

    $('#btnHealthIssue').click(function () {
        issue = issue + 1;

        $('#healthIssues').append('<div class="item healthIssue"><label for="healthIssue_' + issue + '">Health Issue: </label><div class="removeIssue"><img src="/DesktopModules/RagnarokRegistration/images/removeissue.png" alt="Remove Issue" /></div><textarea class="healthIssueText" id="healthIssue_' + issue + '" name="healthIssue_' + issue + '" /></div>');
    });

    $('#healthIssues').on("click", "div.removeIssue", function () {
        $(this).parent().remove();
    });
</script>