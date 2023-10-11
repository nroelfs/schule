#include "liste.h"

Liste::Liste(int initialCapacity)
{
    size = 0;
    capacity = initialCapacity;
    array = new int[capacity];
    currentPosition = -1;
}

Liste::~Liste()
{
    delete[] array;
}

bool Liste::empty()
{
    return size == 0;
}

bool Liste::endpos()
{
    return currentPosition == size;
}

void Liste::reset()
{
    currentPosition = 0;
}

void Liste::advance()
{
    if (!endpos())
    {
        currentPosition++;
    }
}

void Liste::insert(int number)
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
    array[currentPosition] = number;
    size++;
}

int Liste::elem()
{
    if (currentPosition < 0 || currentPosition >= size)
    {
        return -1;
    }
    return array[currentPosition];
}

void Liste::remove()
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

//Private Section
void Liste::expandArray()
{
    capacity *= 2;
    int* newArray = new int[capacity];
    for (int i = 0; i < size; i++)
    {
        newArray[i] = array[i];
    }
    delete[] array;
    array = newArray;
}

void Liste::shiftElements(int index)
{
    for (int i = size - 1; i >= index; i--)
    {
        array[i + 1] = array[i];
    }
    currentPosition = index;
}
