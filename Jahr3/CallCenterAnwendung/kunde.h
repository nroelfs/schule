#ifndef KUNDE_H
#define KUNDE_H

#include <QObject>
#include <QJsonObject>

class Kunde
{
public:
    Kunde(QString vorname, QString nachname, QString tel);
    QString GetVorname() const;
    QString GetNachname() const;
    QString GetTel() const;
    QJsonObject toQJsonObject() const;

private:
    QString vorname;
    QString nachname;
    QString tel;
};

#endif // KUNDE_H
