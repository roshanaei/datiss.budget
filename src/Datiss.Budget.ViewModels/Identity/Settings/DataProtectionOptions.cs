using System;

namespace Datiss.Budget.ViewModels.Identity.Settings
{
    public class DataProtectionOptions
    {
        public TimeSpan DataProtectionKeyLifetime { get; set; }
        public string ApplicationName { get; set; }
    }
}