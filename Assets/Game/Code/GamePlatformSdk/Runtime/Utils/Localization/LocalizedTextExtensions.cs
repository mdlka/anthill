using System.Collections.Generic;
using System.Linq;

namespace YellowSquad.GamePlatformSdk
{
    public static class LocalizedTextExtensions
    {
        public static string SelectCurrentLanguageText(this IEnumerable<LocalizedText> texts)
        {
            return texts.First(text => text.Language == GamePlatformSdkContext.Current.Language).Text;
        }
    }
}