#!/bin/bash

# MySQL/MariaDB Verbindungsinformationen
DB_HOST="localhost"
DB_USER="Service'@'localhost"
DB_PASSWORD="Emden123"
DB_NAME="projektgruppea"

# Verzeichnis zum Speichern der Backup-Dateien
BACKUP_DIR="/home/$SUDO_USER/backup"

# Datum und Zeit für den Dateinamen
DATE=$(date +"%Y%m%d%H%M%S")

# Name der Backup-Datei
BACKUP_FILE="$BACKUP_DIR/$DB_NAME-$DATE.sql.gz"

# MySQL-Dump durchführen und mit gzip komprimieren
mysqldump -h $DB_HOST -u $DB_USER -p$DB_PASSWORD $DB_NAME | gzip > $BACKUP_FILE

# Überprüfen, ob das Backup erfolgreich war
if [ $? -eq 0 ]; then
  echo "Datenbank-Backup erfolgreich erstellt: $BACKUP_FILE"
else
  echo "Fehler beim Erstellen des Datenbank-Backups"
fi