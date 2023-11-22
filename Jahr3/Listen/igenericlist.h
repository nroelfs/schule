#ifndef IGENERICLIST_H
#define IGENERICLIST_H

template <typename T>
class IGenericList
{
public:
    //IGenericList();
    /**
  * @brief   empty: Gibt an, ob die Liste leer ist.
  * @return  true wenn Liste leer, sonst false.
  */
    virtual bool empty() const = 0;

    /**
     * @brief   endpos: Gibt an, ob das Ende der Liste erreicht wurde.
     * @return  true, wenn Liste abgearbeitet
     *          (das aktuelle Element das letzte Element) ist,
     *          sonst false.
     */
    virtual bool endpos() const = 0;

    /**
     * @brief   reset: setzt das aktuelle Element auf das erste Element.
     */
    virtual void reset() = 0;

    /**
     * @brief   advance: Das aktuelle Element wird zum Folgeelement.
     */
    virtual void advance() = 0;

    /**
     * @brief   insert: Fügt vor das aktuelle Element ein neues hinzu.
     *                Das neue Element wird zum aktuellen.
     * @param   value: Eintrag der eingefügt werden soll.
     */
    virtual void insert(const T& value) = 0;

    /**
     * @brief   elem: Liefert das aktuelle Element.
     * @return  Das aktuelle Element.
     */
    virtual T elem() const = 0;

    /**
     * @brief   remove: Löscht das aktuelle Element.
     *          Der Nachfolger wird zum aktuellen Element.
     */
    virtual void remove() = 0;

};

#endif // IGENERICLIST_H
