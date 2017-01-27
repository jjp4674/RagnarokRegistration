using DotNetNuke.Common.Utilities;
using DotNetNuke.Security;
using DotNetNuke.Web.Api;
using Ragnarok.Modules.RagnarokRegistration.Controllers;
using Ragnarok.Modules.RagnarokRegistration.Models;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Ragnarok.Modules.RagnarokRegistration.Services
{
    public class ModuleCampController : DnnApiController
    {
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage GetCamps()
        {
            try
            {
                var camps = new CampController().GetCamps().ToJson();
                return Request.CreateResponse(HttpStatusCode.OK, camps);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
            }
        }

        [DnnModuleAuthorize(AccessLevel = SecurityAccessLevel.View)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public HttpResponseMessage AddRegistration(RegistrationDTO DTO)
        {
            try
            {
                var payment = new Payment()
                {
                    Amount = DTO.R_Payment.P_Amount, 
                    DataDescriptor = DTO.R_Payment.P_DataDescriptor, 
                    DataValue = DTO.R_Payment.P_DataValue
                };

                var address = new Address()
                {
                    Address1 = DTO.R_Address.A_Address1,
                    Address2 = DTO.R_Address.A_Address2,
                    City = DTO.R_Address.A_City,
                    State = DTO.R_Address.A_State,
                    Zip = DTO.R_Address.A_Zip
                };

                var contactInfo = new ContactInformation()
                {
                    CellPhone = DTO.R_ContactInfo.CI_CellPhone, 
                    HomePhone = DTO.R_ContactInfo.CI_HomePhone, 
                    Email = DTO.R_ContactInfo.CI_Email
                };

                var emergencyContactInfo = new EmergencyContact()
                {
                    Name = DTO.R_EmergencyContactInfo.EC_Name,
                    Phone = DTO.R_EmergencyContactInfo.EC_Phone
                };

                var participant = new Participant()
                {
                    CampId = DTO.R_CampId,
                    FirstName = DTO.R_FirstName,
                    LastName = DTO.R_LastName,
                    DateOfBirth = DTO.R_DateOfBirth,
                    CharacterName = DTO.R_CharacterName,
                    ChapterName = DTO.R_ChapterName,
                    UnitName = DTO.R_UnitName, 
                    Payment = payment, 
                    Address = address, 
                    ContactInformation = contactInfo, 
                    EmergencyContact = emergencyContactInfo, 
                    Signature = DTO.R_Signature
                };

                var registration = new Registration()
                {
                    Participant = participant
                };

                CampController cc = new CampController();
                cc.AddRegistration(registration);

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                CampController cc = new CampController();
                cc.AddError(0, e.Message);

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e.Message, e);
            }
        }
    }
}