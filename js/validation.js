$().ready(function () {
    var sigError = false;
    var isMinor = false;
    var isMerchant = false;
    var signatureData = "";

    $.validator.addMethod("zipcode", function (value, element) {
        return this.optional(element) || /^\d{5}(?:-\d{4})?$/.test(value);
    }, "Please provide a valid zipcode.");

    $.validator.addMethod("date", function (value, element) {
        return Date.parseExact(value, "MM/dd/yyyy");
    }, "Please provide a valid date (MM/DD/YYYY).");

    var validator = $('#Form').validate({
        rules: {
            registrationCost: "required", 
            firstName: "required",
            lastName: "required",
            dateOfBirth: {
                required: true,
                date: true
            },
            address1: "required",
            city: "required",
            state: "required",
            zip: {
                required: true,
                zipcode: true
            },
            homePhone: {
                required: false,
                phoneUS: true
            },
            cellPhone: {
                required: false,
                phoneUS: true
            },
            email: {
                required: true,
                email: true
            },
            characterName: "required",
            camp: "required",
            emergencyContactName: "required",
            emergencyContactPhone: {
                required: true,
                phoneUS: true
            },
            acceptRelease: "required",
            acceptOver18: "required",
            acceptActivities: "required",
            creditCardNumber: {
                required: true,
                creditcard: true
            }
        },
        messages: {
            registrationCost: "Registration type is required.", 
            firstName: "First name is required.",
            lastName: "Last name is required.",
            dateOfBirth: {
                required: "Date of birth is required.",
                date: "Date of birth must be a valid date (MM/DD/YYYY)."
            },
            address1: "Address 1 is required.",
            city: "City is required.",
            state: "State is required.",
            zip: {
                required: "Zip code is required.",
                zipcode: "Zip code must be a valid zip code."
            },
            homePhone: {
                phoneUS: "Home phone must be a valid phone number."
            },
            cellPhone: {
                phoneUS: "Cell phone must be a valid phone number."
            },
            email: {
                required: "Email is required.",
                email: "Email must be a valid email address."
            },
            characterName: "Character name is required.",
            camp: "Camp is required.",
            emergencyContactName: "Emergency contact name is required.",
            emergencyContactPhone: {
                required: "Emergency contact phone is required.",
                phoneUS: "Cell phone must be a valid phone number."
            },
            acceptRelease: "You must accept the waiver to pre-register.",
            acceptOver18: "You must be over 18 to pre-register.",
            acceptActivities: "You must accept to pre-register.",
            creditCardNumber: {
                required: "Credit card number is required.",
                creditcard: "Credit card number must be a valid card number."
            }
        }
    });

    $('#nextParticipantInfo').click(function () {
        var costText = $('#registrationCost option:selected').text();
        if (costText.includes("Child"))
        {
            isMinor = true;
        }
        else if (costText.includes("Merchant"))
        {
            isMerchant = true;
        }

        var ctValid = validator.element("#registrationCost");
        if (ctValid)
        {
            $('#registrationInfo').hide();
            $('#participantInfo').show();
        }
    });

    $('#nextCharacterInfo').click(function () {
        var fnValid = validator.element("#firstName");
        var lnValid = validator.element("#lastName");
        var dobValid = validator.element("#dateOfBirth");
        var a1Valid = validator.element("#address1");
        var cValid = validator.element("#city");
        var sValid = validator.element("#state");
        var zValid = validator.element("#zip");
        var hpValid = validator.element("#homePhone");
        var cpValid = validator.element("#cellPhone");
        var eValid = validator.element("#email");

        if (fnValid && lnValid && dobValid && a1Valid && cValid && sValid && zValid && hpValid && cpValid && eValid) {
            if (isMerchant)
            {
                $('#lblUnitName').html('Business Name: <span class="required">*</span>');
            }
            else
            {
                $('#lblUnitName').html('Unit Name:');
            }

            $('#participantInfo').hide();
            $('#characterInfo').show();
        }
    });

    $('#nextEmergencyInfo').click(function () {
        var cnValid = validator.element("#characterName");
        var cValid = validator.element("#camp");

        if (cnValid && cValid) {
            $('#characterInfo').hide();
            $('#emergencyInfo').show();
        }
    });

    $('#nextWaiverInfo').click(function () {
        var ecnValid = validator.element("#emergencyContactName");
        var ecpValid = validator.element("#emergencyContactPhone");

        if (ecnValid && ecpValid) {
            $('#emergencyInfo').hide();

            if (!isMinor) {
                $('#waiverInfo').show();
            }
            else {
                $('#paymentInfo').show();
            }
            $('#signaturePad').resize();

            if (signatureData != "")
            {
                signaturePad.fromDataURL(signatureData);
            }
        }
    });

    $('#nextPaymentInfo').click(function () {
        var arValid = validator.element("#acceptRelease");
        var ao18Valid = validator.element("#acceptOver18");
        var aaValid = validator.element("#acceptActivities");

        var sValid = !signaturePad.isEmpty();
        if (!sValid)
        {
            if (!sigError) {
                $(".m-signature-pad--body").append("<label class='error' id='sig-error'>Your signature is required.</label>");
                sigError = true;
            }
        }
        else
        {
            $('#sig-error').remove();
            sigError = false;
        }

        if (arValid && ao18Valid && aaValid && sValid) {
            signatureData = signaturePad.toDataURL();

            $('#waiverInfo').hide();
            $('#paymentInfo').show();
        }
    });

    $('#prevRegistrationInfo').click(function () {
        $('#participantInfo').hide();
        $('#registrationInfo').show();
    });

    $('#prevParticipantInfo').click(function () {
        $('#characterInfo').hide();
        $('#participantInfo').show();
    });

    $('#prevCharacterInfo').click(function () {
        $('#emergencyInfo').hide();
        $('#characterInfo').show();
    });

    $('#prevEmergencyInfo').click(function () {
        $('#waiverInfo').hide();
        $('#emergencyInfo').show();
    });

    $('#prevWaiverInfo').click(function () {
        $('#paymentInfo').hide();
        if (!isMinor) {
            if (signatureData != "") {
                signaturePad.fromDataURL(signatureData);
            }

            $('#waiverInfo').show();
        }
        else {
            $('#emergencyInfo').show();
        }
        $('#signaturePad').resize();
    });

    $('#register').click(function () {
        var ccnValid = validator.element("#creditCardNumber");

        if (ccnValid) {
            $('#overlay').show();

            var secureData = {}, authData = {}, cardData = {};

            cardData.cardNumber = $('#creditCardNumber').val();
            cardData.month = $('#creditCardExpirationMonth').val();
            cardData.year = $('#creditCardExpirationYear').val();
            secureData.cardData = cardData;

            authData.clientKey = '4sW764Nm2cXadGSbtj37rA5sM46uYXcW6Z6C678x5LNAKH9sJRu9ENwAu38erwgJ';
            authData.apiLoginID = '2B4f4zGR';
            secureData.authData = authData;

            Accept.dispatchData(secureData, 'responseHandler');
        }
    });
});

function responseHandler(response) {
    if (response.messages.resultCode === 'Error') {
        for (var i = 0; i < response.messages.message.length; i++) {
            $('#cardError').text(response.messages.message[i].text);
        }

        $('#overlay').hide();
    } else {
        useOpaqueData(response.opaqueData);
    }
}

function useOpaqueData(responseData) {
    submitRegistration(responseData);
}

function submitRegistration(dataObj) {
    var campId = $('#camp').val();
    var firstName = $('#firstName').val();
    var lastName = $('#lastName').val();
    var dob = $('#dateOfBirth').val();
    var address1 = $('#address1').val();
    var address2 = $('#address2').val();
    var city = $('#city').val();
    var state = $('#state').val();
    var zip = $('#zip').val();

    var characterName = $('#characterName').val();
    var chapterName = $('#chapterName').val();
    var unitName = $('#unitName').val();

    var homePhone = $('#homePhone').val();
    var cellPhone = $('#cellPhone').val();
    var email = $('#email').val();

    var emergencyName = $('#emergencyContactName').val();
    var emergencyPhone = $('#emergencyContactPhone').val();

    var cost = $('#registrationCost').val();
    var costText = $('#registrationCost option:selected').text();

    var signature = signaturePad.toDataURL();

    var payment = {
        P_Amount: cost,
        P_DataDescriptor: dataObj.dataDescriptor, 
        P_DataValue: dataObj.dataValue
    };

    var address = {
        A_Address1: address1,
        A_Address2: address2,
        A_City: city,
        A_State: state,
        A_Zip: zip
    };

    var contactInfo = {
        CI_HomePhone: homePhone,
        CI_CellPhone: cellPhone,
        CI_Email: email
    };

    var emergencyContactInfo = {
        EC_Name: emergencyName,
        EC_Phone: emergencyPhone
    };

    var healthIssue = new Array();

    $('.healthIssueText').each(function () {
        var issue = {
            HI_Issue: $(this).val()
        };
        healthIssue.push(issue);
    });

    var registration = {
        R_CampId: campId,
        R_FirstName: firstName,
        R_LastName: lastName,
        R_DateOfBirth: dob,
        R_CharacterName: characterName,
        R_ChapterName: chapterName,
        R_UnitName: unitName,
        R_Payment: payment, 
        R_Address: address,
        R_ContactInfo: contactInfo,
        R_EmergencyContactInfo: emergencyContactInfo,
        R_HealthIssues: healthIssue,
        R_Signature: signature,
        R_RegText: costText
    };

    $.ajax({
        url: "/DesktopModules/RagnarokRegistration/API/ModuleCamp/AddRegistration",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(registration),
        beforeSend: sf.setModuleHeaders
    }).done(function (data) {

        $('#paymentInfo').hide();
        $('#confirmation').show();

        $('#overlay').hide();

    }).fail(function (jqXHR, textStatus) {

        $('#paymentInfo').hide();
        $('#error').show();

        $('#overlay').hide();

    });
}