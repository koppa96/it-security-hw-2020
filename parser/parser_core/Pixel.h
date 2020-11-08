#pragma once

#include "Defines.h"

struct Pixel {
	unsigned int r, g, b;

	Pixel(unsigned int r, unsigned int g, unsigned int b) : r{ r }, g{ g }, b{ b } {
	}
};