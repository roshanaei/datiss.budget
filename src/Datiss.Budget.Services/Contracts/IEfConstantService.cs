using Datiss.Budget.Entities;
using Datiss.Budget.Services.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Contracts
{
       public interface IEfConstantService
        {
            void AddNewConstant(Constant constant);
            IList<Constant> GetAllConstants();

            void EditConstant(Constant model);
            Task<IList<Constant>> GetAllConstantsAsync();



            //Task<ServiceActionResult<Constant>> AddApiAsync(Constant model);
        //Task<List<ConstantApiModel>> GetAllApiAsync();
    }
    
}
