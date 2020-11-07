#pragma once

#include "ParseImage.h"
#include <memory>
#include <string>
#include <vector>

class ParseData {
	len_t image_count = 0;
	std::string creator_name;
	std::vector<ParseImage> images;
	len_t max_image_duration = -1;
	len_t preview_index = -1;
public:
	const unsigned char* raw_data;
	const len_t raw_data_len;

	ParseData(const unsigned char* in_buffer, len_t in_len) : raw_data{ in_buffer }, raw_data_len{ in_len } {

	}

	const ParseImage& GetPreviewImage() {
		if (images.size() < 1)
			throw std::underflow_error("Animation contains no images!");

		if (preview_index < 0 || preview_index > images.size())
			preview_index = 0;

		return images[preview_index];
	}

	void AddImage(const ParseImage& image, len_t duration) {
		images.push_back(image);
		if (duration > max_image_duration) {
			max_image_duration = duration;
			preview_index = images.size() - 1;
		}
	}

	//Getters/setters:

	const len_t GetImageCount() const {
		return image_count;
	}

	void SetImageCount(len_t img_count) {
		image_count = img_count;
	}

	const std::string& GetCreatorName() const {
		return creator_name;
	}

	void SetCreatorName(std::string name) {
		creator_name = name;
	}
};

using Parse = std::shared_ptr<ParseData>;