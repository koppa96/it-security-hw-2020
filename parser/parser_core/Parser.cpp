#include "pch.h"
#include "Parser.h"
#include <stdexcept>
#include <cmath>

Parse& Parser::GenerateParse(const char* in_buffer, len_t in_len) {
	const unsigned char* raw_data_converted = reinterpret_cast<const unsigned char*>(in_buffer);	//Making sure input data is treated as unsigned
	parse = std::make_shared<ParseData>(raw_data_converted, in_len);
	for (len_t i = 0; i < (in_len - 1); ) {
		char block_type = in_buffer[i++];

		len_t block_len = ReadLength(i); //TODO: Check if read size is correct
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

len_t Parser::ParseHeaderBlock(len_t current_idx) {
	std::string filetype(parse->raw_data + current_idx, parse->raw_data + current_idx + FILE_TYPE_SIZE);
	if (filetype != "CAFF")
		throw std::invalid_argument("Invalid file type!");
	current_idx += FILE_TYPE_SIZE;

	len_t header_size = ReadLength(current_idx); //TODO: Check if read size is correct
	current_idx += LENGTH_BLOCK_SIZE;

	len_t num_anim = ReadLength(current_idx); //TODO: Check if read image count is correct
	parse->SetImageCount(num_anim);
	current_idx += LENGTH_BLOCK_SIZE;

	return current_idx;
}

len_t Parser::ParseCreditsBlock(len_t current_idx) {
	//TODO: read and parse date
	current_idx += DATE_BLOCK_SIZE;

	len_t creator_len = ReadLength(current_idx); //TODO: Check if read length is correct
	current_idx += LENGTH_BLOCK_SIZE;

	std::string creator(parse->raw_data + current_idx, parse->raw_data + current_idx + creator_len);
	parse->SetCreatorName(creator);
	current_idx += creator_len;

	return current_idx;
}

len_t Parser::ParseAnimationBlock(len_t current_idx) {
	len_t duration = ReadLength(current_idx); //TODO: Check if read duration is correct		//The read value is probably not correct atm, need to check later, it's ok for now
	current_idx += LENGTH_BLOCK_SIZE;

	auto img = ParseCIFF(current_idx);
	parse->AddImage(img, duration);

	return img->data_end_idx;
}

len_t Parser::ReadLength(len_t current_idx) {
	if ((current_idx + LENGTH_BLOCK_SIZE) > parse->raw_data_len)
		throw std::out_of_range("Data index larger than data size!");

	len_t length = 0;
	for (int i = 0; i < LENGTH_BLOCK_SIZE; i++) {
		length |= ((len_t)parse->raw_data[current_idx + i]) << (i * 8);
	}
	return length;
}

ParseImage Parser::ParseCIFF(len_t current_idx) {
	len_t start_idx = current_idx;

	std::string filetype(parse->raw_data + current_idx, parse->raw_data + current_idx + FILE_TYPE_SIZE);
	if (filetype != "CIFF")
		throw std::invalid_argument("Invalid file type!");
	current_idx += FILE_TYPE_SIZE;

	len_t header_size = ReadLength(current_idx); //TODO: Check if read size is correct
	len_t header_end = start_idx + header_size;
	current_idx += LENGTH_BLOCK_SIZE;

	len_t content_size = ReadLength(current_idx); //TODO: Check if read size is correct
	current_idx += LENGTH_BLOCK_SIZE;

	len_t width = ReadLength(current_idx);
	current_idx += LENGTH_BLOCK_SIZE;

	len_t height = ReadLength(current_idx);
	current_idx += LENGTH_BLOCK_SIZE;

	if(content_size != (width * height * BYTES_PER_PIXEL))
		throw std::out_of_range("Content size not matching image size!");

	int i = 0;
	while (parse->raw_data[current_idx + i] != '\n') {
		if(((len_t)current_idx + i) > header_end)
			throw std::invalid_argument("Caption must end with a \\n!");
		i++;
	}
	std::string caption(parse->raw_data + current_idx, parse->raw_data + current_idx + i);
	current_idx += i;

	ParseImage image = std::make_shared<ImageParseData>(caption, width, height);

	i = 0;
	while (((len_t)current_idx + i) < header_end) {
		int j = 0;
		while (parse->raw_data[current_idx + i + j] != '\0') {
			if (((len_t)current_idx + i + j) > (start_idx + header_size))
				throw std::invalid_argument("Last image tag must end with a \\0!");
			j++;
		}
		std::string tag(parse->raw_data + current_idx + i, parse->raw_data + current_idx + i + j);
		image->AddTag(tag);
		i += j + 1;
	}
	current_idx = header_end;

	len_t pixel_count = width * height;
	for (i = 0; i < pixel_count; i++) {
		len_t base_idx = current_idx + (len_t)i * BYTES_PER_PIXEL;

		if ((base_idx + BYTES_PER_PIXEL - 1) > parse->raw_data_len)
			throw std::out_of_range("Pixel count not matching image size!");

		Pixel p(parse->raw_data[base_idx], parse->raw_data[base_idx + 1], parse->raw_data[base_idx + 2]);
		image->AddPixel(p);
	}

	current_idx += content_size;

	image->data_start_idx = start_idx;
	image->data_end_idx = current_idx;

	return image;
}
