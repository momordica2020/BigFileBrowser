using Prism.Mvvm;

namespace FileViewer.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "Prism Demo";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}