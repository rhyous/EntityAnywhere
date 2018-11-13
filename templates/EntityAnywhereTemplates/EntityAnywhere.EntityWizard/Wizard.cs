using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace EntityAnywhere.EntityWizard
{
    class Wizard : IWizard
    {
        // This method is called before opening any item that   
        // has the OpenInEditor attribute.  
        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        // This method is only called for item templates,  
        // not for project templates.  
        public void ProjectItemFinishedGenerating(ProjectItem
            projectItem)
        {
        }

        // This method is called after the project is created.  
        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary,
                               WizardRunKind runKind, object[] customParams)
        {
            try
            {
                var entity = replacementsDictionary["$safeprojectname$"]?.Split('.')?.Reverse().First();
                var window = new MainWindow();
                window.EntityTextBox.Text = entity;
                window.ShowDialog();
                replacementsDictionary.Add("$Entity$", window.EntityTextBox.Text);
                replacementsDictionary.Add("$IEntity$", $"I{window.EntityTextBox.Text}");                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // This method is only called for item templates,  
        // not for project templates.  
        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}