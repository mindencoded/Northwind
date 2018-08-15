using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Northwind.WebRole.Dtos
{
    [DataContract]
    public class PersonDto : SerializableDynamicObject
    {
        [DataMember] public int Id { get; set; }

        [DataMember]
        [StringLength(50)]
        [Required]
        public string Company { get; set; }

        [DataMember]
        [StringLength(50)]
        [Required]
        public string LastName { get; set; }

        [DataMember]
        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }

        [DataMember]
        [StringLength(50)]
        [Required]
        public string EmailAddress { get; set; }

        [DataMember] [StringLength(50)] public string JobTitle { get; set; }

        [DataMember] [StringLength(25)] public string BusinessPhone { get; set; }

        [DataMember] [StringLength(25)] public string HomePhone { get; set; }

        [DataMember] [StringLength(25)] public string MobilePhone { get; set; }

        [DataMember] [StringLength(25)] public string FaxNumber { get; set; }

        [DataMember] [StringLength(250)] public string Address { get; set; }

        [DataMember] [StringLength(50)] public string City { get; set; }

        [DataMember] [StringLength(50)] public string StateProvince { get; set; }

        [DataMember] [StringLength(15)] public string ZipPostalCode { get; set; }

        [DataMember] [StringLength(50)] public string CountryRegion { get; set; }

        [DataMember] [StringLength(250)] public string WebPage { get; set; }

        [DataMember] [StringLength(250)] public string Notes { get; set; }

        [DataMember] public byte[] Attachments { get; set; }
    }
}