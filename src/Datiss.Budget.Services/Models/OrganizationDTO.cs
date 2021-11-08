using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Models
{
    public class CreateOrganizationDTO
    {
        public int? ParentId { get; set; }

        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public OrganizationType Type { get; set; }

        public bool Enabled { get; set; }

        public bool SewageStatus { get; set; }
    }

    public class UpdateOrganizationDTO : CreateOrganizationDTO
    {
        public int Id { get; set; }
    }

    public class OrganizationDTO
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Title { get; set; }

        public int DisplayOrder { get; set; }

        public OrganizationType Type { get; set; }

        public bool SewageStatus { get; set; }

        public EntityStatus Status { get; set; }
    }

}
