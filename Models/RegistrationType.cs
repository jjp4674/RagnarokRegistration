using System;

namespace Ragnarok.Modules.RagnarokRegistration.Models
{
    [Serializable]
    public class RegistrationType
    {
        public string Text { get; set; }
        public string Day { get; set; }
        public DateTime ArrivalDate { get; set; }
        public bool IsMinor { get; set; }
        public bool IsMerchant { get; set; }
        public double Cost { get; set; }

        public RegistrationType()
        {

        }
    }
}