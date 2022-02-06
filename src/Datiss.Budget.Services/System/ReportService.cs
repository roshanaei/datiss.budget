using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Datiss.Budget.Common.GuardToolkit;
using Datiss.Budget.Enum;
using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.Resources;
using Datiss.Budget.Extensions;
using Datiss.Budget.Services.Infrastructure;
using Datiss.Budget.Services.Contracts;
using Datiss.Budget.Services.Models;
using Mapster;

namespace Datiss.Budget.Services
{
    public class ReportService : IReportService
    {

        private readonly IUnitOfWork _uow;

        private readonly DbSet<Report> _dbSet;

        public ReportService(IUnitOfWork uow) {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
            _dbSet = _uow.Set<Report>();
        }

        public async Task<Report> GetAsync(int id) {
            var report = await _dbSet
                .Include(_ => _.Params)
                .SingleOrDefaultAsync(x => x.Id == id);

            report.CheckArgumentIsNull(nameof(report));

            return report;
        }

        public async Task<Report> GetAsync(string name) {
            var report = await _dbSet
               .Include(_ => _.Params)
               .SingleOrDefaultAsync(x => x.Name.ToUpper() == name.ToUpper());

            report.CheckArgumentIsNull(nameof(report));

            return report;
        }

        public async Task<PagedResult<ReportDTO>> GetAdminListAsync(ReportFilterDTO filter) {
            filter.CheckArgumentIsNull(nameof(filter));

            var result = new PagedResult<ReportDTO>
            {
                PageSize = filter.PageSize,
                PageNumber = filter.PageNumber
            };

            var query = _dbSet.Where(_ => _.Status != EntityStatus.Deleted);

            //set filter
            if(filter.Search.IsNotNullOrEmpty()) {
                filter.Search = filter.Search.ToUpper().CorrectYeKe();
                query = query.Where(_ => _.Title.ToUpper().Contains(filter.Search) ||
                                        _.Name.ToUpper().Contains(filter.Search));
            }

            result.TotalCount = await query.CountAsync();

            //apply paging
            query = query
                .Skip(filter.StartIndex)
                .Take(filter.PageSize);

            result.Items = await query.Include(_ => _.Params)
                                      .Select(_ => _.Adapt<ReportDTO>())
                                      .ToListAsync();

            return await Task.FromResult(result);
        }

        public async Task<ValidationResult<ReportDTO>> CreateAsync(CreateReportData model) 
        {
            model.CheckArgumentIsNull(nameof(model));

            if (checkMandatoryFieldsIsEmpty(model.Title, model.Name))
                return ValidationResult<ReportDTO>
                    .Failed(ValidationMode.Create, ServiceMessages.MandatoryFields);

            if (await existByNameAsync(model.Name))
                return ValidationResult<ReportDTO>
                    .Failed(ValidationMode.Create, ServiceMessages.ReportExistByName);

            var report = new Report
            {
                Name = model.Name.CorrectYeKe(),
                Title = model.Title.CorrectYeKe(),
                Status = EntityStatus.Enabled,
                Description = model.Description?.CorrectYeKe(),
                FileData = model.FileData
            };

            foreach(var p in model.Parameters) {
                report.Params.Add(p.Adapt<ReportParam>());
            }

            await _dbSet.AddAsync(report);
            await _uow.SaveChangesAsync();

            return ValidationResult<ReportDTO>
                .Success(report.Adapt<ReportDTO>(), ValidationMode.Create);
        }

        public async Task<ValidationResult<ReportDTO>> UpdateAsync(UpdateReportData model) 
        {
            model.CheckArgumentIsNull(nameof(model));

            var report = await _dbSet.Include(_ => _.Params)
                                     .SingleOrDefaultAsync(_ => _.Id == model.Id);
            report.CheckReferenceIsNull(nameof(report));

            if(checkMandatoryFieldsIsEmpty(model.Title, model.Name))
                return ValidationResult<ReportDTO>
                     .Failed(ValidationMode.Update, ServiceMessages.MandatoryFields);
                     
            if (await existByNameAsync(model.Name, model.Id))
                return ValidationResult<ReportDTO>
                    .Failed(ValidationMode.Update, ServiceMessages.ReportExistByName);

            report.Name = model.Name.CorrectYeKe();
            report.Title = model.Title.CorrectYeKe();
            report.Description = model.Description?.CorrectYeKe();
            report.Status = model.Status;
            report.Params.Clear();
            foreach (var p in model.Parameters) {
                report.Params.Add(p.Adapt<ReportParam>());
            }

            _dbSet.Update(report);
            await _uow.SaveChangesAsync();

            return ValidationResult<ReportDTO>.Success(
                report.Adapt<ReportDTO>(), 
                ValidationMode.Update);
        }


        #region private methods

        private bool checkMandatoryFieldsIsEmpty(string title, string name)
            => title.IsNullOrEmpty() || name.IsNullOrEmpty();
        
        private async Task<bool> existByNameAsync(string name, int? reportId = null)
            => reportId.HasValue
                ? await _dbSet.AnyAsync(_ => _.Name.ToUpper() == name.ToUpper().CorrectYeKe())
                : await _dbSet.AnyAsync(_ => _.Name.ToUpper() == name.ToUpper().CorrectYeKe() &&
                                                _.Id != reportId);

        private IQueryable<Report> setOrder(
            IQueryable<Report> query,
            string orderBy = "id",
            bool desc = false) {
            if (string.IsNullOrWhiteSpace(orderBy))
                orderBy = "id";

            orderBy = orderBy.ToLower();
            switch(orderBy) {
                case "title":
                    return desc
                        ? query.OrderByDescending(_ => _.Title)
                        : query.OrderBy(_ => _.Title);
                case "name":
                    return desc
                        ? query.OrderByDescending(_ => _.Name)
                        : query.OrderBy(_ => _.Name);
                default:
                    return desc
                        ? query.OrderByDescending(_ => _.Id)
                        : query.OrderBy(_ => _.Id);
            }
        }

        #endregion
    }
}
