##
# Dateien von der tftp-Share auf der NAS holen
# Im IIS-Kontext klappt der Zugriff auf die NAS nicht.
##
$configScript = "config_internal.json"
$fileFilter = "E077*"

$here = $PSScriptRoot
$conf = Get-Content "${here}\${configScript}" | ConvertFrom-Json

rm "${here}/tftp" -force -recurse
mkdir "${here}/tftp"

net use "$($conf.ThinClientShare)" "$($conf.ThinClientPwd)" /user:"$($conf.ThinClientUser)" /persistent:no
cp "$($conf.ThinClientShare)\${fileFilter}" "${here}\tftp"
net use "$($conf.ThinClientShare)" /delete
