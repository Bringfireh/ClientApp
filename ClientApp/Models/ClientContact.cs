namespace ClientApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ClientContact")]
    public partial class ClientContact
    {
        [Key]
        public string Code { get; set; }

        [StringLength(128)]
        public string ClientCode { get; set; }

        [StringLength(128)]
        public string ContactCode { get; set; }

        public virtual Client Client { get; set; }

        public virtual Contact Contact { get; set; }
    }
}
