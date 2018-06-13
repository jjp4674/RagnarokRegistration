using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ragnarok.Modules.RagnarokRegistration.Models
{
    [Serializable]
    public class Payment
    {
        public decimal Amount { get; set; }
        public string DataDescriptor { get; set; }
        public string DataValue { get; set; }

        #region Constructors

        public Payment()
        {

        }

        public Payment(SqlDataReader reader)
        {
            SetObjectData(reader);
        }

        #endregion

        #region Private Methods

        private void SetObjectData(SqlDataReader reader)
        {
            try
            {
                Amount = Convert.ToDecimal(reader["pm_Amount"].ToString());
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}