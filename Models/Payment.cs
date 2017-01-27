using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ragnarok.Modules.RagnarokRegistration.Models
{
    public class Payment
    {
        public decimal Amount { get; set; }
        public string DataDescriptor { get; set; }
        public string DataValue { get; set; }
    }
}