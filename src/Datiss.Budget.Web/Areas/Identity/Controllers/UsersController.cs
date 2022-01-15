using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Datiss.Budget.Enum;
using Datiss.Budget.Common;
using Datiss.Budget.ViewModels;
using Datiss.Budget.Services.Models;
using Datiss.Budget.Entities.Identity;
using Datiss.Budget.Services.Identity;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.ViewModels.Identity;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Common.IdentityToolkit;
using Datiss.Budget.Services.Contracts.Identity;
using DNTCommon.Web.Core;
using Mapster;
using Datiss.Budget.Resources;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Services.Infrastructure;

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
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;

        public UsersController(
            IUserService userService,
            IConstantService constantService,
            IOrganizationService organizationService) {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
        }

        [HttpGet("{page?}")]
        public async Task<IActionResult> Index(int page = 1) {
            var filter = new UserFilterDTO();

            var myfilter = TempData.Get<UserFilterViewModel>(_indexFilterKey);
            if(myfilter != null) {
                filter = myfilter.Adapt<UserFilterDTO>();
                TempData.Put(_indexFilterKey, myfilter);
            }

            filter.PageNumber = page;

            var result = await _userService.GetListAsync(filter);
            var model = result.Adapt<UsersIndexViewModel>();
            model.SetPositionFilterSource(await getPostionDropDownAsync(), filter.PositionId);
            model.SetOrganizationFilterSource(await getOrganizationDropDownAsync(), filter.OrganizationId);

            return View(model);
        }

        [HttpPost("{page?}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(UserFilterViewModel model, int page = 1) {
            model.CheckArgumentIsNull(nameof(model));
            var filter = model.Adapt<UserFilterDTO>();
            TempData.Put(_indexFilterKey, filter);

            var result = await _userService.GetListAsync(filter);
            var viewModel = result.Adapt<UsersIndexViewModel>();
            viewModel.Filter = model;

            var positionSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__Position))
                .Adapt<List<DropDownItemViewModel>>();
            viewModel.SetPositionFilterSource(positionSource, filter.PositionId);

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<List<DropDownItemViewModel>>();
            viewModel.SetOrganizationFilterSource(orgSource, filter.OrganizationId);

            return View(viewModel);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create() {
            var model = new CreateUserViewModel(
                positions: await getPostionDropDownAsync(),
                organizations: await getOrganizationDropDownAsync()
            );

            return PartialView("_Create", model);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateUserViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            if(!ModelState.IsValid) {
                model.AddError(ViewMessages.ModelState);
                return PartialView("_Create");
            }

            ValidationResult<UserResultDTO> result = null;
            try {
                result = await _userService.CreateAsync(model.Adapt<CreateUserDTO>());
            }
            catch(CreateUserException) {
                return Json(new {
                    hasError = true,
                    message = "خطایی در ایجاد کاربرد وجود دارد!"
                });
            }
            catch(Exception ex) {
                return Json(new {
                    hasError = true,
                    message = ViewMessages.SystemError
                });
            }

            if(result.NotValid) {
                return Json(new {
                    hasError = true,
                    message = result.Message
                });
            }

            return Json(new {
                data = result.Result,
                success = true
            });
        }

        #region helper methods

        private async Task<IEnumerable<DropDownItemViewModel>> getPostionDropDownAsync()
            => (await _constantService.GetByConstantKeyAsync(ConstantKeys.__Position))
                    .Adapt<List<DropDownItemViewModel>>();

        private async Task<IEnumerable<DropDownItemViewModel>> getOrganizationDropDownAsync()
            => (await _organizationService.GetDropDownDataAsync())
                    .Adapt<List<DropDownItemViewModel>>();

        #endregion
    }
}
