using JetBrains.Application.BuildScript.Application.Zones;
using JetBrains.Platform.VisualStudio.Protocol.BuildScript;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.TextControl;

namespace Github.Ulex.ZenSharp.Integration
{
    [ZoneDefinition(ZoneFlags.AutoEnable)]
    [ZoneDefinitionConfigurableFeature("ZenSharp", "ZenSharp auto completion", false)]
    public interface IZenSharpZoneDefinition : IZone, IRequire<ITextControlsZone>, IRequire<IVisualStudioBackendZone>, ILanguageCSharpZone
    {
    }

    [ZoneMarker]
    public class ZoneMarker : IRequire<IZenSharpZoneDefinition>
    {
    }
}
