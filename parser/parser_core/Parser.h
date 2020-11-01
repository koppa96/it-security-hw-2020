#pragma once

#include "Defines.h"
#include "Parse.h"
#include "ParseImage.h"

class Parser {
	
	Parse parse;

	ParseImage ParseCIFF(len_t current_idx);
	len_t ParseHeaderBlock(len_t current_idx);
	len_t ParseCreditsBlock(len_t current_idx);
	len_t ParseAnimationBlock(len_t current_idx);
	len_t ReadLength(len_t current_idx);
public:
	Parser() {

	}

	Parse& GenerateParse(const char* in_buffer, len_t in_len);
};