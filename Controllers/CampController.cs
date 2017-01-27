using DotNetNuke.Common.Utilities;
using Ragnarok.Modules.RagnarokRegistration.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AuthorizeNet;
using AuthorizeNet.Api;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using AuthorizeNet.Api.Controllers;

namespace Ragnarok.Modules.RagnarokRegistration.Controllers
{
    public class CampController
    {
        private const string PROCEDURE_GET_CAMPS = "GetCamps";
        private const string PROCEDURE_ADD_REGISTRATION = "AddRegistration";
        private const string PROCEDURE_ADD_ERROR = "AddError";

        /// <summary>
        /// Gets the list of camps for the registration page.
        /// </summary>
        /// <returns>A list of camps.</returns>
        public IList<Camp> GetCamps()
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = PROCEDURE_GET_CAMPS;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (!reader.HasRows)
                    {
                        // throw exception
                    }

                    var camps = new List<Camp>();

                    while (reader.Read())
                    {
                        Camp camp = new Camp();
                        camp.Id = Convert.ToInt32(reader["Id"].ToString());
                        camp.CampName = reader["CampName"].ToString();
                        camp.CampLocation = reader["CampLocation"].ToString();

                        CampMaster campMaster = new CampMaster();
                        campMaster.FirstName = reader["FirstName"].ToString();
                        campMaster.LastName = reader["LastName"].ToString();

                        camp.CampMaster = campMaster;

                        camps.Add(camp);
                    }

                    return camps;
                }
            }
        }

        public void AddRegistration(Registration registration)
        {
            var response = HandlePayment(registration.Participant.Payment);
            int participantId = 0;

            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = PROCEDURE_ADD_REGISTRATION;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@par_c_id", registration.Participant.CampId);
                cmd.Parameters.AddWithValue("@par_FirstName", registration.Participant.FirstName);
                cmd.Parameters.AddWithValue("@par_LastName", registration.Participant.LastName);
                cmd.Parameters.AddWithValue("@par_DOB", registration.Participant.DateOfBirth);
                cmd.Parameters.AddWithValue("@par_CharacterName", registration.Participant.CharacterName);
                cmd.Parameters.AddWithValue("@par_ChapterName", registration.Participant.ChapterName);
                cmd.Parameters.AddWithValue("@par_UnitName", registration.Participant.UnitName);

                // Payment
                if (response.transactionResponse != null)
                {
                    cmd.Parameters.AddWithValue("@pm_AuthCode", response.transactionResponse.authCode);
                    cmd.Parameters.AddWithValue("@pm_TransId", response.transactionResponse.transId);
                    cmd.Parameters.AddWithValue("@pm_Amount", registration.Participant.Payment.Amount);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@pm_AuthCode", "");
                    cmd.Parameters.AddWithValue("@pm_TransId", "");
                    cmd.Parameters.AddWithValue("@pm_Amount", registration.Participant.Payment.Amount);
                }

                // Address
                cmd.Parameters.AddWithValue("@add_Address1", registration.Participant.Address.Address1);
                cmd.Parameters.AddWithValue("@add_Address2", registration.Participant.Address.Address2);
                cmd.Parameters.AddWithValue("@add_City", registration.Participant.Address.City);
                cmd.Parameters.AddWithValue("@add_State", registration.Participant.Address.State);
                cmd.Parameters.AddWithValue("@add_Zip", registration.Participant.Address.Zip);

                // Contact Information
                cmd.Parameters.AddWithValue("@ci_HomePhone", registration.Participant.ContactInformation.HomePhone);
                cmd.Parameters.AddWithValue("@ci_CellPhone", registration.Participant.ContactInformation.CellPhone);
                cmd.Parameters.AddWithValue("@ci_Email", registration.Participant.ContactInformation.Email);

                // Emergency Contact Information
                cmd.Parameters.AddWithValue("@ec_Name", registration.Participant.EmergencyContact.Name);
                cmd.Parameters.AddWithValue("@ec_Phone", registration.Participant.EmergencyContact.Phone);

                // Signature
                cmd.Parameters.AddWithValue("@s_Signature", registration.Participant.Signature);

                conn.Open();

                participantId = (int)cmd.ExecuteScalar();
            }
        }

        public void AddError(int participantId, string errorMessage)
        {
            using (SqlConnection conn = new SqlConnection(Config.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = PROCEDURE_ADD_ERROR;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Connection = conn;
                cmd.Parameters.AddWithValue("@err_par_id", participantId);
                cmd.Parameters.AddWithValue("@err_ErrorMessage", errorMessage);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private createTransactionResponse HandlePayment(Payment payment)
        {
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType
            {
                name = "3QeAr3WX7z",
                ItemElementName = ItemChoiceType.transactionKey,
                Item = "9f896x36Uy242FQf"
            };

            var opaqueData = new opaqueDataType
            {
                dataDescriptor = payment.DataDescriptor, 
                dataValue = payment.DataValue
            };

            var paymentType = new paymentType { Item = opaqueData };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(), 
                amount = payment.Amount, 
                payment = paymentType
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest };

            var controller = new createTransactionController(request);
            controller.Execute();

            var response = controller.GetApiResponse();

            if (response.messages.resultCode == messageTypeEnum.Ok)
            {
                return response;
            }
            else
            {
                throw new Exception("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
            }
        }
    }
}