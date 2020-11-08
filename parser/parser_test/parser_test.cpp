#define INOUT_PIPING 0

#include <string>
#include <fstream>
#include "../parser_core/LibApi.h"
#include "../parser_core/Exceptions.h"

#if INOUT_PIPING
#include <cstdio>
#include <iostream>
#endif

int main()
{
    //Default input/output path:
    std::string input_path = "../../files/1.caff";
    std::string output_path = "../../output/caff1.bmp";
    //Reading input
#if INOUT_PIPING
    std::string anim_str{ std::istreambuf_iterator<char>(std::cin), std::istreambuf_iterator<char>() };
    const char* in_buff = anim_str.c_str();
    const int in_len = anim_str.length() + 1;
#else
    std::ifstream anim_in(input_path, std::ios_base::in | std::ios_base::binary);
    std::streampos fileSize;
    anim_in.seekg(0, std::ios::end);
    fileSize = anim_in.tellg();
    anim_in.seekg(0, std::ios::beg);
    const int in_len = (int)fileSize + 1;
    const char* in_buff = new char[in_len];
    anim_in.read((char*)&in_buff[0], fileSize);
#endif //INOUT_PIPING
    //Setting output buffers and lengths
    const int out_len = in_len;
    char *out_buff = new char[out_len];
    //Passing input for parsing
#if INOUT_PIPING
    int real_out_len = 0;
    try
    {
        real_out_len = ParseAnimation(in_buff, in_len, out_buff, out_len);
    }
    catch (caff_parser_exception e)
    {
        std::cout << "Invalid CAFF file: " << e.getMessage() << std::endl;
    }
#else
    const int real_out_len = ParseAnimation(in_buff, in_len, out_buff, out_len);
#endif //INOUT_PIPING
    //Writing image, releasing resources
#if INOUT_PIPING
    std::cout.write(out_buff, real_out_len);
#else
    std::ofstream image_out(output_path, std::ios_base::out | std::ios_base::binary);
    image_out.write(out_buff, real_out_len);
    anim_in.close();
    image_out.close();
#endif //INOUT_PIPING
    delete[] out_buff;
}
