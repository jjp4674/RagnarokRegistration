using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ragnarok.Modules.RagnarokRegistration.Models
{
    [Serializable]
    public class EmergencyContact
    {
        public string Name { get; set; }
        public string Phone { get; set; }

        #region Constructors

        public EmergencyContact()
        {

        }

        public EmergencyContact(SqlDataReader reader)
        {
            SetObjectData(reader);
        }

        #endregion

        #region Private Methods

        private void SetObjectData(SqlDataReader reader)
        {
            try
            {
                Name = reader["ec_Name"].ToString();
                Phone = reader["ec_Phone"].ToString();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}