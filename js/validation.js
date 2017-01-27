$().ready(function () {
    var sigError = false;

    $.validator.addMethod("zipcode", function (value, element) {
        return this.optional(element) || /^\d{5}(?:-\d{4})?$/.test(value);
    }, "Please provide a valid zipcode.");

    $.validator.addMethod("date", function (value, element) {
        return Date.parseExact(value, "MM/dd/yyyy");
    }, "Please provide a valid date (MM/DD/YYYY).");

    var validator = $('#Form').validate({
        rules: {
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
            chapterName: "required",
            unitName: "required",
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
            chapterName: "Chapter name is required.",
            unitName: "Unit name is required.",
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
            $('#participantInfo').hide();
            $('#characterInfo').show();
        }
    });

    $('#nextEmergencyInfo').click(function () {
        var cnValid = validator.element("#characterName");
        var chnValid = validator.element("#chapterName");
        var unValid = validator.element("#unitName");
        var cValid = validator.element("#camp");

        if (cnValid && chnValid && unValid && cValid) {
            $('#characterInfo').hide();
            $('#emergencyInfo').show();
        }
    });

    $('#nextWaiverInfo').click(function () {
        var ecnValid = validator.element("#emergencyContactName");
        var ecpValid = validator.element("#emergencyContactPhone");

        if (ecnValid && ecpValid) {
            $('#emergencyInfo').hide();
            $('#waiverInfo').show();
            $('#signaturePad').resize();
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
            $('#waiverInfo').hide();
            $('#paymentInfo').show();
        }
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
        $('#waiverInfo').show();
        $('#signaturePad').resize();
    });

    $('#register').click(function () {
        var ccnValid = validator.element("#creditCardNumber");

        if (ccnValid) {
            var secureData = {}, authData = {}, cardData = {};

            cardData.cardNumber = $('#creditCardNumber').val();
            cardData.month = $('#creditCardExpirationMonth').val();
            cardData.year = $('#creditCardExpirationYear').val();
            secureData.cardData = cardData;

            authData.clientKey = '96fmL4CBZfQdmxZ9Y92Ks8SbF9N399L4fZfEYsZ8JstwbG4U6k9fd7QG4mFyjgkx';
            authData.apiLoginID = '3QeAr3WX7z';
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
    } else {
        useOpaqueData(response.opaqueData);
    }
}

function useOpaqueData(responseData) {
    console.log(responseData.dataDescriptor);
    console.log(responseData.dataValue);
    submitRegistration(responseData);
}

function submitRegistration(dataObj) {
    var campId = $('#camp').val();
    alert(campId);
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

    var signature = signaturePad.toDataURL();

    var payment = {
        P_Amount: 12.00, 
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
        R_Signature: signature
    };

    $.ajax({
        url: "/DesktopModules/RagnarokRegistration/API/ModuleCamp/AddRegistration",
        type: "POST",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(registration),
        beforeSend: sf.setModuleHeaders
    }).done(function (data) {

        console.log('Success');

        $('#paymentInfo').hide();
        $('#confirmation').show();

    }).fail(function (jqXHR, textStatus) {

        console.log('Error: ' + jqXHR.responseText);

        $('#paymentInfo').hide();
        $('#error').show();

    });
}