#!/bin/bash

# Backup && Log Pfad
BACKUP_DIR="/var/www/html"
LOG_FILE="/var/log/backup.log"

# Protokollierung von Nachrichten
log() {
  local log_message="$1"
  echo "$(date "+%Y-%m-%d %H:%M:%S"): $log_message" >> "$LOG_FILE"
}

# Führe das Backup durch
backup() {
  local backup_file="$BACKUP_DIR/backup_$(date +%Y%m%d).tar.gz"
  log "Backup wird gestartet..."

  tar -czf "$backup_file" "$BACKUP_DIR"

  if [ $? -eq 0 ]; then
    log "Backup erfolgreich abgeschlossen. Backup-Datei: $backup_file"
  else
    log "Fehler beim Backup!"
  fi
}

# Hauptprogramm
backup
