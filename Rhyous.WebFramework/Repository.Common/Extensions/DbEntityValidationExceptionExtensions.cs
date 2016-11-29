using System;
using System.Data.Entity.Validation;
using System.Text;

namespace Rhyous.WebFramework.Repositories
{
    public static class DbEntityValidationExceptionExtensions
    {
        public static string GetValidationResultErrorsAsString(this DbEntityValidationException e)
        {
            var errorMsg = new StringBuilder();
            foreach (var validationResult in e.EntityValidationErrors)
            {
                foreach (var error in validationResult.ValidationErrors)
                {
                    errorMsg.Append(validationResult.Entry.Entity + ": ");
                    errorMsg.Append(error.ErrorMessage + Environment.NewLine);
                }
            }
            return errorMsg.ToString();
        }
    }
}