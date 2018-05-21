using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace userinterface.Models
{
    [DataContract]
    [Serializable]
    public class Payment
    {
        [DataMember(Name = "invoiceref")]
        [Description("")]
        public string InvoiceRef { get; set; }

        [DataMember(Name = "messagepayment")]
        [Description("")]
        public string MessagePayment { get; set; }
    }

    [DataContract]
    [Serializable]
    public class ResponsePayment : ResponseBase
    {
        [DataMember(Name = "data")]
        public Payment Data { get; set; }
    }
}