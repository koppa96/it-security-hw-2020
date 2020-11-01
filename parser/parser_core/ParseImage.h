#pragma once

#include "Defines.h"
#include "Pixel.h"
#include <memory>
#include <string>
#include <vector>
#include <stdexcept>

class ImageParseData {
	std::vector<std::string> tags;
	const len_t width;
	const len_t height;
	const len_t image_size;
	const std::string caption;
	std::vector<Pixel> pixels;
public:
	len_t data_start_idx;
	len_t data_end_idx;

	ImageParseData(const std::string& caption, len_t width, len_t height) : caption{ caption }, width{ width }, height{ height }, image_size{ width * height } {
		pixels.reserve(image_size);
	}

	void AddTag(const std::string& tag) {
		tags.push_back(tag);
	}

	void AddPixel(const Pixel& pixel) {
		if(pixels.size() >= image_size)
			throw std::out_of_range("Attempted to add pixel above image size!");

		pixels.push_back(pixel);
	}
};

using ParseImage = std::shared_ptr<ImageParseData>;