#include <iostream>
#include <string>
#include <fstream>
#include "../parser_core/LibApi.h"


void readAndparseAnim() {
    //Reading whole file at once
    std::ifstream anim_in("../../files/1.caff", std::ios_base::in | std::ios_base::binary);
    std::string anim_str{ std::istreambuf_iterator<char>(anim_in), std::istreambuf_iterator<char>() };
    //Converting to c-style string to pass for parsing
    const char* in_buff = anim_str.c_str();
    //Setting output buffers and lengths
    const int in_len = anim_str.length() + 1;
    const int out_len = in_len;
    char* out_buff = new char[out_len];
    //Passing input for parsing
    const int real_out_len = ParseAnimation(in_buff, in_len, out_buff, out_len);
    //Writing image, releasing resources
    std::ofstream image_out("../../output/asd.bmp");
    image_out.write(out_buff, real_out_len);
    anim_in.close();
    image_out.close();
    delete[] out_buff;
}

int main()
{
    readAndparseAnim();
}
