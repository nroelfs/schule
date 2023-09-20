#ifndef ILIST_H
#define ILIST_H

template <typename T>
class IList {
public:
    virtual bool empty() = 0;
    virtual bool endpos() = 0;
    virtual void reset() = 0;
    virtual void advance() = 0;
    virtual T elem() = 0;
    virtual void insert(T& element) = 0;
    virtual void remove() = 0;
    virtual ~IList() {}
};


#endif // ILIST_H
