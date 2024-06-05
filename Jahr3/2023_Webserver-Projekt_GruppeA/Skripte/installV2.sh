#!/bin/bash

# Aktualisiere die Paketliste
apt update

# Installiere die deutschen Sprachpakete
sudo apt install raspberrypi-ui-mods
update-locale LANG=de_DE.UTF-8 LC_MESSAGES=POSIX

# Konfiguriere die Zeitzone
timedatectl set-timezone Europe/Berlin

# Installation dotnet
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y apt-transport-https
sudo apt-get update
sudo apt-get install -y dotnet-sdk-6.0

# Überprüfe die Installation
dotnet --version
sudo rm packages-microsoft-prod.deb


#Passwort EIngabe (YES)
# Installiere SSH-Server
#Command may disrupt existing ssh connections. Proceed with operation (y|n)?
apt install -y openssh-server

# Installiere Nginx
apt install -y nginx
sudo apt update
sudo apt install -y apt-transport-https lsb-release ca-certificates
sudo apt install -y lsb-release apt-transport-https ca-certificates
sudo wget -O /etc/apt/trusted.gpg.d/php.gpg https://packages.sury.org/php/apt.gpg
sudo sh -c 'echo "deb https://packages.sury.org/php/ $(lsb_release -sc) main" > /etc/apt/sources.list.d/php.list'
sudo apt install php8.1 php8.1-fpm php8.1-mysql php8.1-xml php8.1-curl php8.1-gd -y
sudo mv /etc/nginx/sites-available/default /etc/nginx/sites-available/default.backup

# Installiere MySQL-Server 
sudo apt install -y mariadb-server

# MySQL-Root-Passwort setzen
echo "mysql-server mysql-server/root_password password schule" | sudo debconf-set-selections
echo "mysql-server mysql-server/root_password_again password schule" | sudo debconf-set-selections



# Erstelle einen MySQL-Benutzer ----- warum apache ordner 
mysql -u root -p$github-e "CREATE USER 'Service'@'localhost' IDENTIFIED BY 'Emden123';"
mysql -u root -p$github -e "GRANT ALL PRIVILEGES ON *.* TO 'Service'@'localhost' WITH GRANT OPTION;"
mysql -u root -p$github -e "FLUSH PRIVILEGES;"
#Passwort absprache überspringen

# Konfiguriere Nginx für phpMyAdmin ----config anpassen + fehlende ergänzen serivce snippets
#default config nginx
sudo tee /etc/nginx/sites-available/default <<EOF
server {
        listen 80;
        server_name github_web;
        location / {
                proxy_pass http://0.0.0.0:5000;
                proxy_http_version 1.1;
                proxy_set_header Upgrade $http_upgrade;
                proxy_set_header Connection keep-alive;
                proxy_set_header Host $host;
                proxy_cache_bypass $http_upgrade;
        }
        include snippets/phpmyadmin.conf;
}
EOF
#phpmyadmin config für reverse Proxy
sudo tee /etc/nginx/snippets/phpmyadmin.conf <<EOF
location /phpmyadmin {
    root /usr/share/;
    index index.php index.html index.htm;
    location ~ ^/phpmyadmin/(.+\.php)$ {
        try_files $uri =404;
        root /usr/share/;
        fastcgi_pass unix:/run/php/php8.1-fpm.sock;
        fastcgi_index index.php;
        fastcgi_param SCRIPT_FILENAME $document_root$fastcgi_script_name;
        include /etc/nginx/fastcgi_params;
    }

    location ~* ^/phpmyadmin/(.+\.(jpg|jpeg|gif|css|png|js|ico|html|xml|txt))$ {
        root /usr/share/;
    }
}
EOF

#backend service config ---- methode zum dynamischen ordner pfad ermitteln für :  /home/github/backend/ServerAppSchule.dll ergänzen sudo find / -type f -name "ServerAppSchule.dll" -print
sudo tee /etc/systemd/system/backend.service <<EOF
#backend.service -File
#Dir: /etc/systemd/system/

[Unit]
Description=Projekt Backend

[Service]
WorkingDirectory=/home/github/backend/
ExecStart=/usr/bin/dotnet /home/github/backend/ServerAppSchule.dll --urls http://0.0.0.0:5000
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
EOF

#für auto start
systemctl enable backend 
systemctl start backend


# Konfiguriere phpMyAdmin
echo "phpmyadmin phpmyadmin/dbconfig-install boolean true" | sudo debconf-set-selections
echo "phpmyadmin phpmyadmin/app-password-confirm password github" | sudo debconf-set-selections
echo "phpmyadmin phpmyadmin/mysql/admin-pass password github" | sudo debconf-set-selections
echo "phpmyadmin phpmyadmin/mysql/app-pass password github" | sudo debconf-set-selections
echo "phpmyadmin phpmyadmin/reconfigure-webserver multiselect nginx" | sudo debconf-set-selections

# Starte die Dienste
systemctl start nginx
systemctl start mysql
# Aktiviere die Dienste, um sie beim Start automatisch zu starten
systemctl enable nginx
systemctl enable mysql

# Überprüfe den Status von Nginx
systemctl status nginx
systemctl status mysql


# Öffne die benötigten Ports in der Firewall
ufw allow OpenSSH
ufw allow "nginx Full"
ufw allow mysql

# Aktiviere die Firewall
ufw enable

# Zeige eine Erfolgsmeldung
echo "Konfiguration abgeschlossen. Du kannst jetzt auf deinen Server zugreifen."
