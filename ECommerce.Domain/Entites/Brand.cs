using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entites
{
    public class Brand
    {
        public Guid BrandId { get; private set; }
        public string Name { get; private set; } = string.Empty; // Unique

        public Brand() { } // For EF Core

        public Brand(string name) 
        {
            if (string.IsNullOrWhiteSpace(name))
               throw new DomainExceptions("Brand name cannot be empty.");
            BrandId = Guid.NewGuid();
            Name = name;
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainExceptions("Brand name cannot be empty.");
            Name = newName;
        }

    }
}
