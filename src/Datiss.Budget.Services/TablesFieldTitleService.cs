using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities.DWH;
using Datiss.Budget.Enum;
using Datiss.Budget.Security;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Contracts.Identity;
using Datiss.Budget.Services.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services
{
    public class TablesFieldTitleService : ITablesFieldTitleService
    {
        private readonly IUserContext _userContext;
        private readonly IUnitOfWork _uow;
        private readonly IUserService _userService;

        private readonly DbSet<TablesFiledTitle> _dbSet;


        public TablesFieldTitleService(
            IUserContext userContext,
            IUnitOfWork uow,
            IUserService userService)
        {
            _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<TablesFiledTitle>();
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }
        private IQueryable<TablesFiledTitle> Query()
            => _dbSet.AsNoTracking();

        public async Task<TablesFiledTitle> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return await Task.FromResult(entity);
        }

        public async Task<IEnumerable<DropDownItem>> GetByTableSectionNameAsync(TablesName tablesName, SectionName sectionName = SectionName.A)
            => await Query()
                        .Where(x => x.ParentId != null &&
                                    x.TableName == tablesName &&
                                    x.SectionName == sectionName)
                        .OrderBy(x => x.DisplayOrder)
                        .Select(x => new DropDownItem
                        {
                            Id = x.Id,
                            Title = x.Title
                        }).ToListAsync();
    }
}
