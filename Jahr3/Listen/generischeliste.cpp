#include "generischeListe.h"
#include <string>

template <typename T>
generischeListe<T>::generischeListe(int initialCapacity)
//    : size(0),
//      capacity(initialCapacity),
//      array(new T[capacity]),
//      currentPosition(-1)
{
    size = 0;
    capacity = initialCapacity;
    array = new T[capacity];
    currentPosition = -1;
}

template <typename T>
generischeListe<T>::~generischeListe()
{
    delete[] array;
}

template <typename T>
bool generischeListe<T>::empty() const
{
    return size == 0;
}

template <typename T>
bool generischeListe<T>::endpos() const
{
    return currentPosition == size;
}

template <typename T>
void generischeListe<T>::reset()
{
    currentPosition = 0;
}

template <typename T>
void generischeListe<T>::advance()
{
    if (!endpos())
    {
        currentPosition++;
    }
}

template <typename T>
void generischeListe<T>::insert(const T& element)
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
T generischeListe<T>::elem() const
{
    if (currentPosition < 0 || currentPosition >= size)
    {
        return T();
    }
    return array[currentPosition];
}

template <typename T>
void generischeListe<T>::remove()
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

template <typename T>
void generischeListe<T>::invert()
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

template <typename T>
void generischeListe<T>::expandArray()
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
void generischeListe<T>::shiftElements(int index)
{
    for (int i = size - 1; i >= index; i--)
    {
        array[i + 1] = array[i];
    }
    currentPosition = index;
}

template class generischeListe<char>;
template class generischeListe<std::string>;
template class generischeListe<int>;
template class generischeListe<double>;
template class generischeListe<long>;
template class generischeListe<float>;
template class generischeListe<bool>;


