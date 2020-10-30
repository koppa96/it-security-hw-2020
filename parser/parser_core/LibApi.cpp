#include "pch.h"
#include "LibApi.h"
#include "Parse.h"
#include "Parser.h"

int ParseAnimation(const char* in_buffer, int in_len, char* out_buffer, int out_len)
{
    Parser parser;
    Parse p = parser.GenerateParse(in_buffer, in_len);
    ParseImage pi = p->GetPreview();
    //TODO: Save parsed preview and return path
    return 0;
}
