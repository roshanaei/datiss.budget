using Datiss.Budget.DataLayer.Context;
using Datiss.Budget.Entities;
using Datiss.Budget.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Organization = Datiss.Budget.Entities.Organization;

namespace Datiss.Budget.Services
{
    public class EfOrganizationService : IEfOrganizationService
    {
        private readonly IUnitOfWork _uow;
        //private readonly IEfProductService _productService;
        private readonly DbSet<Organization> _organizations;

        //public EfOrganizationService(IUnitOfWork uow, IEfProductService productService)
        //{
        //    _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        //    _productService = productService ?? throw new ArgumentNullException(nameof(productService));

        //    _organizations = _uow.Set<Organization>();
        //}

        public void AddNewOrganization(Organization organization)
        {
            _organizations.Add(organization);
        }

        public void EditOrganization(Organization model)
        {
            var organization = _organizations.Single(x => x.Id == model.Id);
            if (organization != null)
            {
                organization.Title = model.Title;
                _uow.SaveChanges();
            }
        }

        public IList<Organization> GetAllOrganizations()
        {
            return _organizations.ToList();
        }

        public async Task<IList<Organization>> GetAllOrganizationsAsync()
        {
            return await _organizations.ToListAsync();
        }
    }
}
