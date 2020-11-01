#include "pch.h"
#include "LibApi.h"
#include "Parse.h"
#include "Parser.h"

/// <param name="out_len">The size of the output buffer</param>
/// <returns>The actual size of the output (<= buffer size)</returns>
len_t ParseAnimation(const char* in_buffer, len_t in_len, char* out_buffer, len_t out_len)
{
    Parser parser;
    Parse p = parser.GenerateParse(in_buffer, in_len);
    ParseImage pi = p->GetPreview();
    //TODO: Save parsed preview and return path
    return 0;
}
