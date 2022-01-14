using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{

    public class UserResultDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public int? PositionId { get; set; }
        public DateTime? CreatedDateTime { get; set; }
        public DateTime? LastVisitDateTime { get; set; }
        public bool IsEmailPublic { get; set; }
        public string NationalCode { get; set; }
        public int? OrganizationId { get; set; }
        public EntityStatus Status { get; set; }
    }

    public class CreateUserDto
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

    public class UpdateUserDto : CreateUserDto
    {
        public int Id { get; set; }
    }
}
