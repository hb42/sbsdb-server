##
# Dateien von der tftp-Share der NAS holen
##
$configScript = "config_internal.json"

$here = $PSScriptRoot
$conf = Get-Content "${here}\${configScript}" | ConvertFrom-Json

rm "${here}/$($conf.ThinClientIPs.localPath)" -force -recurse -ea SilentlyContinue
mkdir "${here}/$($conf.ThinClientIPs.localPath)"

net use "$($conf.ThinClientIPs.networkShare)" "$($conf.ThinClientIPs.pwd)" /user:"$($conf.ThinClientIPs.user)" /persistent:no
cp "$($conf.ThinClientIPs.networkShare)\$($conf.ThinClientIPs.fileFilter)" "${here}\$($conf.ThinClientIPs.localPath)"
net use "$($conf.ThinClientIPs.networkShare)" /delete
