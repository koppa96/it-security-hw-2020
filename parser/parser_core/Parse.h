#pragma once

#include <memory>
#include <string>

class ParseData {
	int image_count = 0;
	std::string creator_name;
public:
	const char* raw_data;
	const int raw_data_len;

	ParseData(const char* in_buffer, int in_len) : raw_data{ in_buffer }, raw_data_len{ in_len } {

	}

	int GetPreview(char* out_buffer, int out_len) {
		return 0;
	}

	//Getters/setters:

	const int GetImageCount() const {
		return image_count;
	}

	void SetImageCount(int img_count) {
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