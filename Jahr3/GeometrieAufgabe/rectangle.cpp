#include "rectangle.h"
#include "IGeometryObject.h"
#include <stdexcept>

float Rectangle::getLength() const
{
    return length;
}

void Rectangle::setLength(float newLength)
{
    if (newLength < 0) {
        throw std::invalid_argument("Rectangle Größen müssen größer als 0 sein");
        return;
    }
    length = newLength;
}

float Rectangle::getWidth() const
{
    return width;
}

void Rectangle::setWidth(float newWidth)
{
    if (newWidth <0) {
        throw std::invalid_argument("Rectangle Größen müssen größer als 0 sein");
        return;
    }
    width = newWidth;
}

Rectangle::Rectangle(float l, float w) : length(l), width(w) {
    if (length < 0 || width < 0) {
        throw std::invalid_argument("Rectangle Größen müssen größer als 0 sein");
    }
}

float Rectangle::surfaceArea() const {
    return length * width;
}

float Rectangle::volume() const {
    return 0.0; // A rectangle has no volume
}
