#include "kunde.h"
#include <QString>

Kunde::Kunde(QString vorname, QString Nachname, QString tel)
{
    this->vorname = vorname;
    this->nachname = nachname;
    this->tel = tel;
}

QString Kunde::GetVorname()
{
    return vorname;
}

QString Kunde::GetNachname()
{
    return nachname;
}

QString Kunde::GetTel()
{
    return tel;
}

