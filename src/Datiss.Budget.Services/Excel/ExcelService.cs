using Ganss.Excel;
using System.Collections.Generic;

namespace Datiss.Budget.Services.Excel
{

    public interface IExcelService
    {
        IEnumerable<TResult> Import<TResult>(string filename) where TResult : class;

        void Export<T>(IEnumerable<T> model, string filename) where T : class;
    }

    public class ExcelService : IExcelService 
    {

        private readonly ExcelMapper _mapper;

        public ExcelService() {
            _mapper = new ExcelMapper();
        }

        public IEnumerable<TResult> Import<TResult>(string filename) where TResult : class {
            return _mapper.Fetch<TResult>(filename);
        }

        public void Export<T>(IEnumerable<T> model, string filename) where T : class {
            _mapper.Save<T>(filename, model, "Sheet1");
        }
    }
}
