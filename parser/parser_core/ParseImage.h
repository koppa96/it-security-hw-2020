#pragma once

#include "Pixel.h"
#include <memory>
#include <string>
#include <vector>
#include <stdexcept>

class ImageParseData {
	std::vector<std::string> tags;
	const int width;
	const int height;
	const int image_size;
	const std::string caption;
	std::vector<Pixel> pixels;
public:
	int data_start_idx;
	int data_end_idx;

	ImageParseData(const std::string& caption, int width, int height) : caption{ caption }, width{ width }, height{ height }, image_size{ width * height } {
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