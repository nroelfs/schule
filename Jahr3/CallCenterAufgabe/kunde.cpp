#include "kunde.h"

Kunde::Kunde(QString Vorname, QString Nachname, QString tel)
{
    this->vorname = Vorname;
    this->nachname = Nachname;
    this->tel = tel;
}

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
void Kunde::fromQJsonObject(const QJsonObject& jsonObj) {
    this->vorname = jsonObj["vorname"].toString();
    this->nachname = jsonObj["nachname"].toString();
    this->tel = jsonObj["tel"].toString();
}
bool Kunde::operator==(const Kunde& andererKunde) const{
    return vorname == andererKunde.vorname &&
            nachname == andererKunde.nachname &&
           tel == andererKunde.tel;
}
