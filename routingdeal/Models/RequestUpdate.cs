using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace routingdeal.Models
{
    [DataContract]
    [Serializable]
    public class RequestUpdate
    {
        [Required]
        [DataMember(Name = "invoicekey")]
        [Description("Clave de factura del convenio")]
        public string InvoiceKey { get; set; }

        [Required]
        [DataMember(Name = "state")]
        [Description("Nuevo estado del convenio")]
        public bool State { get; set; }

        [Required]
        [DataMember(Name = "url")]
        [Description("Url del convenio")]
        public string Url { get; set; }

        [Required]
        [DataMember(Name = "template")]
        [Description("Url de la plantilla XSLT")]
        public string Template { get; set; }
    }
}