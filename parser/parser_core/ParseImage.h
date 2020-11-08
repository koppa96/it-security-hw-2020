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
	len_t data_start_idx = -1;
	len_t data_end_idx = -1;

	ImageParseData(const std::string& caption, len_t width, len_t height);

	void AddTag(const std::string& tag);

	void AddPixel(const Pixel& pixel);

	std::vector<Pixel> GetPixels();

	const len_t GetWidth() const;

	const len_t GetHeight() const;
};

using ParseImage = std::shared_ptr<ImageParseData>;