#ifndef RECTANGLE_H
#define RECTANGLE_H
#include "IGeometryObject.h"

class Rectangle : public IGeometryObject {
private:
    float length;
    float width;

public:
    Rectangle(float l, float w);
    float surfaceArea() const override;
    float volume() const override;
    float getLength() const;
    void setLength(float newLength);
    float getWidth() const;
    void setWidth(float newWidth);
};


#endif // RECTANGLE_H
