#ifndef LISTE_H
#define LISTE_H
#include "abstractlist.h"

class Liste : public AbstractList
{
public:
    Liste(int initialCapacity = 10);
    ~Liste();

    bool empty() override;
    bool endpos() override;
    void reset() override;
    void advance() override;
    void insert(int number) override;
    int elem() override;
    void remove() override;
    //selbst erstellt
    void invert();
private:
    int* array;
    int size;
    int capacity;
    int currentPosition;
    /**
     * @brief   expandArray: vergrößert das Array, verdoppelt die capacity
     */
    void expandArray();
    /**
     * @brief   shiftElements: bewegt alle Elemente in einem Array eine Postionion nach hinten
     *          wenn 0 die vorderste Position ist
     * @param   index: Aktuelle Position im Array
     */
    void shiftElements(int index);
};

#endif // LISTE_H
