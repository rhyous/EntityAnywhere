using System.Collections.Generic;
using System.Linq;

namespace EntityAnywhere.EntityWizard
{
    internal class Starter : IStarter
    {
        public void Start(IDictionary<string, string> replacementsDictionary)
        {
            var entity = replacementsDictionary["$safeprojectname$"]?.Split('.')?.Reverse().First();
            if (!replacementsDictionary.TryGetValue("$EntityAnywhereTemplateType$", out string templateType))
                templateType = "Entity";
            var model = ModelDictionary[templateType];
            var vm = new MainWindowViewModel(model);
            var window = new MainWindow();
            window.DataContext = vm;
            window.EntityTextBox.Text = entity;
            window.ShowDialog();
            replacementsDictionary.Add("$Entity$", window.EntityTextBox.Text);
            replacementsDictionary.Add("$IEntity$", $"I{window.EntityTextBox.Text}");
        }

        public IDictionary<string,Model> ModelDictionary
        {
            get { return _ModelDictionary ?? (_ModelDictionary = new ModelDictionary()); }
            set { _ModelDictionary = value; }
        } private IDictionary<string, Model> _ModelDictionary;
    }
}