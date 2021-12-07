//namespace ClientApp.Models
//{
//    using System;
//    using System.Data.Entity;
//    using System.ComponentModel.DataAnnotations.Schema;
//    using System.Linq;

//    public partial class MyModel : DbContext
//    {
//        public MyModel()
//            : base("name=MyModel")
//        {
//        }

//        public virtual DbSet<Client> Clients { get; set; }
//        public virtual DbSet<ClientContact> ClientContacts { get; set; }
//        public virtual DbSet<Contact> Contacts { get; set; }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<Client>()
//                .HasOptional(e => e.Client1)
//                .WithRequired(e => e.Client2);

//            modelBuilder.Entity<Client>()
//                .HasMany(e => e.ClientContacts)
//                .WithOptional(e => e.Client)
//                .WillCascadeOnDelete();

//            modelBuilder.Entity<Contact>()
//                .HasMany(e => e.ClientContacts)
//                .WithOptional(e => e.Contact)
//                .WillCascadeOnDelete();
//        }
//    }
//}
