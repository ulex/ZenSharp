using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.TextControl;

namespace Github.Ulex.ZenSharp.Integration
{
    [ZoneDefinition(ZoneFlags.AutoEnable)]
    [ZoneDefinitionConfigurableFeature("ZenSharp", "ZenSharp auto completion", false)]
    public interface IZenSharpZoneDefinition : IZone, IRequire<ITextControlsZone>, ILanguageCSharpZone
    {
    }

    [ZoneMarker]
    public class ZoneMarker : IRequire<IZenSharpZoneDefinition>
    {
    }
}