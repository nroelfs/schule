#include "kunde.h"

Kunde::Kunde(QString vorname, QString nachname, QString tel)
    : vorname(vorname), nachname(nachname), tel(tel) {}

QString Kunde::GetVorname() const { return vorname; }
QString Kunde::GetNachname() const { return nachname; }
QString Kunde::GetTel() const { return tel; }

QJsonObject Kunde::toQJsonObject() const {
    QJsonObject jsonObj;
    jsonObj["vorname"] = vorname;
    jsonObj["nachname"] = nachname;
    jsonObj["tel"] = tel;
    return jsonObj;
}
