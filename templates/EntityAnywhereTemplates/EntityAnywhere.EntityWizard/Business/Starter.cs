using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityAnywhere.EntityWizard
{
    public class Starter : IStarter
    {
        public void Start(IDictionary<string, string> replacementsDictionary)
        {
            var entity = replacementsDictionary["$safeprojectname$"]?.Split('.')?.Reverse().First();
            var vm = new MainWindowViewModel();
            var window = new MainWindow();
            window.EntityTextBox.Text = entity;
            window.ShowDialog();
            replacementsDictionary.Add("$Entity$", window.EntityTextBox.Text);
            replacementsDictionary.Add("$IEntity$", $"I{window.EntityTextBox.Text}");
        }
    }
}