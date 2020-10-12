#include <iostream>
#include <string>
#include "LibApi.h"

int main()
{
    std::string in_txt = "AnotherTestString";
    const int in_len = in_txt.length() + 1;
    const char* in_buff = in_txt.c_str();
    const int out_len = in_len;
    char* out_buff = new char[out_len];
    ParseAnimation(in_buff, in_len, out_buff, out_len);
    std::cout << out_buff;
    delete[] out_buff;
}
