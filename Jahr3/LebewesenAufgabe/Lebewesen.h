#ifndef LEBEWESEN_H
#define LEBEWESEN_H
#include <iostream>
#include <cstdlib>
#include <ctime>
using namespace std;
class Lebewesen{
protected:
    int _Alter;
    int _Lebenserwartung;
    bool _Lebendig;
public:
    virtual void Fressen() = 0;
    virtual void GefressenWerden() = 0;

    Lebewesen();
};


inline Lebewesen::Lebewesen()
{
    _Lebendig = true;
    _Alter = 0;
    _Lebenserwartung = rand();// % 30 + 1; //für 1 bis 30 Jahre
}

#endif // LEBEWESEN_H
