using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace routingdeal.Models
{
    [DataContract]
    [Serializable]
    public class RequestDeal
    {
        [DataMember(Name = "name")]
        [Description("")]
        public string Name { get; set; }

        [DataMember(Name = "invoicekey")]
        [Description("")]
        public string InvoiceKey { get; set; }

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
        /*[Required]
        [DataMember(Name = "invoicekey")]
        [Description("Clave de factura del convenio")]
        public string InvoiceKey { get; set; }

        [Required]
        [DataMember(Name = "name")]
        [Description("Nombre del convenio")]
        public string Name { get; set; }

        [Required]
        [DataMember(Name = "url")]
        [Description("Url del convenio")]
        public string Url { get; set; }

        [Required]
        [DataMember(Name = "template")]
        [Description("Url de la plantilla XSLT")]
        public string Template { get; set; }*/
    }
}