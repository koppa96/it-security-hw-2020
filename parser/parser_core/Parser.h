#pragma once

#include "Parse.h"

class Parser {
	void ParseCIFF(Parse&);
	void ParseCIFFHeader(Parse&);
	void ParseHeaderBlock(Parse&);
	void ParseCreditsBlock(Parse&);
	void ParseAnimationBlock(Parse&);
public:
	Parser() {

	}

	Parse& GenerateParse(const char* in_buffer, int in_len) {
		Parse p;
		return p;
	}
};