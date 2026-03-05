using ECommerce.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Entites
{
    public class Category
    {
        public Guid CategoryId { get; private set; }
        public string CategoryName { get; private set; } = string.Empty; // Unique


        public Category() { } // For EF Core

        public Category(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new DomainExceptions("Category name cannot be empty.");
            CategoryId = Guid.NewGuid();
            CategoryName = categoryName;
        }

        public void UpdateCategoryName(string newCategoryName)
        {
            if (string.IsNullOrWhiteSpace(newCategoryName))
                throw new DomainExceptions("Category name cannot be empty.");
            CategoryName = newCategoryName;
        }
    }
}
