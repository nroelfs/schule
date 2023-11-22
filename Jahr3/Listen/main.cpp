#include <iostream>
#include <liste.h>
#include "generischeliste.h"
using namespace std;

int main()
{
    Liste t;
    int selection;
    do
    {
        cout << "Listen Tester" << endl
             << "1: insert" << endl
             << "2: remove" << endl
             << "3: advance" << endl
             << "4: empty" << endl
             << "5: enpos" << endl
             << "6: reset" << endl
             << "7: elem" << endl
             << "8: invert" << endl
             << "0: beenden" << endl;

        cout << "Auswahl: ";
        cin >> selection;

        switch (selection)
        {
            case 1:
                int number;
                cout << "Geben Sie die Zahl ein, die Sie einfügen möchten: ";
                cin >> number;
                t.insert(number);
                break;
            case 2:
                t.remove();
                break;
            case 3:
                t.advance();
                break;
            case 4:
                cout << "Liste ist " << (
                            t.empty()
                            ? ""
                            : "nicht ") << "leer." << endl;
                break;
            case 5:
                cout << "Endposition erreicht: " << (
                            t.endpos()
                            ? "Ja"
                            : "Nein") << endl;
                break;
            case 6:
                t.reset();
                break;
            case 7:
                cout << "Element an der aktuellen Position: " << t.elem() << endl;
                break;
            case 8:
                t.invert();
                break;
            case 0:
                cout << "Programm beendet." << endl;
                break;

        }
    } while (selection != 0);





//    if(t.empty()){
//        cout << "ich bin leer!" << endl;
//    }
//    for(int i = 0; i < 10; i++){
//        t.insert(i);
//    }
//    t.reset();
//    //t.invert();
//    while(!t.endpos()){
//        if(t.elem() == 5){
//            t.remove();
//        }
//        cout << t.elem() << endl;
//        t.advance();

//    }

    //t.~Liste();

//    try {
//        generischeListe<char> myList;

//        myList.insert('1');
//        myList.insert('2');
//        myList.insert('3');

//        cout << "Elements in the list: " << endl;
//        while (!myList.endpos())
//        {
//            cout << myList.elem() << endl;
//            myList.advance();
//        }
//        cout << endl;
//    }
//    catch (exception& e) {
//        cout << e.what() << endl;
//        return -1;
//    }


     return 0;
}
