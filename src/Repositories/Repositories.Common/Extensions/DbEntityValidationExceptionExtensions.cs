using System;
using System.Data.Entity.Validation;
using System.Text;

namespace Rhyous.WebFramework.Repositories
{
    /// <summary>
    /// This extension takes a DbEntityValidationException and creates a more understandable error message by putting entity validation error data into a string.
    /// </summary>
    public static class DbEntityValidationExceptionExtensions
    {
        /// <summary>
        /// Gets entity validation errors and puts them into a string.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
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