#include "pch.h"
#include "Parser.h"
#include <stdexcept>

Parse& Parser::GenerateParse(const char* in_buffer, int in_len) {
	Parse p = std::make_shared<ParseData>(in_buffer, in_len);
	for (int i = 0; i < in_len; ) {
		int block_type = (int)in_buffer[i++];

		switch (block_type) {
		case 1:
			i = ParseHeaderBlock(p, i);
			break;
		case 2:
			i = ParseCreditsBlock(p, i);
			break;
		case 3:
			i = ParseAnimationBlock(p, i);
			break;
		default:
			throw std::invalid_argument("Invalid block id!");
		}
	}
	return p;
}