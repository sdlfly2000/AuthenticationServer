# AngularPresentation
## Host on Nginx -> configure file (/etc/nginx/confi.d/AuthenticationService.conf)
'''

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

'''
## link to AngularPresentation in Project
'''

sudo ln -s /home/sdlfly2000/Projects/AuthenticationService/AngularPresentation /var/www/AngularPresentation

'''
### Assig Permission to folders
'''

sudo chmod 755 -R /home/sdlfly2000/Projects/AuthenticationService/AngularPresentation
sudo chmod 755 -R /var/www/AngularPresentation

'''

# AuthService
## Move AuthService.service to /etc/systemd/system/
'''

sudo cp ~/Projects/AuthenticationService/AuthService.service /etc/systemd/system/

'''


