#include "Parser.h"

Parse& Parser::GenerateParse(const char* in_buffer, len_t in_len) {
	const unsigned char* raw_data_converted = reinterpret_cast<const unsigned char*>(in_buffer);	//Making sure input data is treated as unsigned
	parse = std::make_shared<ParseData>(raw_data_converted, in_len);

	len_t i = 0;
	char block_type = parse->raw_data[i++];
	if (block_type != CAFF_HEADER_BLOCK_TYPE) {
		throw std::invalid_argument("The file must start with a CAFF header.");
	}

	i = ReadBlockLength(i, in_len);

	i = ParseHeaderBlock(i);

	while (i < (in_len - 1)) {
		char block_type = parse->raw_data[i++];

		i = ReadBlockLength(i, in_len);

		switch (block_type) {
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
	if (parse->GetImagesCurrentSize() < parse->GetImageCount()) {
		throw std::underflow_error("Image count doesn't match the actual number of images!");
	}
	return parse;
}

len_t Parser::ReadBlockLength(len_t current_idx, len_t in_len) {
	len_t block_len = ReadLength(current_idx);
	current_idx += LENGTH_BLOCK_SIZE;
	if (current_idx + block_len > in_len) {
		throw std::out_of_range("Invalid block length: The end of the block is outside of the file.");
	}
	return current_idx;
}

len_t Parser::ParseHeaderBlock(len_t current_idx) {
	std::string filetype(parse->raw_data + current_idx, parse->raw_data + current_idx + FILE_TYPE_SIZE);
	if (filetype != "CAFF")
		throw std::invalid_argument("Invalid file type!");
	current_idx += FILE_TYPE_SIZE;

	len_t header_size = ReadLength(current_idx);
	current_idx += LENGTH_BLOCK_SIZE;
	if (header_size != CAFF_HEADER_SIZE) {
		throw std::out_of_range("The CAFF header has an invalid size.");
	}

	len_t num_anim = ReadLength(current_idx);
	parse->SetImageCount(num_anim);
	current_idx += LENGTH_BLOCK_SIZE;

	return current_idx;
}

len_t Parser::ParseCreditsBlock(len_t current_idx) {
	//TODO: read and parse date
	current_idx += DATE_BLOCK_SIZE;

	len_t creator_len = ReadLength(current_idx);
	if (creator_len > MAX_ALLOWED_CREATOR_LENGTH || creator_len < 0) {
		throw std::out_of_range("Invalid creator length!");
	}
	current_idx += LENGTH_BLOCK_SIZE;


	std::string creator(parse->raw_data + current_idx, parse->raw_data + current_idx + creator_len);
	parse->SetCreatorName(creator);
	current_idx += creator_len;

	return current_idx;
}

len_t Parser::ParseAnimationBlock(len_t current_idx) {
	if (parse->GetImagesCurrentSize() >= parse->GetImageCount()) {
		throw std::overflow_error("Image count doesn't match the actual number of images!");
	}

	len_t duration = ReadLength(current_idx);
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

	len_t header_size = ReadLength(current_idx);
	if (header_size > MAX_ALLOWED_CIFF_HEADER_SIZE)
		throw std::invalid_argument("Invalid image header size!");
	len_t header_end = start_idx + header_size;
	current_idx += LENGTH_BLOCK_SIZE;

	len_t content_size = ReadLength(current_idx);
	if (content_size > MAX_ALLOWED_CIFF_CONTENT_SIZE)
		throw std::invalid_argument("Invalid image content size!");
	current_idx += LENGTH_BLOCK_SIZE;

	len_t width = ReadLength(current_idx);
	current_idx += LENGTH_BLOCK_SIZE;

	len_t height = ReadLength(current_idx);
	current_idx += LENGTH_BLOCK_SIZE;

	if(content_size != (width * height * BYTES_PER_PIXEL))
		throw std::out_of_range("Content size not matching image size!");

	int i = 0;
	while (parse->raw_data[current_idx + i] != '\n') {
		if(((len_t)current_idx + i) > header_end || i > MAX_ALLOWED_CIFF_CAPTION_LENGTH)
			throw std::invalid_argument("Caption must end with a \\n!");
		i++;
	}
	std::string caption(parse->raw_data + current_idx, parse->raw_data + current_idx + i);
	current_idx += (len_t)i + 1;

	ParseImage image = std::make_shared<ImageParseData>(caption, width, height);

	i = 0;
	while (((len_t)current_idx + i) < header_end) {
		int j = 0;
		while (parse->raw_data[current_idx + i + j] != '\0') {
			if (((len_t)current_idx + i + j) > (start_idx + header_size))
				throw std::invalid_argument("Last image tag must end with a \\0!");
			if (i > MAX_ALLOWED_CIFF_TAG_LENGTH)
				throw std::invalid_argument("Image tags must end with a \\0!");
			j++;
		}
		std::string tag(parse->raw_data + current_idx + i, parse->raw_data + current_idx + i + j);
		image->AddTag(tag);
		i += j + 1;
	}
	current_idx += i;

	if (current_idx != header_end)
		throw std::invalid_argument("Invalid image header size!");

	len_t pixel_count = width * height;
	for (i = 0; i < pixel_count; i++) {
		if ((current_idx + BYTES_PER_PIXEL - 1) > parse->raw_data_len)
			throw std::out_of_range("Pixel count not matching image size!");

		Pixel p(parse->raw_data[current_idx], parse->raw_data[current_idx + 1], parse->raw_data[current_idx + 2]);
		image->AddPixel(p);
		current_idx += BYTES_PER_PIXEL;
	}

	image->data_start_idx = start_idx;
	image->data_end_idx = current_idx;

	return image;
}
