using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common.Exceptions
{
    public class ConflictException : Exception
    {
        public ConflictException(string entityName, string field ,object value) 
            : base($"{entityName} with {field} '{value}' already exists.") { }
        public ConflictException(string message) : base(message) { }
    }
}
