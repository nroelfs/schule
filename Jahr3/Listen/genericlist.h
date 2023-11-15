#ifndef GENERICLIST_H
#define GENERICLIST_H

template <typename T>
class GenericList
{
public:
    GenericList(int initialCapacity);
    ~GenericList();

    bool empty();
    bool endpos();
    void reset();
    void advance();
    void insert(T element);
    T elem();
    void remove();
    void invert();

private:
    void expandArray();
    void shiftElements(int index);

    int size;
    int capacity;
    T* array;
    int currentPosition;
};

#endif  // GENERICLIST_H
