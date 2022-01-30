using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Datiss.Budget.Common.Exceptions
{
    public class UpdateUserException : CreateUserException
    {

        public UpdateUserException(IEnumerable<IdentityError> errors)
            : base(errors) {  }


    }
}
