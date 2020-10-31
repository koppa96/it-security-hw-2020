#include "pch.h"
#include "Parser.h"
#include <stdexcept>
#include <cmath>

Parse& Parser::GenerateParse(const char* in_buffer, int in_len) {
	parse = std::make_shared<ParseData>(in_buffer, in_len);
	for (int i = 0; i < in_len; ) {
		int block_type = (int)in_buffer[i++];

		int block_len = ReadLength(i); //TODO: Check if read size is correct
		i += LENGTH_BLOCK_SIZE;

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
	return parse;
}

int Parser::ParseHeaderBlock(int current_idx) {
	std::string filetype(parse->raw_data + current_idx, parse->raw_data + current_idx + FILE_TYPE_SIZE);
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

int Parser::ParseCreditsBlock(int current_idx) {
	//TODO: read and parse date
	current_idx += DATE_BLOCK_SIZE;

	int creator_len = ReadLength(current_idx); //TODO: Check if read length is correct
	current_idx += LENGTH_BLOCK_SIZE;

	std::string creator(parse->raw_data + current_idx, parse->raw_data + current_idx + creator_len);
	parse->SetCreatorName(creator);
	current_idx += creator_len;

	return current_idx;
}

int Parser::ParseAnimationBlock(int current_idx) {
	int duration = ReadLength(current_idx); //TODO: Check if read duration is correct		//The read value is probably not correct atm, need to check later, it's ok for now
	current_idx += LENGTH_BLOCK_SIZE;

	auto img = ParseCIFF(current_idx);
	parse->AddImage(img, duration);

	return img->data_end_idx;
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

const ParseImage& Parser::ParseCIFF(int current_idx) {
	int start_idx = current_idx;

	std::string filetype(parse->raw_data + current_idx, parse->raw_data + current_idx + FILE_TYPE_SIZE);
	if (filetype != "CIFF")
		throw std::invalid_argument("Invalid file type!");
	current_idx += FILE_TYPE_SIZE;

	int header_size = ReadLength(current_idx); //TODO: Check if read size is correct
	int header_end = start_idx + header_size;
	current_idx += LENGTH_BLOCK_SIZE;

	int content_size = ReadLength(current_idx); //TODO: Check if read size is correct
	current_idx += LENGTH_BLOCK_SIZE;

	int width = ReadLength(current_idx);
	current_idx += LENGTH_BLOCK_SIZE;

	int height = ReadLength(current_idx);
	current_idx += LENGTH_BLOCK_SIZE;

	if(content_size != (width * height * BYTES_PER_PIXEL))
		throw std::out_of_range("Content size not matching image size!");

	int i = 0;
	while (parse->raw_data[current_idx + i] != '\n') {
		if((current_idx + i) > header_end)
			throw std::invalid_argument("Caption must end with a \\n!");
		i++;
	}
	std::string caption(parse->raw_data + current_idx, parse->raw_data + current_idx + i);
	current_idx += i;

	ParseImage image = std::make_shared<ImageParseData>(caption, width, height);

	i = 0;
	while ((current_idx + i) <= header_end) {
		int j = 0;
		while (parse->raw_data[current_idx + i + j] != '\0') {
			if ((current_idx + i + j) > (start_idx + header_size))
				throw std::invalid_argument("Last image tag must end with a \\0!");
			j++;
		}
		std::string tag(parse->raw_data + current_idx + i, parse->raw_data + current_idx + i + j);
		image->AddTag(tag);
		i += j + 1;
	}
	current_idx = header_end;

	for (i = 0; i < content_size; i++) {
		int base_idx = current_idx + i * BYTES_PER_PIXEL;

		if ((base_idx + BYTES_PER_PIXEL - 1) > parse->raw_data_len)
			throw std::out_of_range("Pixel count not matching image size!");

		Pixel p(parse->raw_data[base_idx], parse->raw_data[base_idx + 1], parse->raw_data[base_idx + 2]);
		image->AddPixel(p);
	}

	return image;
}
