namespace Datiss.Budget.ViewModels
{
    public abstract class BaseViewModel
    {
        public bool _HasError { get; set; }

        public string _ErrorMessage { get; set; }

        public int _CurrentPage { get; set; } = 1;

        public bool _Success { get; set; }

        public string _Message { get; set; }

        public void Succeed(string msg) {
            _Success = true;
            _Message = msg;
        }

        public void AddError(string msg) {
            _HasError = true;
            _ErrorMessage = msg;
        }
    }

}
