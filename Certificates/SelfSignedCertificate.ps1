#http://woshub.com/how-to-create-self-signed-certificate-with-powershell/
#https://gist.github.com/RomelSan/bea2443684aa0883b117c37bac1de520
#$cert = New-SelfSignedCertificate -certstorelocation cert:\localmachine\my -dnsname "localhost","127.0.0.1","192.168.1.1" -subject "CN=Northwind" -keyexportpolicy Exportable
#$pwd = ConvertTo-SecureString -String 'p@ssw0rd' -Force -AsPlainText
#$path = 'cert:\localMachine\my\' + $cert.thumbprint
#Export-PfxCertificate -cert $path -FilePath Certificates\Northwind.pfx -Password $pwd
#Export-Certificate -Cert $cert -FilePath Certificates\Northwind.cer
#Export-Certificate -Cert $cert -FilePath Certificates\Northwind.p7b -Type p7b

#-------------------------------------------------------------------------------------
# Create Self signed root certificate
# -dnsname -DnsName domain.example.com,anothersubdomain.example.com
# -Subject "CN=Patti Fuller,OU=UserAccounts,DC=corp,DC=contoso,DC=com" 
$cert = New-SelfSignedCertificate -Type Custom -KeySpec Signature `
-Subject "CN=NorthwindRootCert" `
-KeyExportPolicy Exportable `
-HashAlgorithm sha256 -KeyLength 4096 `
-CertStoreLocation "Cert:\CurrentUser\My" `
-KeyUsageProperty Sign `
-KeyUsage CertSign `
-NotAfter (Get-Date).AddYears(5)

# Generate certificates from root (For Client Authentication only) (Not for web server)
New-SelfSignedCertificate -Type Custom -KeySpec Signature `
-Subject "CN=NorthwindChildCertClient" -KeyExportPolicy Exportable `
-HashAlgorithm sha256 -KeyLength 2048 `
-NotAfter (Get-Date).AddMonths(24) `
-CertStoreLocation "Cert:\CurrentUser\My" `
-Signer $cert -TextExtension @("2.5.29.37={text}1.3.6.1.5.5.7.3.2")

# Generate certificate from root for web service
New-SelfSignedCertificate -Type Custom `
-Subject "CN=NorthwindChildCertWeb" -KeyExportPolicy Exportable `
-DnsName "localhost","localhost/Northwind.WebRole" `
-HashAlgorithm sha256 -KeyLength 2048 `
-KeyUsage "KeyEncipherment", "DigitalSignature" `
-NotAfter (Get-Date).AddMonths(24) `
-CertStoreLocation "Cert:\CurrentUser\My" `
-Signer $cert

# Export as PFX
$PFXPass = ConvertTo-SecureString -String "p@ssw0rd" -Force -AsPlainText
$path = 'cert:\CurrentUser\My\' + $cert.thumbprint
Export-PfxCertificate -Cert $path -Password $PFXPass -FilePath Certificates\Northwind.pfx

# Export Normal
# Exports a certificate to the file system as a DER-encoded .cer file without its private key.
Export-Certificate -Cert $cert -FilePath Certificates\Northwind.cer

# Exports a certificate to the file system as a PKCS#7-fomatted .p7b file without its private key.
Export-Certificate -Cert $cert -FilePath Certificates\Northwind.p7b -Type p7b

