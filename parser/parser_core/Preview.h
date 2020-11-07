#pragma once

#include "Defines.h"
#include <memory>

struct PreviewData {
	char* data;
	len_t length;

	PreviewData(char* data, len_t length) : data { data }, length { length }
	{
	}

	~PreviewData() {
		delete[] data;
	}
};

using Preview = std::shared_ptr<PreviewData>;