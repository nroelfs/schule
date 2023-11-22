#ifndef GENERICLIST_H
#define GENERICLIST_H
#include <string.h>
#include "IGenericList.h"
template <typename T>
class generischeListe : public IGenericList<T>
{
public:
    generischeListe(int initialCapacity = 1);
    ~generischeListe();

    bool empty() const override;
    bool endpos() const override;
    void reset() override;
    void advance() override;
    void insert(const T& element) override;
    T elem() const override;
    void remove() override;
    void invert();

private:
    void expandArray();
    void shiftElements(int index);

    int size;
    int capacity;
    T* array;
    int currentPosition;
};

#endif
