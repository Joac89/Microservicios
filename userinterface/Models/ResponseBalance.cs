using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace userinterface.Models
{
    [DataContract]
    [Serializable]
    public class Balance
    {
        [Required]
        [DataMember(Name = "invoiceref")]
        [Description("")]
        public string InvoiceRef { get; set; }

        [Required]
        [DataMember(Name = "balancetopay")]
        [Description("")]
        public double BalanceToPay { get; set; }
    }

    [DataContract]
    public class ResponseBalance : ResponseBase
    {
        [DataMember(Name = "data")]
        public Balance Data { get; set; }
    }
}
