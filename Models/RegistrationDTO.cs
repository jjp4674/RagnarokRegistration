using System;

namespace Ragnarok.Modules.RagnarokRegistration.Models
{
    public class RegistrationDTO
    {
        public int R_CampId { get; set; }
        public string R_FirstName { get; set; }
        public string R_LastName { get; set; }
        public DateTime R_DateOfBirth { get; set; }
        public string R_CharacterName { get; set; }
        public string R_ChapterName { get; set; }
        public string R_UnitName { get; set; }
        public PaymentDTO R_Payment { get; set; }
        public AddressDTO R_Address { get; set; }
        public ContactInformationDTO R_ContactInfo { get; set; }
        public EmergencyContactDTO R_EmergencyContactInfo { get; set; }
        public string R_Signature { get; set; }
    }

    public class PaymentDTO
    {
        public decimal P_Amount;
        public string P_DataDescriptor;
        public string P_DataValue;
    }

    public class AddressDTO
    {
        public string A_Address1 { get; set; }
        public string A_Address2 { get; set; }
        public string A_City { get; set; }
        public string A_State { get; set; }
        public string A_Zip { get; set; }
    }

    public class ContactInformationDTO
    {
        public string CI_HomePhone { get; set; }
        public string CI_CellPhone { get; set; }
        public string CI_Email { get; set; }
    }

    public class EmergencyContactDTO
    {
        public string EC_Name { get; set; }
        public string EC_Phone { get; set; }
    }
}