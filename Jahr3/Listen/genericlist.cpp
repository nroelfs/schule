#include "genericlist.h"

template <typename T>
GenericList<T>::GenericList(int initialCapacity)
{
    size = 0;
    capacity = initialCapacity;
    array = new T[capacity];
    currentPosition = -1;
}

template <typename T>
GenericList<T>::~GenericList()
{
    delete[] array;
}

template <typename T>
bool GenericList<T>::empty()
{
    return size == 0;
}

template <typename T>
bool GenericList<T>::endpos()
{
    return currentPosition == size;
}

template <typename T>
void GenericList<T>::reset()
{
    currentPosition = 0;
}

template <typename T>
void GenericList<T>::advance()
{
    if (!endpos())
    {
        currentPosition++;
    }
}

template <typename T>
void GenericList<T>::insert(T element)
{
    if (size == capacity)
    {
        expandArray();
    }

    if (currentPosition < 0)
    {
        currentPosition = 0;
    }

    shiftElements(currentPosition);
    array[currentPosition] = element;
    size++;
}

template <typename T>
T GenericList<T>::elem()
{
    if (currentPosition < 0 || currentPosition >= size)
    {
        // throw runtime_error("element is not defined");
        //eigentlich gebe aber ein object T zurück
        return T();
    }
    return array[currentPosition];
}

template <typename T>
void GenericList<T>::remove()
{
    if (currentPosition >= 0 && currentPosition < size)
    {
        shiftElements(currentPosition + 1);
        size--;
        if (size == 0)
        {
            currentPosition = -1;
        }
    }
}
//self added
template <typename T>
void GenericList<T>::invert()
{
    T* newArray = new T[capacity];
    int newArrayIndex = 0;
    for (int i = size - 1; i >= 0; i--)
    {
        newArray[newArrayIndex] = array[i];
        newArrayIndex++;
    }
    delete[] array;
    array = newArray;
}
//Private
template <typename T>
void GenericList<T>::expandArray()
{
    capacity *= 2;
    T* newArray = new T[capacity];
    for (int i = 0; i < size; i++)
    {
        newArray[i] = array[i];
    }
    delete[] array;
    array = newArray;
}

template <typename T>
void GenericList<T>::shiftElements(int index)
{
    for (int i = size - 1; i >= index; i--)
    {
        array[i + 1] = array[i];
    }
    currentPosition = index;
}
