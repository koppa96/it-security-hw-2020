#define FUZZING 1

#include <string>
#include <fstream>
#include "../parser_core/LibApi.h"
#include "../parser_core/Exceptions.h"

#if FUZZING
#include <cstdio>
#include <iostream>
#endif

int main()
{
    std::string input_path = "../../files/1.caff";
    std::string output_path = "../../output/asd_inout.bmp";
#if FUZZING
    //FILE* stream_r, * stream_w;
    //Change the second (NULL) parameters to file names if you want to redirect stdin/stdout to a file
    //freopen_s(&stream_r, NULL, "rb", stdin);
    //freopen_s(&stream_w, NULL, "wb", stdout);
    std::streampos fileSize;
    std::cin.seekg(0, std::ios::end);
    fileSize = std::cin.tellg();
    std::cin.seekg(0, std::ios::beg);
    const int in_len = (int)fileSize + 1;
    const char* in_buff = new char[in_len];
    std::cin.read((char*)&in_buff[0], fileSize);
#else
    std::ifstream anim_in(input_path, std::ios_base::in | std::ios_base::binary);
    std::streampos fileSize;
    anim_in.seekg(0, std::ios::end);
    fileSize = anim_in.tellg();
    anim_in.seekg(0, std::ios::beg);
    const int in_len = (int)fileSize + 1;
    const char* in_buff = new char[in_len];
    anim_in.read((char*)&in_buff[0], fileSize);
#endif //FUZZING
    //Setting output buffers and lengths
    const int out_len = in_len;
    char *out_buff = new char[out_len];
    //Passing input for parsing
#if FUZZING
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
#endif
    //Writing image, releasing resources
#if FUZZING
    std::cout.write(out_buff, real_out_len);
    //fclose(stream_r);
    //fclose(stream_w);
#else
    std::ofstream image_out(output_path, std::ios_base::out | std::ios_base::binary);
    image_out.write(out_buff, real_out_len);
    anim_in.close();
    image_out.close();
#endif //FUZZING
    delete[] out_buff;
}
