using Contracts;
using Entities;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public class OwnerRepository : RepositoryBase<Owner>, IOwnerRepository
    {
        public OwnerRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public IEnumerable<Owner> GetAllOwners()
        {
            return FindAll()
                .OrderBy(owner => owner.Name)
                .ToList();
        }

        public Owner GetOwnerById(Guid ownerId)
        {
            return FindByCondition(owner => owner.OwnerId.Equals(ownerId))
            .FirstOrDefault();
        }

        public Owner GetOwnerWithDetails(Guid ownerId)
        {
            return FindByCondition(owner => owner.OwnerId.Equals(ownerId))
                .Include(ac => ac.Accounts)
                .FirstOrDefault();
        }

        public void CreateOwner(Owner owner)
        {
            Create(owner);
        }

        public void UpdateOwner(Owner owner)
        {
            Update(owner);
        }

        public void DeleteOwner(Owner owner)
        {
            Delete(owner);
        }

        
        //public IEnumerable<Owner> GetOwners(OwnerParameters ownerParameters)
        //{
        //    return FindAll() //FindAll method to return all the data from the db and then apply parameters on that result. 
        //                     //The bottom line is that both examples are correct depending on the total amount of data in the database.
        //                     //You just have to test it and choose which one is faster for your db. 
        //        .OrderBy(on => on.Name)
        //        .Skip((ownerParameters.PageNumber - 1) * ownerParameters.PageSize)
        //        .Take(ownerParameters.PageSize)
        //        .ToList();
        //}
    }
}