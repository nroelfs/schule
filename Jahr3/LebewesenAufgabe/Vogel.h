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
     //int _Flugenergieverbrauch; // in kJ pro Flugeinheit
public:
     void Fressen() override;
     void Fressen(int energiegehalt);
     void GefressenWerden() override;
     Vogel(string _bezeichnung, int _maximalerSaettigungsgrad/*, int _flugenergieverbrauch*/);
     void Fliegen();
     void FutterAufnehmen(int menge);
};
inline Vogel::Vogel(string _bezeichnung, int _maximalerSaettigungsgrad/*, int _flugenergieverbrauch*/) :
    _Bezeichnung(_bezeichnung), _MaximalerSaettigungsgrad(_maximalerSaettigungsgrad)/*, _Flugenergieverbrauch(_flugenergieverbrauch)*/ {
    _AktuellerSaettigungsgrad = 0;
}
inline void Vogel::Fressen() {
     cout << "Der Vogel frisst Insekten." << endl;
}
inline void Vogel::Fressen(int energiegehalt){
    _AktuellerSaettigungsgrad += energiegehalt;
     cout << "Der Vogel frisst Insekten." << endl;
}

inline void Vogel::GefressenWerden() {
     cout << "Der Vogel wird von einem anderen Tier gefressen." << endl;
}
inline void Vogel::Fliegen() {
    if (_AktuellerSaettigungsgrad > 0) {
        //_AktuellerSaettigungsgrad -= _Flugenergieverbrauch;
        cout << "Der Vogel fliegt und verbraucht Energie." << endl;
    } else {
        cout << "Der Vogel hat nicht genug Energie zum Fliegen." << endl;
    }
}

inline void Vogel::FutterAufnehmen(int menge) {
    _AktuellerSaettigungsgrad += menge;
    if (_AktuellerSaettigungsgrad > _MaximalerSaettigungsgrad)
        _AktuellerSaettigungsgrad = _MaximalerSaettigungsgrad;
}

#endif // VOGEL_H
