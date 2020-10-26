#pragma once

#include <memory>

class ParseData {
	int image_count = 0;
public:
	const char* raw_data;
	const int raw_data_len;

	ParseData(const char* in_buffer, int in_len) : raw_data{ in_buffer }, raw_data_len{ in_len } {

	}

	int GetPreview(char* out_buffer, int out_len) {
		return 0;
	}

	int GetImageCount() {
		return image_count;
	}

	void SetImageCount(int img_count) {
		image_count = img_count;
	}
};

using Parse = std::shared_ptr<ParseData>;