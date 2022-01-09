using System.IO;
using System.Linq;
using Ganss.Excel;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Datiss.Budget.Common.GuardToolkit;
using System.Threading.Tasks;
using Datiss.Budget.Common.Exceptions;

namespace Datiss.Budget.Services.Excel
{

    public interface IExcelService
    {
        Task<IEnumerable<TResult>> ImportAsync<TResult>(
            IFormFile fileInfo) where TResult : class;

        Stream Export<T>(IEnumerable<T> model, Stream stream) where T : class;

        Task<IEnumerable<TResult>> ImportAsync<TResult>(
            IFormFile fileInfo,
            int sheetIndex,
            int minRowNum) where TResult : class;
    }

    public class ExcelService : IExcelService 
    {

        private readonly ExcelMapper _mapper;

        public ExcelService() {
            _mapper = new ExcelMapper();
        }

        private string[] _validExts = ".xls;.xlsx;.csv".Split(";");
        private const int _maxSize = 1024 * 1024 * 5; //TODO : add to appsetting

        private bool validateExcelFile(IFormFile fileInfo) {
            var ext = Path.GetExtension(fileInfo.FileName);

            if (fileInfo == null || fileInfo.Length == 0)
                throw new ImportExcelFileException();

            if (!_validExts.Contains(ext))
                throw new ImportExcelFileFormatInvalidException();

            if (fileInfo.Length > _maxSize)
                throw new ImportExcelFileSizeInvalidException();

            return true;
        }

        public async Task<IEnumerable<TResult>> ImportAsync<TResult>(IFormFile fileInfo) where TResult : class {
            validateExcelFile(fileInfo);

            using (var stream = new MemoryStream()) {
                await fileInfo.CopyToAsync(stream);
                var result = _mapper.Fetch<TResult>(stream, 0);

                return await Task.FromResult(result);
            }
        }

        public async Task<IEnumerable<TResult>> ImportAsync<TResult>(
            IFormFile fileInfo, 
            int sheetIndex, 
            int minRowNum) where TResult : class {

            using (var stream = new MemoryStream()) {
                await fileInfo.CopyToAsync(stream);
                _mapper.HeaderRow = false;
                _mapper.MinRowNumber = minRowNum;
                var result = _mapper.Fetch<TResult>(stream, sheetIndex);

                return await Task.FromResult(result);
            }
        }

        public Stream Export<T>(IEnumerable<T> model, Stream stream) where T : class {
            _mapper.Save<T>(stream, model, "Sheet1");

            return stream;
        }
    }
}
