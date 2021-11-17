using Datiss.Budget.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Models
{
    public class CreateWWsFeeDTO
    {
        public int YearId { get; set; }
        public int OrganizationId { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeTitle { get; set; }
        public int UsageLayerId { get; set; }
        public ActivityType ActivityType { get; set; }
        public int P1Fee { get; set; }
        public int P2Fee { get; set; }
        public int P1Note3 { get; set; }
        public int P2Note3 { get; set; }
        public int P1Note7 { get; set; }
        public int P2Note7 { get; set; }
    }

    public class UpdateWWsFeeDTO : CreateWWsFeeDTO
    {
        public int Id { get; set; }
    }

    public class WWsFeeDTO
    {
        public int Id { get; set; }
        public int YearId { get; set; }
        public int Year { get; set; }
        public int OrganizationId { get; set; }
        public string OrganizationDisplay { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeDisplay { get; set; }
        public int UsageLayerId { get; set; }
        public string UsageLayerDisplay { get; set; }
        public ActivityType ActivityType { get; set; }
        public int P1Fee { get; set; }
        public int P2Fee { get; set; }
        public int P1Note3 { get; set; }
        public int P2Note3 { get; set; }
        public int P1Note7 { get; set; }
        public int P2Note7 { get; set; }
    }
}
