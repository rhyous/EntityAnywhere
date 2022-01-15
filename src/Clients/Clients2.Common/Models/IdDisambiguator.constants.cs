using System.Diagnostics.CodeAnalysis;

namespace Rhyous.EntityAnywhere.Clients2
{
    [ExcludeFromCodeCoverage]
    public class IdDisambiguator
    {
        public static string Prefix = "$";
        /// <summary>
        /// The separator. 
        /// </summary>
        /// <remarks>We would prefer not to use the dot characer,
        /// but so few options are available as URL parameters.</remarks>
        public static char Separator = '.';
        /// <summary>
        /// This says that it is for sure the Id property. Which is usual.
        /// </summary>
        public static string Id = $"{Prefix}Id";
        /// <summary>
        /// This says that it is not the Id but is alternate, either AlternateKey or AlternateId.
        /// AlternateId would be in this format: $Alt.Property.Value
        /// AlternateKey would be in this format: $Alt.$Key.Value
        /// </summary>
        public static string Alt = $"{Prefix}Alt";
        /// <summary>
        /// This says that this identifier is for sure the AlternateKey.
        /// </summary>
        public static string Key = $"{Prefix}Key";
        /// <summary>
        /// This says that this identifier is for sure the AlternateKey.
        /// </summary>
        public static string AltKey = $"{Alt}{Separator}{Key}{Separator}";
    }
}