param (
  $csproj  # project file
)
##
# build-Nummer hochzaehlen
# Die Nummer wird im project file als <VersionSuffix> gefuehrt. 
# Format <alpha|beta|rc>.<build nr>
#
# Bei einer Release-Version (ohne VersionSuffix) macht das Script nichts.
# Wenn nur ein String als VersionSuffix eingetragen ist, wird ".0" angehaengt.
#
# Script als PreBuild-Script:
#   powershell –NonInteractive –noprofile -file ./build/prebuild.ps1 -csproj ${ProjectFile}
#   Working Directory = ${ProjectDir}
#
# Die Aenderung der .csproj wird erst beim naechsten Buildlauf beruecksichtigt.
# -> "build" + "publish"
##

# ist die Projekt-Datei vorhanden? 
if (test-path $csproj) {
  # .csproj als XML einlesen
  $xml = [xml](Get-Content $csproj)
  
  # die beiden Versioneintraege holen
  # Hauptversion, wird momentan nicht veraendert
  [string]$versionPrefix = $xml.Project.PropertyGroup.VersionPrefix
  # pre-release
  [string]$versionSuffix = $xml.Project.PropertyGroup.VersionSuffix
  
  # sofern vorhanden in Versions-Nummern zerlegen
  if ($versionPrefix) {
    $major, $minor, $patch  = $versionPrefix.Split(".")
  }
  # blanks entfernen
  $major = $major.trim()
  $minor = $minor.trim()
  $patch = $patch.trim()
  
  if ($versionSuffix) {
    $pretype, $prenumber = $versionSuffix.Split(".")
  }
  # pre-release vorhanden?
  if ($pretype) {
    # Nummer in int konvertieren
    [int]$build = 0
    [bool]$isnumber = [int]::TryParse($prenumber, [ref]$build)
    # sofern gueltige Zahl gefunden, hochzaehlen (ansonsten 0) 
    if ($isnumber) {
      $build++
    }
    # den passenden <PropertyGroup>-Abschnitt holen und neuen Wert eintragen
    ($xml.Project.PropertyGroup | Where-Object { $_['VersionSuffix'] -ne $null}).VersionSuffix = "${pretype}.${build}"
    ($xml.Project.PropertyGroup | Where-Object { $_['PackageVersion'] -ne $null}).PackageVersion = "${major}.${minor}.${patch}-${pretype}.${build}"
  
    # .csproj speichern
    $xml.Save($csproj)
  } 
}


