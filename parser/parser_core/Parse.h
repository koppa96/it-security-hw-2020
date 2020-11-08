#pragma once

#include <memory>
#include <string>
#include <vector>
#include "ParseImage.h"

class ParseData {
	len_t image_count = 0;
	std::string creator_name;
	std::vector<ParseImage> images;
	len_t max_image_duration = -1;
	len_t preview_index = -1;
public:
	const unsigned char* raw_data;
	const len_t raw_data_len;

	ParseData(const unsigned char* in_buffer, len_t in_len) : raw_data{ in_buffer }, raw_data_len{ in_len } { }

	const ParseImage& GetPreviewImage();

	void AddImage(const ParseImage& image, len_t duration);

	const len_t GetImageCount() const;
	void SetImageCount(len_t img_count);

	const std::string& GetCreatorName() const;
	void SetCreatorName(std::string name);
};

using Parse = std::shared_ptr<ParseData>;