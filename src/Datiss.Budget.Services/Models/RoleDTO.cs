using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class RoleDTO
    {

        public RoleDTO() 
        {
            Claims = new List<RoleClaimDTO>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsConstantRole => Name.ToUpper() == "ADMIN";
        public int UsersCount { get; set; }
        public IEnumerable<RoleClaimDTO> Claims { get; set; }
    }   

    public class RoleClaimDTO 
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string RoleTitle { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

}
