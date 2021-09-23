namespace Datiss.Budget.ViewModels
{
    public abstract class BaseViewModel
    {
        public bool _HasError { get; set; }

        public string _ErrorMessage { get; set; }

        public int _CurrentPage { get; set; } = 1;


        public void AddError(string msg) {
            _HasError = true;
            _ErrorMessage = msg;
        }
    }
}
