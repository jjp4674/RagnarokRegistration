using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ragnarok.Modules.RagnarokRegistration.Models
{
    public class Participant
    {
        public int Id { get; set; }
        public int CampId { get; set; }
        public int TagNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CharacterName { get; set; }
        public string ChapterName { get; set; }
        public string UnitName { get; set; }
        public DateTime EventYear { get; set; }
        public DateTime DateSigned { get; set; }
        public string Status { get; set; }
        public string Signature { get; set; }
        public string IsMinor { get; set; }
        public string IsMerchant { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public Payment Payment { get; set; }
        public Address Address { get; set; }
        public ContactInformation ContactInformation { get; set; }
        public EmergencyContact EmergencyContact { get; set; }
        public List<HealthIssue> HealthIssues { get; set; }
    }
}