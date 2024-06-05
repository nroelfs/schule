#!/bin/bash

# Statische IP-Adresse und Netzwerkinformationen
STATIC_IP="172.21.93.25"  # Setzen Sie Ihre gewünschte statische IP-Adresse hier ein
NETMASK="255.255.255.0"
GATEWAY="172.21.93.1"  # Setzen Sie Ihr Gateway ein
DNS_SERVER="8.8.8.8"  # Setzen Sie Ihren bevorzugten DNS-Server ein

# Netzwerkschnittstelle (normalerweise eth0 oder ens33)
INTERFACE="eth0"  # Stellen Sie sicher, dass Sie die richtige Schnittstelle verwenden

# Aktuelles Netzwerkkonfigurationsdatei-Backup erstellen
cp /etc/netplan/01-netcfg.yaml /etc/netplan/01-netcfg.yaml.backup

# Netzwerkkonfigurationsdatei ändern
cat > /etc/netplan/01-netcfg.yaml <<EOF
network:
  version: 2
  renderer: networkd
  ethernets:
    $INTERFACE:
      addresses:
        - $STATIC_IP/24
      gateway4: $GATEWAY
      nameservers:
        addresses:
          - $DNS_SERVER
EOF

# Netzwerkdienste neu konfigurieren
netplan apply

# DHCP deaktivieren
systemctl stop systemd-networkd
systemctl disable systemd-networkd

# Netzwerkdienste starten
systemctl start systemd-networkd
systemctl enable systemd-networkd

# Abschlussmeldung
echo "Statische IP-Adresse $STATIC_IP wurde konfiguriert, und DHCP wurde deaktiviert."

# Optional: Server neu starten, um die Änderungen zu übernehmen
