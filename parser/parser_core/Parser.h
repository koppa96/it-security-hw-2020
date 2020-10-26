#pragma once

#include "Parse.h"

constexpr auto LENGTH_BLOCK_SIZE = 8;
constexpr auto FILE_TYPE_SIZE = 4;

class Parser {
	
	Parse parse;

	void ParseCIFF();
	void ParseCIFFHeader();
	int ParseHeaderBlock(int current_idx);
	int ParseCreditsBlock(int current_idx);
	int ParseAnimationBlock(int current_idx);
	int ReadLength(int current_idx);
public:
	Parser() {

	}

	Parse& GenerateParse(const char* in_buffer, int in_len);
};