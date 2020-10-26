#pragma once

#include <memory>

class ParseData {
	const char* raw_data;
	const int raw_data_len;
public:
	ParseData(const char* in_buffer, int in_len) : raw_data{ in_buffer }, raw_data_len{ in_len } {

	}

	int GetPreview(char* out_buffer, int out_len) {
		return 0;
	}
};

using Parse = std::shared_ptr<ParseData>;