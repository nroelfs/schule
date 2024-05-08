#include "cuboid.h"
#include <stdexcept>

Cuboid::Cuboid(const Rectangle (&rects)[3]) : faces{rects[0], rects[1], rects[2]} {
    for (const Rectangle& rect : faces) {
           if (rect.surfaceArea() < 0) {
               throw std::invalid_argument("Alle Werte müssen größer 0 sein");
           }
       }
}

float Cuboid::surfaceArea() const {
    double totalSurfaceArea = 0.0;
    for (const Rectangle& face : faces) {
        totalSurfaceArea += face.surfaceArea();
    }
    return totalSurfaceArea * 2;
}

float Cuboid::volume() const {

    if (faces[0].surfaceArea() > 0)
        return faces[0].surfaceArea() * 6;
    else
        return 0.0;

}
