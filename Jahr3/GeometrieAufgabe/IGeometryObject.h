#ifndef IGEOMETRYOBJECT_H
#define IGEOMETRYOBJECT_H
class IGeometryObject{
public:
    virtual float surfaceArea() const = 0;
    virtual float volume() const = 0;
};
#endif // IGEOMETRYOBJECT_H
