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

        public string MyErrors {
            get {
                string result = "";
                foreach(var err in CreateErrors) {
                    result += err.Description + " <br>";
                }
                return result;
            }
        }

        public bool UsernameAlreadyExist { get; set; }

        public bool PasswordHasError { get; set; }
    }

}
