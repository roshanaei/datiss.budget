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

    public class ValidationResult<T> : ValidationResult
    {
        public T Result { get; set; }

        public static ValidationResult<T> Success(T result)
            => new ValidationResult<T> {
                IsValid = true,
                Result = result
            };

        public static ValidationResult<T> Success(T result, ValidationMode mode, string message = "")
            => new ValidationResult<T>
            {
                Result = result,
                IsValid = true,
                Message = message,
                Mode = mode
            };

        public static new ValidationResult<T> Failed(string message)
            => new ValidationResult<T> {
                IsValid = false,
                Message = message
            };

        public static new ValidationResult<T> Failed(ValidationMode mode, string message)
            => new ValidationResult<T>
            {
                IsValid = false,
                Message = message,
                Mode = mode
            };


        public static new ValidationResult<T> Failed(T result, ValidationMode mode, string message)
            => new ValidationResult<T>
            {
                Result = result,
                IsValid = false,
                Message = message,
                Mode = mode
            };

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

        public bool NotValid => !IsValid;

        public string Message { get; set; }

        public void AddError(string key, string errorMessage)
        {
            _errors.Add(key, errorMessage);
        }

        public static ValidationResult Success()
            => new ValidationResult {
                IsValid = true
            };
        
        public static ValidationResult Success(ValidationMode mode, string message = "")
            => new ValidationResult
            {
                IsValid = true,
                Message = message,
                Mode = mode
            };

        public static ValidationResult Failed(string message)
            => new ValidationResult
            {
                IsValid = false,
                Message = message
            };

        public static ValidationResult Failed(ValidationMode mode, string message)
            => new ValidationResult
            {
                IsValid = false,
                Message = message,
                Mode = mode
            };

    }
}