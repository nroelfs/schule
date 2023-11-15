#include <iostream>
#include <liste.h>
#include <genericlist.h>
using namespace std;

int main()
{
//    Liste t;
//    for(int i = 0; i < 100; i++){
//        t.insert(i);
//    }
//    //t.invert();
//    while(!t.endpos()){
//       int test = t.elem();
//       cout << test << endl;;
//       t.advance();
//    }
//   t.~Liste();

   GenericList<char> t(10);
   for(int i = 0; i < 100; i++){
           t.insert(i);
       }
       t.invert();
       while(!t.endpos()){
          int test = t.elem();
          cout << test << endl;;
          t.advance();
       }
     t.~GenericList();



    return 0;
}
