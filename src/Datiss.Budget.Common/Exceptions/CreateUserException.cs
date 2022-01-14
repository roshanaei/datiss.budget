using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Datiss.Budget.Common.Exceptions
{

    public class CreateUserException : BaseAppException
    {

        public CreateUserException(): base() { }

        public CreateUserException(bool userNameAlreadyExist)
            => UsernameAlreadyExist = userNameAlreadyExist;

        public CreateUserException(IEnumerable<IdentityError> errors)
            => CreateErrors = errors;

        public IEnumerable<IdentityError> CreateErrors { get; set; }

        public bool UsernameAlreadyExist { get; set; }

        public bool PasswordHasError { get; set; }
    }

}
