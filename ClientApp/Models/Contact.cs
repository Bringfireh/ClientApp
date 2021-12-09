namespace ClientApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Contact")]
    public partial class Contact
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Contact()
        {
            ClientContacts = new HashSet<ClientContact>();
        }

        [Key]
        public string Code { get; set; }

        [StringLength(128)]
        public string Surname { get; set; }

        [StringLength(128)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [StringLength(128)]
        [Display(Name = "Other Names")]
        public string OtherNames { get; set; }

        
        [Required]
        [Display(Name = "Email Address")]
        [StringLength(128)]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [StringLength(128)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientContact> ClientContacts { get; set; }
    }
}
