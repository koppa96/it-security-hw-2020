#pragma once

#include "Parse.h"

class Parser {
	void ParseCIFF(Parse&);
	void ParseCIFFHeader(Parse&);
	int ParseHeaderBlock(Parse&, int current_idx);
	int ParseCreditsBlock(Parse&, int current_idx);
	int ParseAnimationBlock(Parse&, int current_idx);
public:
	Parser() {

	}

	Parse& GenerateParse(const char* in_buffer, int in_len);
};