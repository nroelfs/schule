#include <iostream>
#include "Insekt.h"
#include "Vogel.h"
using namespace std;

int main()
{   
    Insekt ameise("ameise", 10);
    Vogel specht("specht", 50, 5);

    specht.Fressen();
    ameise.GefressenWerden();
    specht.Fliegen();

    return 0;
}
