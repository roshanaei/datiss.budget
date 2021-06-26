using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datiss.Budget.Services.Infrastructure
{
    public class ServiceResult<T> where T: class 
    {

        public T Result { get; set; }

        public ValidationResult Validation { get; set; }
    }
}
