using Datiss.Budget.Entities.Identity;

namespace Datiss.Budget.ViewModels.Identity.Emails
{
    public class RegisterEmailConfirmationViewModel : EmailsBase
    {
        public User User { set; get; }
        public string EmailConfirmationToken { set; get; }
    }
}
