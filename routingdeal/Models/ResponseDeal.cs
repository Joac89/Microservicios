using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace routingdeal.Models
{
    [DataContract]
    [Serializable]
    public class Deal
    {
        [DataMember(Name = "id")]
        [Description("")]
        public long Id { get; set; }

        [DataMember(Name = "name")]
        [Description("")]
        public string Name { get; set; }

        [DataMember(Name = "invoicekey")]
        [Description("")]
        public string InvoiceKey { get; set; }

        [DataMember(Name = "state")]
        [Description("")]
        public bool State { get; set; }

        [DataMember(Name = "url")]
        [Description("")]
        public string Url { get; set; }

        [DataMember(Name = "template")]
        [Description("")]
        public string Template { get; set; }

        [DataMember(Name = "type")]
        [Description("")]
        public string Type { get; set; }

        [DataMember(Name = "requesttemplate")]
        [Description("")]
        public string RequestTemplate { get; set; }

        [DataMember(Name = "numrequest")]
        [Description("")]
        public int NumRequest { get; set; }
    }

    [DataContract]
    [Serializable]
    public class ResponseDeal : ResponseBase
    {
        [DataMember(Name = "data")]
        public Deal Data { get; set; }
    }
}