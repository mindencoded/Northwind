﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Northwind.WebRole.Dtos
{
    [DataContract]
    public class OrderStatusDto : SerializableDynamicObject
    {
        [DataMember] public byte Id { get; set; }

        [DataMember]
        [StringLength(50)]
        [Required(AllowEmptyStrings = false)]
        public string StatusName { get; set; }
    }
}