#include <list>
#include <iostream>
#include <fstream>
#include <string>



using namespace std;

int main()
{
    bool beenden = false;
    string dateiPfad, line; //Dateipfad und Zeile in der Text datei
    list<int> Zahlen; //Liste für gelesene Zahlen
    ifstream stream; //Klasse zum Datei lesen
    cout << "Herzlich Wilkommen!" << endl;
    do{
        cout << "Hier k"<<(char)148<<"nnen Sie Dateien einlesen, die eine Liste von Ganzzahlen enthalten. Die eingelesene Datei wird anschlie"<<(char)225<<"end zeilenweise und Sortiert ausgegeben.  " << endl
             << "Um das Programm zu beenden geben Sie anstatt des Dateipfades ein e oder E ein." << endl
             << "Bitte den Dateipfad angeben: ";
        cin >> dateiPfad;
        //überprüfung ob das Programm beendet werden soll
       if(dateiPfad == "e" || dateiPfad == "E"){
           beenden = true;
       }
       else{
            stream.open(dateiPfad);
            if(stream){
                while(getline(stream, line)){
                    try{
                        Zahlen.push_front(stoi(line));

                    } catch(invalid_argument ia){
                        cout << ia.what() << endl;
                    }
                    catch(exception e) {

                        cout << e.what() << "->line-content: " << line << endl;
                    }
                }
                stream.close();
                Zahlen.sort();
                for (auto zahl : Zahlen) {

                    cout << zahl <<endl;
                }
                cout << endl;
                Zahlen.clear();
            }
            else{
                cout << "Fehler beim Datei "<<(char)148<< "ffen" << endl << "Bitte "<< (char)154<<"berprüfen Sie den Dateipfad."<<endl;
            }

       }
    }
    while(!beenden);
    return 0;
}
