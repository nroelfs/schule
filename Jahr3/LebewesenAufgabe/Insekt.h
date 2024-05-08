#ifndef INSEKT_H
#define INSEKT_H
#include "Lebewesen.h"
#include "string"
using namespace std;
class Insekt: public Lebewesen{
private:
    string _Bezeichnung;
    int _Energiegehalt;
public:
    void Fressen() override;
    void GefressenWerden()override;
    Insekt(string _bezeichnung, int _energiegehalt);

};

inline Insekt::Insekt(string _bezeichnung, int _energiegehalt) : _Bezeichnung(_bezeichnung), _Energiegehalt(_energiegehalt) {}

inline void Insekt::Fressen() {
    cout << "Das Insekt frisst etwas." << std::endl;
}

inline void Insekt::GefressenWerden(){
    cout << "Das Insekt wird gefressen." << std::endl;
}
#endif // INSEKT_H
