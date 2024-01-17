#include <iostream>
#include "arraylist.h"

using namespace std;

int main()
{
    ArrayList<int> liste;
    int auswahl;
    int index = 0;

    do
    {
        cout << "Listentester\n\n";
        cout << "Liste: \n";
        if (!liste.empty()) {
            liste.reset();
            while (!liste.endpos()) {
                cout << liste.elem() << " ";
                liste.advance();
            }
            cout << liste.elem() << "\n";
            liste.reset();
            for (int i = 0; i < index; i++) {
                liste.advance();
                cout << "  ";
            }
            cout << "^\n\n";
        }
        else {
            cout << "Liste ist leer \n\n";
        }
        cout << "Auswahl:\n";
        cout << "1: insert element\n";
        cout << "2: remove element\n";
        cout << "3: elem\n";
        cout << "4: advance\n";
        cout << "5: reset\n";
        cout << "6: endpos\n";
        cout << "7: empty\n";
        cout << "0: exit\n\n";
        cout << "Auswahl: ";
        cin >> auswahl;
        switch (auswahl)
        {
        case 1:
            int number;
            cout << "Zahl: ";
            cin >> number;
            liste.insert(number);
            break;

        case 2:
            liste.remove();
            if(liste.empty())
            {
                index=0;
            }
            break;

        case 3:
            cout << "element: " << liste.elem() << "\n";
            break;

        case 4:
            if (!liste.endpos()) {
                index++;
            }
            liste.advance();
            break;

        case 5:
            liste.reset();
            index = 0;
            break;

        case 6:
            cout << "endpos: " << (liste.endpos() ? "true" : "false") << "\n";
            break;

        case 7:
            cout << "empty: " << (liste.empty() ? "true" : "false") << "\n";
        }

    } while (auswahl != 0);

    return 0;
}
