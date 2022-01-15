using System;
using Datiss.Budget.Enum;

namespace Datiss.Budget.Services.Models
{

    public class UserResultDTO
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int? PositionId { get; set; }
        public string PositionTitle { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? LastVisitDateTime { get; set; }
        public bool IsEmailPublic { get; set; }
        public string NationalCode { get; set; }
        public int? OrganizationId { get; set; }
        public string OrganizationTitle { get; set; }
        public EntityStatus Status { get; set; }
    }

    public class UserFilterDTO : FilterInputDTO
    {
        public string Username { get; set; }
        public string NationalCode { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public int? OrganizationId { get; set; }
        public int? PositionId { get; set; }
        public EntityStatus? Status { get; set; }
    }

    public class CreateUserDTO
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int? PositionId { get; set; }
        public string NationalCode { get; set; }
        public int? OrganizationId { get; set; }
    }

    public class UpdateUserDTO : CreateUserDTO
    {
        public int Id { get; set; }
        public EntityStatus Status { get; set; }
    }


}
