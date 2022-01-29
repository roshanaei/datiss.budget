using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Datiss.Budget.Common.Exceptions
{
    public class UserChangePasswordException : BaseAppException
    {

        public IEnumerable<IdentityError> ChangePasswordErrors { get; set; }

        public UserChangePasswordException(IEnumerable<IdentityError> errors) : base()
            => ChangePasswordErrors = errors;

        public UserChangePasswordException() : base() { }

        public UserChangePasswordException(string message) : base(message) { }

        public string MyErrors {
            get {
                string result = "";
                foreach (var err in ChangePasswordErrors) {
                    result += err.Description + " <br>";
                }
                return result;
            }
        }
    }
}
