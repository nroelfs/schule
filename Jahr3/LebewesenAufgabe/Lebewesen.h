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

    int Alter() const;
    void setAlter(int newAlter);
    int Lebenserwartung() const;
    void setLebenserwartung(int newLebenserwartung);
    bool Lebendig() const;
    void setLebendig(bool newLebendig);
    Lebewesen();
};
inline bool Lebewesen::Lebendig() const
{
    return _Lebendig;
}

inline void Lebewesen::setLebendig(bool newLebendig)
{
    _Lebendig = newLebendig;
}

inline Lebewesen::Lebewesen()
{
    _Lebendig = true;
    _Alter = 0;
    _Lebenserwartung = rand();// % 30 + 1; //für 1 bis 30 Jahre
}

inline int Lebewesen::Alter() const
{
    return _Alter;
}

inline void Lebewesen::setAlter(int newAlter)
{
    _Alter = newAlter;
}

inline int Lebewesen::Lebenserwartung() const
{
    return _Lebenserwartung;
}

inline void Lebewesen::setLebenserwartung(int newLebenserwartung)
{
    _Lebenserwartung = newLebenserwartung;
}

#endif // LEBEWESEN_H
