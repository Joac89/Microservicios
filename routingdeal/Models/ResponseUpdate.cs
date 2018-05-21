using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace routingdeal.Models
{
    [DataContract]
    [Serializable]
    public class UpdateDeal
    {
        [DataMember(Name = "invoicekey")]
        [Description("")]
        public string InvoiceKey { get; set; }

        [DataMember(Name = "state")]
        [Description("")]
        public bool State { get; set; }
    }

    [DataContract]
    [Serializable]
    public class ResponseUpdate : ResponseBase
    {
        [DataMember(Name = "date")]
        public UpdateDeal Data { get; set; }
    }
}