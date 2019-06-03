using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ragnarok.Modules.RagnarokRegistration.Models
{
    [Serializable]
    public class Attendee
    {
        private int _id;
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        private int? _tagNumber;
        public int? TagNumber
        {
            get
            {
                return _tagNumber;
            }
            set
            {
                _tagNumber = value;
            }
        }

        private int _campId;
        public int CampId
        {
            get
            {
                return _campId;
            }
            set
            {
                _campId = value;
            }
        }

        private string _campName;
        public string CampName
        {
            get
            {
                return _campName;
            }
            set
            {
                _campName = value;
            }
        }

        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
            }
        }

        private DateTime _dateOfBirth;
        public DateTime DateOfBirth
        {
            get
            {
                return _dateOfBirth;
            }
            set
            {
                _dateOfBirth = value;
            }
        }

        private string _characterName;
        public string CharacterName
        {
            get
            {
                return _characterName;
            }
            set
            {
                _characterName = value;
            }
        }

        private string _unitName;
        public string UnitName
        {
            get
            {
                return _unitName;
            }
            set
            {
                _unitName = value;
            }
        }

        private string _chapterName;
        public string ChapterName
        {
            get
            {
                return _chapterName;
            }
            set
            {
                _chapterName = value;
            }
        }

        private DateTime _eventYear;
        public DateTime EventYear
        {
            get
            {
                return _eventYear;
            }
            set
            {
                _eventYear = value;
            }
        }

        private DateTime _dateSigned;
        public DateTime DateSigned
        {
            get
            {
                return _dateSigned;
            }
            set
            {
                _dateSigned = value;
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }

        private bool _isMinor;
        public bool IsMinor
        {
            get
            {
                return _isMinor;
            }
            set
            {
                _isMinor = value;
            }
        }

        private int? _minorParentTagNumber;
        public int? MinorParentTagNumber
        {
            get
            {
                return _minorParentTagNumber;
            }
            set
            {
                _minorParentTagNumber = value;
            }
        }

        private bool _isMerchant;
        public bool IsMerchant
        {
            get
            {
                return _isMerchant;
            }
            set
            {
                _isMerchant = value;
            }
        }

        private string _healthIssues;
        public string HealthIssues
        {
            get
            {
                return _healthIssues;
            }
            set
            {
                _healthIssues = value;
            }
        }

        private DateTime? _registrationDate;
        public DateTime? RegistrationDate
        {
            get
            {
                return _registrationDate;
            }
            set
            {
                _registrationDate = value;
            }
        }

        private DateTime? _arrivalDate;
        public DateTime? ArrivalDate
        {
            get
            {
                return _arrivalDate;
            }
            set
            {
                _arrivalDate = value;
            }
        }

        private Address _address;
        public Address Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
            }
        }

        private ContactInformation _contactInformation;
        public ContactInformation ContactInformation
        {
            get
            {
                return _contactInformation;
            }
            set
            {
                _contactInformation = value;
            }
        }

        private EmergencyContact _emergencyContact;
        public EmergencyContact EmergencyContact
        {
            get
            {
                return _emergencyContact;
            }
            set
            {
                _emergencyContact = value;
            }
        }

        private Payment _payment;
        public Payment Payment
        {
            get
            {
                return _payment;
            }
            set
            {
                _payment = value;
            }
        }

        #region Constructors

        public Attendee()
        {

        }

        public Attendee(SqlDataReader reader)
        {
            SetObjectData(reader);
        }

        #endregion

        #region Private Methods

        private void SetObjectData(SqlDataReader reader)
        {
            try
            {
                _id = Convert.ToInt32(reader["par_id"].ToString());
                _tagNumber = reader["par_TagNumber"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["par_TagNumber"].ToString());
                _campId = Convert.ToInt32(reader["par_c_id"].ToString());
                _campName = reader["c_CampName"].ToString();
                _firstName = reader["par_FirstName"].ToString();
                _lastName = reader["par_LastName"].ToString();
                _dateOfBirth = Convert.ToDateTime(reader["par_DOB"].ToString());
                _characterName = reader["par_CharacterName"].ToString();
                _unitName = reader["par_UnitName"].ToString();
                _chapterName = reader["par_ChapterName"].ToString();
                _eventYear = Convert.ToDateTime(reader["par_EventYear"].ToString());
                _dateSigned = Convert.ToDateTime(reader["par_DateSigned"].ToString());
                _status = reader["par_Status"].ToString();
                _isMinor = reader["par_IsMinor"].ToString() == "Y" ? true : false;
                _minorParentTagNumber = reader["par_MinorParentTagNumber"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["par_MinorParentTagNumber"].ToString());
                _isMerchant = reader["par_IsMerchant"].ToString() == "Y" ? true : false;
                _registrationDate = reader["par_RegistrationDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["par_RegistrationDate"].ToString());
                _arrivalDate = reader["par_ArrivalDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["par_ArrivalDate"].ToString());
                _healthIssues = reader["hi_HealthIssue"] == DBNull.Value ? "" : reader["hi_HealthIssue"].ToString();
                _address = new Address(reader);
                _contactInformation = new ContactInformation(reader);
                _emergencyContact = new EmergencyContact(reader);
                _payment = new Payment(reader);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion


    }
}