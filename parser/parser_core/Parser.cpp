#include "pch.h"
#include "Parser.h"
#include <stdexcept>
#include <cmath>

Parse& Parser::GenerateParse(const char* in_buffer, int in_len) {
	Parse p = std::make_shared<ParseData>(in_buffer, in_len);
	for (int i = 0; i < in_len; ) {
		int block_type = (int)in_buffer[i++];

		switch (block_type) {
		case 1:
			i = ParseHeaderBlock(i);
			break;
		case 2:
			i = ParseCreditsBlock(i);
			break;
		case 3:
			i = ParseAnimationBlock(i);
			break;
		default:
			throw std::invalid_argument("Invalid block id!");
		}
	}
	return p;
}

int Parser::ParseHeaderBlock(int current_idx) {
	int block_len = ReadLength(current_idx); //TODO: Check if read size is correct
	current_idx += LENGTH_BLOCK_SIZE;

	std::string filetype(parse->raw_data, parse->raw_data + FILE_TYPE_SIZE);
	if (filetype != "CAFF")
		throw std::invalid_argument("Invalid file type!");
	current_idx += FILE_TYPE_SIZE;

	int header_size = ReadLength(current_idx); //TODO: Check if read size is correct
	current_idx += LENGTH_BLOCK_SIZE;

	int num_anim = ReadLength(current_idx); //TODO: Check if read image count is correct
	parse->SetImageCount(num_anim);
	current_idx += LENGTH_BLOCK_SIZE;

	return current_idx;
}

int Parser::ReadLength(int current_idx) {
	if ((current_idx + LENGTH_BLOCK_SIZE) > parse->raw_data_len)
		throw std::out_of_range("Data index larger than data size!");

	int length = 0;
	for (int i = 0; i < LENGTH_BLOCK_SIZE; i++) {
		length += (int)parse->raw_data[current_idx + i] * (int)std::pow(10, i);
	}
	return length;
}