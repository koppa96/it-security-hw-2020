#pragma once

#include "Defines.h"
#include "ParseImage.h"

class ImageSaver {
	unsigned char* imagedata;

	void GenerateBitmapHeader(len_t image_size);
	void GenerateInformationsHeader(ParseImage pi);
	void WriteInt(len_t starting_idx, unsigned int number);
	void WriteInt(len_t starting_idx, len_t number) {
		WriteInt(starting_idx, (unsigned int)number);
	}
	void FillWithZeros(len_t from, len_t to);
	void WriteBitmap(const std::vector<Pixel>& pixels, len_t image_size);
	void SaveFile(std::string filename, len_t image_size);
public:
	std::string SavePreview(ParseImage image);

	ImageSaver() = default;

	~ImageSaver() {
		delete[] imagedata;
	}
};