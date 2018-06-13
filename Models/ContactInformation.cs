using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ragnarok.Modules.RagnarokRegistration.Models
{
    [Serializable]
    public class ContactInformation
    {
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }

        #region Constructors

        public ContactInformation()
        {

        }

        public ContactInformation(SqlDataReader reader)
        {
            SetObjectData(reader);
        }

        #endregion

        #region Private Methods

        private void SetObjectData(SqlDataReader reader)
        {
            try
            {
                HomePhone = reader["ci_HomePhone"].ToString();
                CellPhone = reader["ci_CellPhone"].ToString();
                Email = reader["ci_Email"].ToString();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}