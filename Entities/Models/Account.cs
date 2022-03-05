using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    [Table("account")]
    public class Account
    {
        //[Column("AccountIdNew")]
        public Guid AccountId { get; set; }
        [Required(ErrorMessage = "Date created is required")]
        public DateTime DateCreated { get; set; }
        [Required(ErrorMessage = "Account type is required")]
        public string AccountType { get; set; }
        [ForeignKey(nameof(Owner))]
        public Guid OwnerId { get; set; }
        public Owner Owner { get; set; }
    }
}

//In the Owner class, we have the Accounts property which suggests that one Owner is related to multiple Accounts.
//Additionally, we add the OwnerId and the Owner properties decorated with
//the [ForeignKey] attribute to state that one Account is related to only one Owner.
