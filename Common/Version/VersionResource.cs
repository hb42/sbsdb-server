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
     *
     */
    public class VersionResource {
        // interne Konvention: pre-release hat das Format <alpha|beta|rc>.<lfd. Nr.>, z.B. beta.22
        private readonly string prereleaseRegex = @"(?<prerelease>alpha|beta|rc)\.?(?<build>\d*)";

        // semver
        private readonly string versionRegex = @"(?<major>\d+)\.(?<minor>\d+)\.(?<patch>\d+)-?(?<prerelease>.*)";

        public VersionResource() {
            var myAssembly = Assembly.GetCallingAssembly();
            
            try {
                Version = myAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
                Title = myAssembly.GetCustomAttribute<AssemblyTitleAttribute>().Title;
                Description = myAssembly.GetCustomAttribute<AssemblyDescriptionAttribute>().Description;
                Copyright = myAssembly.GetCustomAttribute<AssemblyCopyrightAttribute>().Copyright;
                Product = myAssembly.GetCustomAttribute<AssemblyProductAttribute>().Product;
            }
            catch {
                Version ??= "0.0.0";
                Title ??= "title";
                Description ??= "desc";
                Copyright ??= "(c)";
                Product ??= "product";
            }

            var m = Regex.Match(Version, versionRegex);
            if (m.Success) {
                Major = int.Parse(m.Groups["major"].Value);
                Minor = int.Parse(m.Groups["minor"].Value);
                Patch = int.Parse(m.Groups["patch"].Value);
                Prerelease = m.Groups["prerelease"].Value;
            }

            Releaseversion = $"{Major}.{Minor}.{Patch}";
            m = Regex.Match(Version, prereleaseRegex, RegexOptions.IgnoreCase);
            if (m.Success) {
                Pretype = m.Groups["prerelease"].Value;
                Prenumber = int.Parse(m.Groups["build"].Value);
            }
        }

        public string Version { get; }
        public string Title { get; }
        public string Description { get; }
        public string Copyright { get; }
        public string Product { get; }

        public string Releaseversion { get; } // Versions-String ohne pre-release
        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        public string Prerelease { get; }
        public string Pretype { get; }
        public int Prenumber { get; }

        /**
         * Versions-Info im package.json-Format
         */
        public object Package() {
            return new {
                version = Version,
                name = Product,
                displayname = Title,
                description = Description,
                copyright = Copyright,
                author = "",  // <authors> wird anscheinend nicht in assembly geschrieben
                license = "MIT", // erst mal fix
                versions = new string[] {"ASP.NET Core " + AspNetCoreMvcVersion()} // TODO + iis version + ggf. windows version
            };
        }
        public override string ToString() {
            return $"{Title} {Version} {Copyright}";
        }
        public string AspNetCoreMvcVersion() {
            // unknown mvc core version
            string mvcCoreVersion = "0";
            try {
                string mvcCoreClass = "Microsoft.AspNetCore.Mvc.Controller, Microsoft.AspNetCore.Mvc.ViewFeatures";
                mvcCoreVersion = Type.GetType(mvcCoreClass).Assembly.GetName().Version.ToString();
            } catch (Exception) {
                // the mvc core Controller class does not exist in the app
            }
            return mvcCoreVersion;
        }
    }
}
