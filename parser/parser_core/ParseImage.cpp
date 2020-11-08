#include "ParseImage.h"


ImageParseData::ImageParseData(const std::string& caption, len_t width, len_t height) : caption{ caption }, width{ width }, height{ height }, image_size{ width * height } {
	pixels.reserve(image_size);
}

void ImageParseData::AddTag(const std::string& tag) {
	tags.push_back(tag);
}

void ImageParseData::AddPixel(const Pixel& pixel) {
	if (pixels.size() >= image_size)
		throw std::out_of_range("Attempted to add pixel above image size!");

	pixels.push_back(pixel);
}

std::vector<Pixel> ImageParseData::GetPixels() {
	return pixels;
}

const len_t ImageParseData::GetWidth() const {
	return width;
}

const len_t ImageParseData::GetHeight() const {
	return height;
}