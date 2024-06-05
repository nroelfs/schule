#!/bin/bash

# Aktualisiere die Paketliste
apt update

# Installiere die deutschen Sprachpakete
apt install -y language-pack-de
update-locale LANG=de_DE.UTF-8 LC_MESSAGES=POSIX

# Konfiguriere die Zeitzone
timedatectl set-timezone Europe/Berlin

# Installation dotnet
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh #Downlaod .Net
chmod +x ./dotnet-install.sh #Vergiebt die berechtigung um das Script zu starten
./dotnet-install.sh --channel 6.0 #Installiert die Version 6.0
./dotnet-install.sh --channel 6.0   --runtime aspnetcore
rm dotnet-install.sh #Entfernt das script

# richtigere Installation Dotnet 6.0
sudo apt-get update &&   sudo apt-get install -y dotnet-sdk-6.0
sudo apt-get update &&   sudo apt-get install -y aspnetcore-runtime-6.0

# Installiere SSH-Server
apt install -y openssh-server

# Installiere Nginx
sudo apt install -y nginx


# Installiere MySQL-Server
apt install -y mysql-server

# MySQL-Root-Passwort setzen
echo "mysql-server mysql-server/root_password password schule" | debconf-set-selections
echo "mysql-server mysql-server/root_password_again password schule" | debconf-set-selections



# Erstelle einen MySQL-Benutzer
mysql -u schule -p schule -e "CREATE USER 'Service'@'localhost' IDENTIFIED BY 'Emden123';"
mysql -u schule -p schule -e "GRANT ALL PRIVILEGES ON *.* TO 'Service'@'localhost' WITH GRANT OPTION;"
mysql -u schule -p schule -e "FLUSH PRIVILEGES;"

# Installiere phpMyAdmin
apt install -y phpmyadmin

# Konfiguriere phpMyAdmin
echo "phpmyadmin phpmyadmin/dbconfig-install boolean true" | debconf-set-selections
echo "phpmyadmin phpmyadmin/app-password-confirm password schule" | debconf-set-selections
echo "schule"
echo "phpmyadmin phpmyadmin/mysql/admin-pass password schule" | debconf-set-selections
echo "schule"
echo "phpmyadmin phpmyadmin/mysql/app-pass password schule" | debconf-set-selections
echo "schule"
echo "phpmyadmin phpmyadmin/reconfigure-webserver multiselect nginx" | debconf-set-selections

# Konfiguriere Nginx für phpMyAdmin
sudo tee /etc/nginx/sites-available/phpmyadmin <<EOF
server {
        listen 80;
        #root /var/www/html;

        server_name github_web;

        location / {
                # First atemmpt to server request as file, then
                # as directory, then fall back to displaying a 404
                # try_files $uri $uri/ /index.html:
                proxy_pass http://0.0.0.0:5000;
                proxy_http_version 1.1;
                proxy_set_header Upgrade $http_upgrade;
                proxy_set_header Connection keep-alive;
                proxy_set_header Host $host;
                proxy_cache_bypass $http_upgrade;
                    } 
        
        }
EOF


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
