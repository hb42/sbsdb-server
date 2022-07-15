##
# Thin Client IPs in die SBS-DB importieren
#
##

# damit relative Pfade funktionieren
Set-Location $PSScriptRoot

$configScript = "../config/config_internal.json"
$conf = Get-Content $configScript | ConvertFrom-Json
$log = "../$($conf.ThinClientIPs.logfile)"
$importPath = "../$($conf.ThinClientIPs.importPath)" 

function cleanup() {
  rm "$importPath"  -force -recurse -ea SilentlyContinue
}

"$((Get-Date).DateTime) BEGIN Import" >$log
cleanup

##
# Dateien von der tftp-Share der NAS holen
##
mkdir "$importPath"
net use "$($conf.ThinClientIPs.networkShare)" "$($conf.ThinClientIPs.pwd)" /user:"$($conf.ThinClientIPs.user)" /persistent:no
cp "$($conf.ThinClientIPs.networkShare)\$($conf.ThinClientIPs.fileFilter)" "$importPath"
net use "$($conf.ThinClientIPs.networkShare)" /delete

##
# IP-Adressen in die SBS-DB importieren
##
Invoke-RestMethod -uri "$($conf.ThinClientIPs.apiCall)" -UseDefaultCredentials >>$log

cleanup
"$((Get-Date).DateTime) END" >>$log
