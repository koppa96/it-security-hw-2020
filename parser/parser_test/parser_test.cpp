#include <iostream>
#include "MyTest.h"

int main()
{
    char* buff = new char[16];
    helloWorld(buff);
    std::cout << buff;
    delete[] buff;
}
