:: Generate a RSA-2048 key and save it to a file rootCA.key. This file will be used as the key to generate the Root SSL certificate
openssl genrsa -out "rootCA.key" 2048
:: You can use the key you generated to create a new Root SSL certificate. Save it to a file named rootCA.pem
openssl req -x509 -new -nodes -key "rootCA.key" -sha256 -days 2048 -out "rootCA.pem" -subj "/CN=northwind.com/O=northwind/C=PE/ST=LIMA/L=LIMA/emailAddress=support@northwind.com"
:: Create a certificate key for localhost using the configuration settings stored in server.csr.cnf. This key is stored in server.key.
openssl req -new -sha256 -nodes -out "server.csr" -newkey rsa:2048 -keyout "server.key" -config "server.conf" -subj "/CN=northwind.com/O=northwind/C=PE/ST=LIMA/L=LIMA/emailAddress=support@northwind.com"
:: A certificate signing request is issued via the root SSL certificate we created earlier to create a domain certificate for localhost. 
openssl x509 -req -in "server.csr" -CA "rootCA.pem" -CAkey "rootCA.key" -CAcreateserial -out "server.crt" -days 500 -sha256 -extfile "v3.ext.conf"

openssl pkcs12 -export -nodes -out "server.pfx" -inkey "server.key" -in "server.crt" -passout pass:


