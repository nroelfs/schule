#ifndef KUNDE_H
#define KUNDE_H

#include <QObject>

class Kunde
{
public:
    Kunde(QString,QString,QString);
    QString GetVorname();
    QString GetNachname();
    QString GetTel();
private:
    QString vorname;
    QString nachname;
    QString tel;
};

#endif // KUNDE_H
