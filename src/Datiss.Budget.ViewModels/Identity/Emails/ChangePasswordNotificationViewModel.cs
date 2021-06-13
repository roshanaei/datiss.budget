using Datiss.Budget.Entities.Identity;

namespace Datiss.Budget.ViewModels.Identity.Emails
{
    public class ChangePasswordNotificationViewModel : EmailsBase
    {
        public User User { set; get; }
    }
}