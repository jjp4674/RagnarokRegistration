using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ragnarok.Modules.RagnarokRegistration.Models
{
    [Serializable]
    public class Address
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        #region Constructors

        public Address()
        {

        }

        public Address(SqlDataReader reader)
        {
            SetObjectData(reader);
        }

        #endregion

        #region Private Methods

        private void SetObjectData(SqlDataReader reader)
        {
            try
            {
                Address1 = reader["add_Address1"].ToString();
                Address2 = reader["add_Address2"].ToString();
                City = reader["add_City"].ToString();
                State = reader["add_State"].ToString();
                Zip = reader["add_Zip"].ToString();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}