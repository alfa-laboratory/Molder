using System.Diagnostics.CodeAnalysis;
using TechTalk.SpecFlow;

namespace EvidentInstruction.Generator.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class LocaleExtension
    {
        public static string Locale(this FeatureContext feature)
        {
            return feature.FeatureInfo.Language.TwoLetterISOLanguageName;
        }
    }
}
