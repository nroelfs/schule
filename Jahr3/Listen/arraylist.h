#ifndef ARRAYLIST_H
#define ARRAYLIST_H

#include "abstractlist.h"
template<typename T>
class ArrayList : public AbstractList<T>
{
private:
    T* items;
    int length;
    int index;

public:
    ArrayList();
    bool empty() override;
    bool endpos() override;
    void reset() override;
    void advance() override;
    void insert(T number) override;
    T elem() override;
    void remove() override;
};

template<typename T>
ArrayList<T>::ArrayList()
{
    length = 0;
    index = -1;
    items = nullptr;
}

template<typename T>
bool ArrayList<T>::empty()
{
    return (length == 0);
}

template<typename T>
bool ArrayList<T>::endpos()
{
    return (length == index + 1);
}

template<typename T>
void ArrayList<T>::reset()
{
    if (length != 0)
    {
        index = 0;
    }
}

template<typename T>
void ArrayList<T>::advance()
{
    if (!endpos())
    {
        index++;
    }
}

template<typename T>
void ArrayList<T>::insert(T number)
{
    T* temp = new T[length + 1];

    if (index > -1)
    {
        for (int i = 0; i < index; i++)
        {
            temp[i] = items[i];
        }

        temp[index] = number;

        for (int i = index + 1; i <= length; i++)
        {
            temp[i] = items[i - 1];
        }

        delete[] items;
    }
    else
    {
        index = 0;
        temp[0] = number;
    }

    length++;

    items = temp;
}

template<typename T>
T ArrayList<T>::elem()
{
    if (index == -1)
    {
        return T();
    }

    return items[index];
}

template<typename T>
void ArrayList<T>::remove()
{
    if (length == 0)
    {
        return;
    }

    if (length == 1)
    {
        delete[] items;
        index = -1;
        length = 0;
        return;
    }

    T* temp = new T[length - 1];

    for (int i = 0; i < index; i++)
    {
        temp[i] = items[i];
    }

    for (int i = index + 1; i < length; i++)
    {
        temp[i - 1] = items[i];
    }

    length--;

    delete[] items;

    items = temp;
}
#endif // ARRAYTLIST_H
