#pragma once

#include "Parse.h"
#include "ParseImage.h"

constexpr auto LENGTH_BLOCK_SIZE = 8;
constexpr auto FILE_TYPE_SIZE = 4;
constexpr auto DATE_BLOCK_SIZE = 6;
constexpr auto BYTES_PER_PIXEL = 3;

class Parser {
	
	Parse parse;

	const ParseImage& ParseCIFF(int current_idx);
	int ParseHeaderBlock(int current_idx);
	int ParseCreditsBlock(int current_idx);
	int ParseAnimationBlock(int current_idx);
	int ReadLength(int current_idx);
public:
	Parser() {

	}

	Parse& GenerateParse(const char* in_buffer, int in_len);
};