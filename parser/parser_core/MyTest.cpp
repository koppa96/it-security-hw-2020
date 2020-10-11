#include "pch.h"
#include "MyTest.h"

void helloWorld(char* out_buffer)
{
     snprintf(out_buffer, 16, "%s", "Hello C++ World");
}
