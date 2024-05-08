#include <iostream>
#include "Insekt.h"
#include "Vogel.h"
using namespace std;

int main()
{   
    Insekt ameise("ameise", 60);
    Vogel specht("specht", 50/*,5*/);

    specht.Fressen(/*ameise.Energiegehalt()*/);
    ameise.GefressenWerden();
    specht.Fliegen();

    return 0;
}
