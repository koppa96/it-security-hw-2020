#pragma once

#include "Defines.h"
#include <algorithm>

#ifdef WIN32
	extern "C" __declspec(dllexport) len_t ParseAnimation(const char* in_buffer, len_t in_len, char* out_buffer, len_t out_len);
#else
	len_t ParseAnimation(const char* in_buffer, len_t in_len, char* out_buffer, len_t out_len);
#endif