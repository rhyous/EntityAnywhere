using Rhyous.Mvvm;

namespace EntityAnywhere.EntityWizard
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private readonly Model _Model;

        public MainWindowViewModel(Model model)
        {
            _Model = model;
        }

        public string WindowTitle
        {
            get { return _Model.WindowTitle; }
            set
            {
                if (_Model.WindowTitle == value)
                    return;
                _Model.WindowTitle = value;
                NotifyPropertyChanged();
            }
        }

        public string Heading
        {
            get { return _Model.Heading; }
            set
            {
                if (_Model.Heading == value)
                    return;
                _Model.Heading = value;
                NotifyPropertyChanged();
            }
        }

        public string Label
        {
            get { return _Model.Label; }
            set
            {
                if (_Model.Label == value)
                    return;
                _Model.Label = value;
                NotifyPropertyChanged();
            }
        }
    }
}