using System.Collections.Generic;

namespace Datiss.Budget.Services.Infrastructure
{
    public enum ValidationMode
    {
        Create = 0,
        Update = 1,
        SoftDelete = 2
    }

    public interface IValidationResult
    {
        void AddError(string key, string errorMessage);
        bool IsValid { get; set; }
        string Message { get; set; }
    }

    public class ValidationResult : IValidationResult
    {
        private Dictionary<string, string> _errors;

        public ValidationResult()
        {
            _errors = new Dictionary<string, string>();
        }

        public ValidationMode Mode { get; set; }

        public bool IsValid { get; set; }

        public string Message { get; set; }

        public void AddError(string key, string errorMessage)
        {
            _errors.Add(key, errorMessage);
        }

        public static ValidationResult Success() {
            return new ValidationResult {
                IsValid = true
            };
        }

        public static ValidationResult Failed(string message) {
            return new ValidationResult {
                IsValid = false,
                Message = message
            };
        }
    }
}