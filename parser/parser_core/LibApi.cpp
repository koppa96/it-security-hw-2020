#include "pch.h"
#include "LibApi.h"
#include "Parse.h"
#include "Parser.h"
#include "ImageSaver.h"
#include <string>

/// <param name="out_len">The size of the output buffer</param>
/// <returns>The actual size of the output (<= buffer size)</returns>
len_t ParseAnimation(const char* in_buffer, len_t in_len, char* out_buffer, len_t out_len)
{
    Parser parser;
    Parse p = parser.GenerateParse(in_buffer, in_len);
    ParseImage pi = p->GetPreview();
    ImageSaver image_saver;
    std::string preview_path = image_saver.SavePreview(pi);
    strncpy_s(out_buffer, out_len, preview_path.c_str(), out_len);
    return min(out_len, (len_t)preview_path.length() + 1);
}
