using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
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
using Datiss.Budget.Services.Contracts.Identity;
using Mapster;
using Datiss.Budget.Resources;
using Datiss.Budget.Common.Exceptions;
using Datiss.Budget.Services.Infrastructure;

namespace Datiss.Budget.Web.Admin.Controllers
{

    [Authorize(Roles = ConstantRoles.Admin)]
    [Area(AreaConstants.AdminArea)]
    [Route("[area]/[controller]")]
    public class UsersController : Controller
    {
        public const string Name = "Users";
        public const string ACTION_Index = nameof(Index);
        public const string ACTION_Create = nameof(Create);
        public const string ACTION_Edit = nameof(Edit);
        public const string ACTION_SetUserPassword = nameof(SetUserPassword);

        private readonly string _indexFilterKey = $"{Name}_{ACTION_Index}";

        private readonly IUserService _userService;
        private readonly IConstantService _constantService;
        private readonly IOrganizationService _organizationService;
        private readonly IApplicationRoleManager _roleManager;

        public UsersController(
            IUserService userService,
            IConstantService constantService,
            IOrganizationService organizationService,
            IApplicationRoleManager roleManager) 
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _constantService = constantService ?? throw new ArgumentNullException(nameof(constantService));
            _organizationService = organizationService ?? throw new ArgumentNullException(nameof(organizationService));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
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
        public async Task<IActionResult> Index(UsersIndexViewModel model, int page = 1) {
            model.CheckArgumentIsNull(nameof(model));
            var filter = model.Filter.Adapt<UserFilterDTO>();
            TempData.Put(_indexFilterKey, filter);
            
            var result = await _userService.GetListAsync(filter);
            model = result.Adapt<UsersIndexViewModel>();
            
            var positionSource = (await _constantService.GetByConstantKeyAsync(ConstantKeys.__Position))
                .Adapt<List<DropDownItemViewModel>>();
            model.SetPositionFilterSource(positionSource, filter.PositionId);

            var orgSource = (await _organizationService.GetDropDownDataAsync())
                .Adapt<List<DropDownItemViewModel>>();
            model.SetOrganizationFilterSource(orgSource, filter.OrganizationId);

            return View(model);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Create() {
            var model = new CreateUserViewModel(
                positions: await getPostionDropDownAsync(),
                organizations: await getOrganizationDropDownAsync(),
                roles: await getRoleDropDownAsync()
            );

            return View(model);
        }

        //[HttpGet("[action]")]
        //public async Task<IActionResult> Create() {
        //    var model = new CreateUserViewModel(
        //        positions: await getPostionDropDownAsync(),
        //        organizations: await getOrganizationDropDownAsync()
        //    );

        //    return PartialView("_Create", model);
        //}

        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateUserViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            if (!ModelState.IsValid) {
                model.SetOrganizationSource(await getOrganizationDropDownAsync());
                model.SetPositionSource(await getPostionDropDownAsync());
                model.SetRoleSource(await getRoleDropDownAsync());
                model.AddError(ViewMessages.ModelState);
                return View(model);
            }

            ValidationResult<UserResultDTO> result = null;
            try {
                result = await _userService.CreateAsync(model.Adapt<CreateUserDTO>());
                if (result.NotValid) {
                    model.AddError(result.Message);
                    return View(model);
                }
            }
            catch (CreateUserException err) {
                model.AddError(err.MyErrors);
            }
            catch (Exception ex) {
                model.AddError(ViewMessages.SystemError);
            }

            if (model._HasError) {
                model.SetOrganizationSource(await getOrganizationDropDownAsync());
                model.SetPositionSource(await getPostionDropDownAsync());
                return View(model);
            }

            return RedirectToAction(ACTION_Index);
        }

        //[HttpPost("[action]")]
        //public async Task<IActionResult> Create(CreateUserViewModel model) {
        //    model.CheckArgumentIsNull(nameof(model));

        //    if (!ModelState.IsValid) {
        //        model.AddError(ViewMessages.ModelState);
        //        return PartialView("_Create", model);
        //        //return View(model);
        //    }

        //    ValidationResult<UserResultDTO> result = null;
        //    try {
        //        result = await _userService.CreateAsync(model.Adapt<CreateUserDTO>());
        //        if (result.NotValid) {
        //            model.AddError(result.Message);
        //            return View(model);
        //        }
        //    }
        //    catch (CreateUserException) {
        //        return Json(new {
        //            hasError = true,
        //            message = "خطایی در ایجاد کاربر وجود دارد!"
        //        });
        //    }
        //    catch (Exception ex) {
        //        return Json(new {
        //            hasError = true,
        //            message = ViewMessages.SystemError
        //        });
        //    }

        //    if (result.NotValid) {
        //        return Json(new {
        //            hasError = true,
        //            message = result.Message
        //        });
        //    }

        //    return Json(new {
        //        data = result.Result,
        //        success = true
        //    });
        //}

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id) {
            try {
                var user = await _userService.GetByIdAsync(id);
                var model = user.Adapt<UpdateUserViewModel>();
                model.SetPositionSource((await getPostionDropDownAsync()), user.PositionId);
                model.SetOrganizationSource((await getOrganizationDropDownAsync()), user.OrganizationId);
                
                return View(model);
            }
            catch(NullReferenceException) {
                return NotFound();
            }
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateUserViewModel model) {
            model.CheckArgumentIsNull(nameof(model));

            var data = model.Adapt<UpdateUserDTO>();
            ValidationResult<UserResultDTO> result = null;
            try {
                result = await _userService.UpdateAsync(data);
                if(result.NotValid) {
                    model.SetOrganizationSource(await getOrganizationDropDownAsync());
                    model.SetPositionSource(await getPostionDropDownAsync());
                    model.AddError(result.Message);
                    return View(model);
                }
            }
            catch(UpdateUserException err) 
            {
                model.AddError(err.MyErrors);
            }
            catch(Exception ex) {
                model.AddError(ViewMessages.SystemError);
            }

            if (model._HasError) {
                model.SetOrganizationSource(await getOrganizationDropDownAsync());
                model.SetPositionSource(await getPostionDropDownAsync());
                return View(model);
            }
            
            return RedirectToAction(ACTION_Index);
        }

        [HttpGet("edit/password/{id}")]
        public async Task<IActionResult> SetUserPassword(int id) 
        {
            try 
            {
                var user = await _userService.GetByIdAsync(id);
                var model = new AdminSetUserPasswordViewModel {
                    UserId = user.Id,
                    UserDisplayName = user.DisplayName,
                    UserName = user.UserName
                };

                return PartialView("_setUserPassword", model);
            }
            catch(Exception ex) 
            {
                return NotFound();
            }
        }

        [HttpPost("edit/password")]
        public async Task<IActionResult> SetUserPassword(AdminSetUserPasswordViewModel model) 
        {
            model.CheckArgumentIsNull(nameof(model));

            try {
                await _userService.SetUserPasswordAsync(model.UserId, model.NewPassword);

                return Ok();
            }
            catch(UserChangePasswordException ex) {
                return new JsonResult(new {
                    hasError = true,
                    message = ex.MyErrors
                });
            }
            catch(Exception ex) {
                return new JsonResult(new {
                    hasError = true,
                    message = ViewMessages.SystemError
                });
            }
        }

        #region private helper methods

        private async Task<IEnumerable<DropDownItemViewModel>> getPostionDropDownAsync()
            => (await _constantService.GetByConstantKeyAsync(ConstantKeys.__Position))
                    .Adapt<List<DropDownItemViewModel>>();

        private async Task<IEnumerable<DropDownItemViewModel>> getOrganizationDropDownAsync()
            => (await _organizationService.GetDropDownDataAsync())
                    .Adapt<List<DropDownItemViewModel>>();

        private async Task<IEnumerable<DropDownItemViewModel>> getRoleDropDownAsync()
            => (await _roleManager.GetAllCustomRolesAsync())
                .Select(x => new DropDownItemViewModel {
                    Id = x.Id,
                    Title = x.Name
                }).ToList();

        #endregion
    }
}
