#include <iostream>
#include <liste.h>
using namespace std;

int main()
{
    cout << "It's alive!" << endl;

    Liste t;
    for(int i = 0; i < 100; i++){
        t.insert(i);
    }
    t.reset();
    while(!t.endpos()){
       int test = t.elem();
       cout << test << endl;;
       t.advance();
    }
   t.~Liste();
    return 0;
}
