using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace userinterface.Models
{
    [DataContract]
    [Serializable]
    public class ResponseBase
    {
        [Required]
        [DefaultValue(0)]
        [DataMember]
        public int Code { get; set; }

        [DefaultValue("")]
        [DataMember]
        public string Message { get; set; } = "";
    }
}