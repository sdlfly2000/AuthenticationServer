# AngularPresentation
## Host on Nginx -> configure file (/etc/nginx/confi.d/AuthenticationService.conf)

HTTP
```

server {
  listen 0.0.0.0:4201;
  location / {
    root /var/www/AngularPresentation/;
    index index.php index.html index.htm;
    try_files $uri $uri/ /index.html;
  }

  location /api/ {
    add_header Access-Control-Allow-Origin '*';
    add_header Access-Control-Allow-Headers '*';
    add_header Access-Control-Allow-Methods '*';
    add_header Access-Control-Allow-Credentials 'true';
    if ($request_method = 'OPTIONS'){
      return 204;
    }
    proxy_pass http://127.0.0.1:4202/api/;
  }
}
```

HTTPS

```
server {
  listen 443 ssl;
  ssl_protocols  TLSv1 TLSv1.1 TLSv1.2;
  ssl_ciphers  AES128-SHA:AES256-SHA:RC4-SHA:DES-CBC3-SHA:RC4-MD5;
  ssl_certificate /home/sdlfly2000/Projects/AuthenticationService/AuthenticationServiceCert.pem;
  ssl_certificate_key /home/sdlfly2000/Projects/AuthenticationService/AuthenticationServiceCert.key;
  ssl_session_cache  shared:SSL:10m;
  ssl_session_timeout  10m;

  location / {
    proxy_pass http://127.0.0.1:4202;
  }
}

```


## link to AngularPresentation in Project
```
sudo ln -s /home/sdlfly2000/Projects/AuthenticationService/AngularPresentation /var/www/AngularPresentation
```

### Assign Permission to folders
```
sudo chmod 755 -R /home/sdlfly2000/Projects/AuthenticationService/AngularPresentation
sudo chmod 755 -R /var/www/AngularPresentation
```


# AuthService
## Move AuthService.service to /etc/systemd/system/
```
sudo cp ~/Projects/AuthenticationService/AuthService.service /etc/systemd/system/
```

# Create Certificate for HTTPS
 [Ref Link](https://cloud.tencent.com/developer/article/1813403)

```
openssl genrsa -des3 -out AuthenticationServiceCert.key 2048
openssl req -new -key AuthenticationServiceCert.key -out AuthenticationServiceCert.csr
openssl rsa -in AuthenticationServiceCert.key -out AuthenticationServiceCert.key
openssl x509 -req -days 365 -in AuthenticationServiceCert.csr -signkey AuthenticationServiceCert.key -out AuthenticationServiceCert.crt
openssl x509 -in AuthenticationServiceCert.crt -out AuthenticationServiceCert.pem -outform PEM
```

# Add Migrate to Database
In VS Package Manager Console
```
Add-Migration "comments"
Update-Database
```

```
dotnet ef database update --project .\4-Infrastructure\Infra.Database\Infra.Database.csproj --startup-project .\1-Presentation\AuthService\AuthService.csproj
```
Note: 
1. AuthService should be as default startup project
2. Default Project should be Infra.Database
