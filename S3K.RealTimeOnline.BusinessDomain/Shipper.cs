using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using S3K.RealTimeOnline.GenericDomain;

namespace S3K.RealTimeOnline.BusinessDomain
{
    [Table("SHIPPER")]
    public class Shipper : Entity
    {
        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [Column("COMPANY")]
        [StringLength(50)]
        public string Company { get; set; }

        [Column("LAST_NAME")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Column("FIRST_NAME")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Column("EMAIL_ADDRESS")]
        [StringLength(50)]
        public string EmailAddress { get; set; }

        [Column("JOB_TITLE")]
        [StringLength(50)]
        public string JobTitle { get; set; }

        [Column("BUSINESS_PHONE")]
        [StringLength(25)]
        public string BusinessPhone { get; set; }

        [Column("HOME_PHONE")]
        [StringLength(25)]
        public string HomePhone { get; set; }

        [Column("MOBILE_PHONE")]
        [StringLength(25)]
        public string MobilePhone { get; set; }

        [Column("FAX_NUMBER")]
        [StringLength(25)]
        public string FaxNumber { get; set; }

        [Column("ADDRESS")]
        [StringLength(250)]
        public string Address { get; set; }

        [Column("CITY")]
        [StringLength(50)]
        public string City { get; set; }

        [Column("STATE_PROVINCE")]
        [StringLength(50)]
        public string StateProvince { get; set; }

        [Column("ZIP_POSTAL_CODE")]
        [StringLength(15)]
        public string ZipPostalCode { get; set; }

        [Column("COUNTRY_REGION")]
        [StringLength(50)]
        public string CountryRegion { get; set; }

        [Column("WEB_PAGE")]
        [StringLength(250)]
        public string WebPage { get; set; }

        [Column("NOTES")]
        [StringLength(250)]
        public string Notes { get; set; }

        [Column("ATTACHMENTS")]
        public byte[] Attachments { get; set; }
    }
}
