#include "LibApi.h"
#include "Parse.h"
#include "Parser.h"
#include "ImageBuilder.h"
#include "Preview.h"
#include <string>

len_t CopyBytes(const char* from, len_t length, char* to, len_t max_len) {
    int n = std::min(length, max_len);
    for (int i = 0; i < n; i++) {
        to[i] = from[i];
    }
    return n;
}

/// <param name="out_len">The size of the output buffer</param>
/// <returns>The actual size of the output (<= buffer size)</returns>
len_t ParseAnimation(const char* in_buffer, len_t in_len, char* out_buffer, len_t out_len)
{
    Parser parser;
    Parse p = parser.GenerateParse(in_buffer, in_len);
    ParseImage pi = p->GetPreviewImage();
    ImageBuilder image_builder;
    Preview preview = image_builder.BuildPreview(pi);
    std::string out_str(preview->data, preview->data + preview->length);
    return CopyBytes(preview->data, preview->length, out_buffer, out_len);
}
