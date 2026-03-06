using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Common.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() : 
            base("You are not authorized to perform this action.") {}
        public UnauthorizedException(string message) : base(message) { }
    }
}
