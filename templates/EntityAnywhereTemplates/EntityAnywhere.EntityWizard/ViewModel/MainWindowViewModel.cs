using Rhyous.Mvvm;

namespace EntityAnywhere.EntityWizard
{
    class MainWindowViewModel : ViewModelBase
    {
        public int Title
        {
            get { return _Title; }
            set { SetAndNotifyPropertyChanged(ref _Title, value); }
        } private int _Title;

        public int Label
        {
            get { return _Label; }
            set { SetAndNotifyPropertyChanged(ref _Label, value); }
        } private int _Label;

    }
}