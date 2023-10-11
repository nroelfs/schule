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

private:
    int* array;
    int size;
    int capacity;
    int currentPosition;

    void expandArray();
    void shiftElements(int index);
};

#endif // LISTE_H
