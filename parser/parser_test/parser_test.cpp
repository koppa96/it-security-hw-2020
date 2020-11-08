#define FUZZING 0

#include <string>
#include <fstream>
#include "../parser_core/LibApi.h"

#if FUZZING
    #include <iostream>
    #include <stdexcept>
    #ifdef WIN32
        #include <fcntl.h>
        #include <io.h>
    #endif
#endif


int main()
{
    std::string input_path = "../../files/1.caff";
    std::string output_path = "../../output/asd_inout.bmp";
#if FUZZING
    #ifdef WIN32
        _setmode(_fileno(stdin), _O_BINARY);
        _setmode(_fileno(stdout), _O_BINARY);
    #else
        throw std::logic_error("Functionality not implemented for non-Windows platforms!");
    #endif  //WIN32
    //The following 3 lines (+2 the fcloses at the end of the function) can be used for testing by redirecting stdin/stdout to read from/write to a file
    //FILE *stream_r, *stream_w;
    //freopen_s(&stream_r, input_path.c_str(), "rb", stdin);
    //freopen_s(&stream_w, output_path.c_str(), "wb", stdout);
    std::string anim_str{ std::istreambuf_iterator<char>(std::cin), std::istreambuf_iterator<char>() };
#else
    //Reading whole file at once
    std::ifstream anim_in(input_path, std::ios_base::in | std::ios_base::binary);
    std::string anim_str{ std::istreambuf_iterator<char>(anim_in), std::istreambuf_iterator<char>() };
#endif  //FUZZING

    //Converting to c-style string to pass for parsing
    const char* in_buff = anim_str.c_str();
    //Setting output buffers and lengths
    const int in_len = anim_str.length() + 1;
    const int out_len = in_len;
    char* out_buff = new char[out_len];
    //Passing input for parsing
    const int real_out_len = ParseAnimation(in_buff, in_len, out_buff, out_len);

    //Writing image, releasing resources
#if FUZZING
    std::cout.write(out_buff, real_out_len);
    //These 2 linges are for testing, see details at the top of the function
    //fclose(stream_r);
    //fclose(stream_w);
#else
    std::ofstream image_out(output_path, std::ios_base::out | std::ios_base::binary);
    image_out.write(out_buff, real_out_len);
    anim_in.close();
    image_out.close();
#endif  //FUZZING
    delete[] out_buff;
}
