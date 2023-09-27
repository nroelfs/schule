#ifndef KUNDE_H
#define KUNDE_H

#include <QObject>
#include <QJsonObject>

class Kunde
{
public:
    Kunde(){}
    Kunde(QString , QString , QString );
    QString GetVorname() const;
    QString GetNachname() const;
    QString GetTel() const;
    QJsonObject toQJsonObject() const;
    void fromQJsonObject(const QJsonObject&);
    bool operator==(const Kunde&)const;
    bool operator!=(const Kunde&)const;
private:
    QString vorname;
    QString nachname;
    QString tel;
};

#endif // KUNDE_H
