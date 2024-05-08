#include <iostream>
#include "Insekt.h"
#include "Vogel.h"
using namespace std;

int main()
{
    srand(time(0));

    Insekt ameise("Spinne", 10);
    Vogel specht("Amsel", 50, 5);

    specht.Fressen();
    ameise.GefressenWerden();
    specht.Fliegen();
    return 0;
}
