using System.Collections.Generic;
using System.Linq;

namespace Neubel.Wow.Win.Authentication.Common
{
    /// <summary>
    /// A set of extension methods enhancing collections
    /// </summary>
    public static class CollectionExtensionMethods
    {
        #region Public Methods
        
        /// <summary>
        /// Gets the subset of validation messages from  that are of severity Error or higher.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IList<ValidationMessage> GetFailureValidationMessages<T>(this IList<T> validationMessages) where T : ValidationMessage
        {
            List<ValidationMessage> errorValMsgs = new List<ValidationMessage>();
            if (validationMessages != null)
            {
                errorValMsgs.AddRange(validationMessages.Where(vm => vm.Severity >= ValidationSeverity.Error));
            }

            return errorValMsgs;
        }
        
        #endregion
    }
}
