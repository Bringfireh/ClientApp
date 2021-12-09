namespace ClientApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Client")]
    public partial class Client
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Client()
        {
            ClientContacts = new HashSet<ClientContact>();
        }

        [Key]
        public string Code { get; set; }

        [Required]
        public string Name { get; set; }

        [StringLength(128)]
        public string Website { get; set; }

        [StringLength(128)]
        public string Fax { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        [Display(Name = "Date Created")]
        public DateTime? DateCreated { get; set; }

        //public virtual Client Client1 { get; set; }

        //public virtual Client Client2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ClientContact> ClientContacts { get; set; }
    }
}
