#ifndef VOGEL_H
#define VOGEL_H
#include "Lebewesen.h"
#include "string"
using namespace std;
class Vogel: public Lebewesen{
private:
     string _Bezeichnung;
     int _AktuellerSaettigungsgrad;
     int _MaximalerSaettigungsgrad;
     int _Flugenergieverbrauch; // in kJ pro Flugeinheit
public:
     void Fressen() override;
     void GefressenWerden() override;
     Vogel(std::string _bezeichnung, int _maximalerSaettigungsgrad, int _flugenergieverbrauch);
     void Fliegen();
     void FutterAufnehmen(int menge);
};
inline Vogel::Vogel(std::string _bezeichnung, int _maximalerSaettigungsgrad, int _flugenergieverbrauch) :
    _Bezeichnung(_bezeichnung), _MaximalerSaettigungsgrad(_maximalerSaettigungsgrad), _Flugenergieverbrauch(_flugenergieverbrauch) {
    _AktuellerSaettigungsgrad = 0;
}
inline void Vogel::Fressen() {
     std::cout << "Der Vogel frisst Insekten." << std::endl;
}

inline void Vogel::GefressenWerden() {
     std::cout << "Der Vogel wird von einem anderen Tier gefressen." << std::endl;
}
inline void Vogel::Fliegen() {
    if (_AktuellerSaettigungsgrad > 0) {
        _AktuellerSaettigungsgrad -= _Flugenergieverbrauch;
        std::cout << "Der Vogel fliegt und verbraucht Energie." << std::endl;
    } else {
        std::cout << "Der Vogel hat nicht genug Energie zum Fliegen." << std::endl;
    }
}

inline void Vogel::FutterAufnehmen(int menge) {
    _AktuellerSaettigungsgrad += menge;
    if (_AktuellerSaettigungsgrad > _MaximalerSaettigungsgrad)
        _AktuellerSaettigungsgrad = _MaximalerSaettigungsgrad;
}

#endif // VOGEL_H
