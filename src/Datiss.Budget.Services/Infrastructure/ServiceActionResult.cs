using System.Collections.Generic;

namespace Datiss.Budget.Services.Infrastructure
{
    public class ServiceActionResult<T> where T : class
    {
        public bool Success { get; set; }
        public T Entity { get; set; }
        public List<T> Entities { get; set; }
        public object Id { get; set; }
        public string Message { get; set; }
        public List<string> Messages { get; set; }
        public object Value { get; set; }
    }
}
