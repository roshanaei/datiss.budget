using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Enum;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.ViewModels.Identity;
using Datiss.Budget.Common.IdentityToolkit;
using Datiss.Budget.Services.Contracts.Identity;
using DNTCommon.Web.Core;
using Mapster;

namespace Datiss.Budget.Web.Areas.Identity.Controllers
{

    [Authorize(Roles = ConstantRoles.Admin)]
    [Area(AreaConstants.IdentityArea)]
    [Route("[area]/[controller]")]
    public class UsersController : Controller
    {
        public const string Name = "Users";
        public const string ACTION_Index = nameof(Index);

        private string _indexFilterKey = $"{Name}_{ACTION_Index}";

        private readonly IUserService _userService;


        public UsersController(
            IUserService userService) {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("{page?}")]
        public async IActionResult Index(int page = 1) {
            var filter = new UserFilterDTO();

            var myfilter = TempData.Get<UserFilterViewModel>(_indexFilterKey);
            if(myfilter != null) {
                filter = myfilter.Adapt<UserFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _userService.GetListAsync(filter);
            var model = result.Adapt<UsersIndexViewModel>();


            return View();
        }
    }
}
