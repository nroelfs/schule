#!/bin/bash

#This script is used to install and configure various components for a web server setup.
#It performs the following tasks:

#Updates the system packages
#Installs the Raspberry Pi UI mods
#Sets the system locale and timezone
#Installs the .NET SDK and extracts the appropriate app files based on the system architecture
#Configures the user profiles to include the .NET SDK in the PATH
#Installs and configures SSH server, Nginx, and PHP
#Sets up and configures MySQL server
#Configures Nginx to proxy requests to the backend server
#Sets up a systemd service for the backend server
#Installs and configures phpMyAdmin
#Configures and enables UFW firewall
#Starts the necessary services and enables them to start on boot
#Removes Apache2 if present
#Reboots the system

sudo apt-get install nmap -y
#Sets the system locale and timezone"
if nmap -p 123 -sU -oG - pool.ntp.org | grep -q 123/closed; then
    sudo timedatectl set-ntp false
    echo "[INFO] NTP port is blocked. Please enter the current date and time (YYYY-MM-DD HH:MM:SS):"
    read -p "Date and Time: " current_datetime
    sudo timedatectl set-time "$current_datetime"
else
    echo "[INFO] NTP port is open. Setting time automatically."
fi

#user=$SUDO_USER
apt update -y
sudo apt install raspberrypi-ui-mods -y
update-locale LANG=de_DE.UTF-8 LC_MESSAGES=POSIX
timedatectl set-timezone Europe/Berlin




#Installs the .NET SDK and extracts the appropriate app files based on the system architecture

sudo apt install wget -y
sudo mkdir /usr/share/dotnet
sudo mkdir -p /home/$SUDO_USER/backend
sudo mkdir -p /home/$SUDO_USER/backup

if getconf LONG_BIT == 32; then
    wget https://download.visualstudio.microsoft.com/download/pr/a72dea03-21fd-48c6-bf0c-78e621b60514/e0b8f186730fce858eb1bffc83c9e41c/dotnet-sdk-6.0.417-linux-arm.tar.gz
    sudo tar zxf dotnet-sdk-6.0.417-linux-arm.tar.gz -C /usr/share/dotnet/
    sudo unzip /home/$SUDO_USER/2023_Webserver-Projekt_GruppeA/Skripte/sammlungV2.1/App32Bit.zip -d /home/$SUDO_USER/backend 
else
    wget https://download.visualstudio.microsoft.com/download/pr/03972b46-ddcd-4529-b8e0-df5c1264cd98/285a1f545020e3ddc47d15cf95ca7a33/dotnet-sdk-6.0.417-linux-arm64.tar.gz;
    sudo tar zxf dotnet-sdk-6.0.417-linux-arm64.tar.gz -C /usr/share/dotnet/
    sudo unzip /home/$SUDO_USER/2023_Webserver-Projekt_GruppeA/Skripte/sammlungV2.1/App64Bit.zip -d /home/$SUDO_USER/backend 
fi

#Configures the user profiles to include the .NET SDK in the PATH
code="\nif [ -n \"\$BASH_VERSION\" ]; then
    if [ -f \"\$HOME/.bashrc\" ]; then
        . \"\$HOME/.bashrc\"
    fi
fi
if [ -d \"\$HOME/bin\" ] ; then
    PATH=\"\$HOME/bin:\$PATH\"
fi
if [ -d \"\$HOME/.local/bin\" ] ; then
    PATH=\"\$HOME/.local/bin:\$PATH\"
fi
export PATH=\$PATH:/usr/share/dotnet
export DOTNET_ROOT=/usr/share/dotnet"

for user_home in /home/*; do
    profile_file="$user_home/.profile"
    if [ -f "$profile_file" ]; then
        echo -e "$code" >> "$profile_file"
    else
        echo "[FAILED] -  no .profile file found ==> Skipping user: $(basename $user_home) "
    fi
done

#Installs and configures SSH server, Nginx, and PHP
apt install -y openssh-server

apt install -y nginx
#sudo apt update
sudo apt install -y apt-transport-https lsb-release ca-certificates
sudo apt install -y lsb-release apt-transport-https ca-certificates
sudo wget -O /etc/apt/trusted.gpg.d/php.gpg https://packages.sury.org/php/apt.gpg
sudo sh -c 'echo "deb https://packages.sury.org/php/ $(lsb_release -sc) main" > /etc/apt/sources.list.d/php.list'
sudo apt install -y php8.2-fpm
sudo mv /etc/nginx/sites-available/default /etc/nginx/sites-available/default.backup

#Sets up and configures MySQL server
MYSQL_ROOT_PASSWORD =  "schule"
sudo apt install -y mariadb-server

sudo mysql -u root  -e "CREATE DATABASE projektgruppea;" 
sudo mysql -u root  -e "CREATE USER 'Service'@'localhost' IDENTIFIED BY 'Emden123';"
sudo mysql -u root  -e "GRANT ALL PRIVILEGES ON *.* TO 'Service'@'localhost' WITH GRANT OPTION;"
sudo mysql -u root  -e "FLUSH PRIVILEGES;"
sudo mysql -u root  -e "UPDATE mysql.user SET plugin = 'mysql_native_password' WHERE User = 'root';"
sudo mysql -u root  -e "DELETE FROM mysql.user WHERE User='root' AND Host NOT IN ('localhost', '127.0.0.1', '::1');"
sudo mysql -u root  -e "DELETE FROM mysql.user WHERE User='';"
sudo mysql -u root  -e "DELETE FROM mysql.db WHERE Db='test' OR Db='test\_%';"
sudo mysql -u root  -e "FLUSH PRIVILEGES;"

echo "mysql-server mysql-server/root_password password $MYSQL_ROOT_PASSWORD" | sudo debconf-set-selections
echo "mysql-server mysql-server/root_password_again password $MYSQL_ROOT_PASSWORD" | sudo debconf-set-selections

#Configures Nginx to proxy requests to the backend server
sudo tee /etc/nginx/sites-available/default <<EOF
server {
        listen 80;
        server_name github_web;
        location / {
                proxy_pass http://0.0.0.0:5000;
                proxy_http_version 1.1;
                proxy_set_header Upgrade \$http_upgrade;
                proxy_set_header Connection keep-alive;
                proxy_set_header Host \$host;
                proxy_cache_bypass \$http_upgrade;
        }
        include snippets/phpmyadmin.conf;
}
EOF

sudo tee /etc/nginx/snippets/phpmyadmin.conf <<EOF
location /phpmyadmin {
    root /usr/share/;
    index index.php index.html index.htm;
    location ~ ^/phpmyadmin/(.+\.php)$ {
        try_files \$uri =404;
        root /usr/share/;
        fastcgi_pass unix:/run/php/php8.2-fpm.sock;
        fastcgi_index index.php;
        fastcgi_param SCRIPT_FILENAME \$document_root\$fastcgi_script_name;
        include /etc/nginx/fastcgi_params;
    }

    location ~* ^/phpmyadmin/(.+\.(jpg|jpeg|gif|css|png|js|ico|html|xml|txt))$ {
        root /usr/share/;
    }
}
EOF

#Sets up a systemd service for the backend server
sudo tee /etc/systemd/system/backend.service <<EOF
#backend.service -File
#Dir: /etc/systemd/system/

[Unit]
Description=Projekt Backend

[Service]
WorkingDirectory=/home/$SUDO_USER/backend/
ExecStart=/usr/share/dotnet/dotnet /home/$SUDO_USER/backend/ServerAppSchule.dll --urls http://0.0.0.0:5000
Restart=always
# Restart service after 10 seconds if the dotnet service crashes:
RestartSec=10
KillSignal=SIGINT
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
EOF

#Installs and configures phpMyAdmin
echo "phpmyadmin phpmyadmin/dbconfig-install boolean true" | sudo debconf-set-selections
echo "phpmyadmin phpmyadmin/app-password-confirm password $MYSQL_ROOT_PASSWORD" | sudo debconf-set-selections
echo "phpmyadmin phpmyadmin/mysql/admin-pass password $MYSQL_ROOT_PASSWORD" | sudo debconf-set-selections
echo "phpmyadmin phpmyadmin/mysql/app-pass password $MYSQL_ROOT_PASSWORD" | sudo debconf-set-selections
echo "phpmyadmin phpmyadmin/reconfigure-webserver multiselect nginx" | sudo debconf-set-selections

sudo DEBIAN_FRONTEND=noninteractive apt-get install -y phpmyadmin

#Configures and enables UFW firewall
sudo apt-get install ufw -y
sudo ufw default deny incoming
sudo ufw default allow outgoing
sudo ufw allow OpenSSH
sudo ufw allow 'Nginx Full'
sudo ufw allow mysql
sudo ufw enable 

ufw enable

#Starts the necessary services and enables them to start on boot
systemctl enable backend 
systemctl enable ssh
systemctl enable nginx
systemctl enable mysql

systemctl start ssh
systemctl start backend
systemctl start nginx
systemctl start mysql

systemctl status nginx
systemctl status mysql
systemctl status ssh
systemctl status backend

#Removes Apache2 if present
sudo apt-get purge apache2 apache2-utils apache2.2-bin apache2-common -y
sudo rm -rf /etc/apache2/ -y
sudo rm -rf /var/log/apache2/ -y

#Reboots the system
sudo reboot -h now
