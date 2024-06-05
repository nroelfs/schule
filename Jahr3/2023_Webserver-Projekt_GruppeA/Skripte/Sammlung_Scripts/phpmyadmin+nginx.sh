#!/bin/bash

# Aktualisiere die Paketliste
sudo apt update

# Installiere phpMyAdmin und MySQL-Client
sudo apt install -y phpmyadmin mysql-client

# Wähle die Installationsoption für Nginx
sudo apt install -y nginx

# Setze die Berechtigungen für den Webserver
sudo chown -R www-data:www-data /var/www/html

# Konfiguriere Nginx für phpMyAdmin
sudo tee /etc/nginx/sites-available/phpmyadmin <<EOF
server {
    listen 80;
    server_name github;

    root /usr/share/nginx/html;
    index index.php index.html index.htm;

    location / {
        try_files \$uri \$uri/ =404;
    }

    location ~ \.php$ {
        include snippets/fastcgi-php.conf;
        fastcgi_pass unix:/var/run/php/php7.4-fpm.sock;
        fastcgi_param SCRIPT_FILENAME \$document_root\$fastcgi_script_name;
        include fastcgi_params;
    }

    location ~ /\.ht {
        deny all;
    }

    location /phpmyadmin {
        rewrite ^/* /phpmyadmin last;
    }

    location ~ ^/phpmyadmin/.*\.php$ {
        fastcgi_param PMA_ABSOLUTE_URI /phpmyadmin;
        alias /usr/share/phpmyadmin\$fastcgi_script_name;
        fastcgi_pass unix:/var/run/php/php7.4-fpm.sock;
        include fastcgi_params;
        fastcgi_param SCRIPT_FILENAME \$document_root\$fastcgi_script_name;
        include snippets/fastcgi-php.conf;
    }
}
EOF

# Erstelle einen symbolischen Link für die Nginx-Konfiguration
sudo ln -s /etc/nginx/sites-available/phpmyadmin /etc/nginx/sites-enabled/

# Entferne die Standard-Nginx-Konfiguration, falls vorhanden
sudo rm /etc/nginx/sites-enabled/default

# Überprüfe die Nginx-Konfiguration
sudo nginx -t

# Starte den Nginx-Dienst neu
sudo systemctl restart nginx

# Öffne die benötigten Ports in der Firewall
sudo ufw allow 'Nginx Full'

# Aktiviere die Firewall
sudo ufw enable

# Deaktiviere den Remotezugriff für den Root-Benutzer in MySQL
sudo mysql -e "ALTER USER 'root'@'localhost' IDENTIFIED WITH mysql_native_password BY '';"
sudo mysql -e "FLUSH PRIVILEGES;"

# Zeige eine Erfolgsmeldung
echo "Konfiguration abgeschlossen. Du kannst jetzt auf phpMyAdmin über http://phpmyadmin.example.com zugreifen."
