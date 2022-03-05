using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DataTransferObjects
{
    public class OwnerDto
    {
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
    }
}

//As you can see, we don’t have the Accounts property,
//because we don’t want to show that information to the client right now.

//Now, all we would have to do is to map a returned list of owners from the database to the list of ownerDto