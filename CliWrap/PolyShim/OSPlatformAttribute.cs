#pragma warning disable CS0436

namespace System.Runtime.Versioning;

// https://learn.microsoft.com/dotnet/api/system.runtime.versioning.osplatformattribute
internal abstract class OSPlatformAttribute(string platformName) : Attribute
{
    public string PlatformName { get; } = platformName;
}