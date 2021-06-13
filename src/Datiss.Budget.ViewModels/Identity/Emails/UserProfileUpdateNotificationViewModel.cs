using Datiss.Budget.Entities.Identity;

namespace Datiss.Budget.ViewModels.Identity.Emails
{
    public class UserProfileUpdateNotificationViewModel : EmailsBase
    {
        public User User { set; get; }
    }
}