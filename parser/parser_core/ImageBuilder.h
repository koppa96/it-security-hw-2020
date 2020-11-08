#pragma once

#include "Defines.h"
#include "ParseImage.h"
#include "Preview.h"

class ImageBuilder {
	unsigned char* imagedata;

	void GenerateBitmapHeader(len_t image_size);
	void GenerateInformationsHeader(ParseImage pi);
	void WriteInt(len_t starting_idx, unsigned int number);
	void WriteInt(len_t starting_idx, len_t number);
	void FillWithZeros(len_t from, len_t to);
	void WriteBitmap(const std::vector<Pixel>& pixels, len_t image_width, len_t image_height, len_t zero_pad_count);
public:
	Preview BuildPreview(ParseImage image);

	ImageBuilder() = default;
};