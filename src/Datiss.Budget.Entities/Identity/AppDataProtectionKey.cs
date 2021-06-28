namespace Datiss.Budget.Entities.Identity
{
    public class AppDataProtectionKey
    {
        public AppDataProtectionKey() { }

        #region Properties

        public int Id { get; set; }

        public string FriendlyName { get; set; }

        public string XmlData { get; set; }

        #endregion
    }
}