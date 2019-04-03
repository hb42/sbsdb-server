using System;
using System.Reflection;
using System.Text.RegularExpressions;

namespace hb.Common.Version {
  /*
   * VersionResource - liefert Informtionen zur Programmversion
   * 
   * Die Werte werden aus der Assembly ausgelesen. Definert werden die
   * Eintraege in <project>.csproj:
   * 
   * - version:      <VersionPrefix>-<VersionSuffix>
   * - title:        <AssemblyTitle>
   * - description:  <Description>
   * - copyright:    <Copyright>
   * - product:      Projekt-Name/Namespace  
   * 
   * Zusaetzlich wird der Versions-String in seine Einzelteile gemaess
   * semver zerlegt (s. https://semver.org/).  
   */
  public class VersionResource {
    public string Version { get; }
    public string Title { get; }
    public string Description { get; }
    public string Copyright { get; }
    public string Product { get; }

    public string Releaseversion { get; }  // Versions-String ohne pre-release
    public int Major { get; }
    public int Minor { get; }
    public int Patch { get; }
    public string Prerelease { get; }
    public string Pretype { get; }
    public int Prenumber { get; }

    // semver
    private readonly string versionRegex = @"(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)-?(?<prerelease>.*)";
    // interne Konvention: pre-release hat das Format <alpha|beta|rc>.<lfd. Nr.>, z.B. beta.22
    private readonly string prereleaseRegex = @"(?<prerelease>alpha|beta|rc)\.?(?<build>\d*)";

    public VersionResource() {
      Assembly myAssembly = Assembly.GetCallingAssembly();
      Version = myAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
      Title = myAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
      Description = myAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
      Copyright = myAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
      Product = myAssembly.GetCustomAttribute<AssemblyProductAttribute>().Product;

      Match m = Regex.Match(Version, versionRegex);
      if (m.Success) {
        Major = Int32.Parse(m.Groups["major"].Value);
        Minor = Int32.Parse(m.Groups["minor"].Value);
        Patch = Int32.Parse(m.Groups["patch"].Value);
        Prerelease = m.Groups["prerelease"].Value;
      }
      Releaseversion = $"{Major}.{Minor}.{Patch}";
      m = Regex.Match(Version, prereleaseRegex, RegexOptions.IgnoreCase);
      if (m.Success) {
        Pretype = m.Groups["prerelease"].Value;
        Prenumber = Int32.Parse(m.Groups["build"].Value);
      }
    }

    public override string ToString() {
      return $"{Title} {Version} {Copyright}";
    }
  }
}
