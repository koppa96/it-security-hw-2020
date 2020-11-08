#include "ImageBuilder.h"
#include <vector>
#include <fstream>

Preview ImageBuilder::BuildPreview(ParseImage pi) {
	auto pixels = pi->GetPixels();
	auto zero_pad_count = (pi->GetWidth() * BYTES_PER_PIXEL) % BMP_REQUIRED_PAD_SIZE_PER_ROW;
	len_t size = BMP_HEADER_SIZE + (len_t)pixels.size() * BYTES_PER_PIXEL + pi->GetHeight() * zero_pad_count;
	imagedata = new unsigned char[size];
	GenerateBitmapHeader(size);
	GenerateInformationsHeader(pi);
	WriteBitmap(pixels, pi->GetWidth(), pi->GetHeight(), zero_pad_count);
	auto image_out = reinterpret_cast<char*>(imagedata);
	Preview data = std::make_shared<PreviewData>(image_out, size);
	return data;
}

void ImageBuilder::GenerateBitmapHeader(len_t image_size) {
	//Signature
	imagedata[0] = 'B';
	imagedata[1] = 'M';
	
	//Size
	WriteInt(2, image_size);

	//0000
	FillWithZeros(6, 10);

	//Bitmap start (54)
	WriteInt(10, (unsigned int)54);
}

void ImageBuilder::WriteInt(len_t starting_idx, unsigned int number) {
	for (int i = 0; i < 4; i++) {
		imagedata[starting_idx + i] = (char)(number >> i * 8) & 0x000000ff;
	}
}

/// <param name="from">Starting index (inclusive)</param>
/// <param name="to">Ending index (exclusive)</param>
void ImageBuilder::FillWithZeros(len_t from, len_t to) {
	for (int i = from; i < to; i++) {
		imagedata[i] = 0;
	}
}

void ImageBuilder::GenerateInformationsHeader(ParseImage pi) {
	//Header size (40)
	WriteInt(14, (unsigned int)40);

	//Width
	WriteInt(18, pi->GetWidth());

	//Height
	WriteInt(22, pi->GetHeight());

	//Output peripheral (1)
	imagedata[26] = 1;
	imagedata[27] = 0;

	//Bit depth (RGB = 24 = 00011000)
	imagedata[28] = 24;
	imagedata[29] = 0;

	//30-33: Compression (no compression = 0)
	//35-37: Bitmap size or 0 if no compression => 0000
	FillWithZeros(30, 38);

	//1 dpi ~ 39,37 dpm
	//96x96 dpi ~ 3780x3780 dpm
	//Horizontal resolution in pixel/meter
	WriteInt(38, (unsigned int)3780);
	//Vertical resolution in pixel/meter
	WriteInt(42, (unsigned int)3780);

	//46-49: Palette color count (no palette for RGB color space, so 0)
	//50-53: Palette used color count (again 0, since no palette)
	FillWithZeros(46, 54);
}

void ImageBuilder::WriteBitmap(const std::vector<Pixel>& pixels, len_t image_width, len_t image_height, len_t zero_pad_count) {
	auto full_width = image_width * BYTES_PER_PIXEL + zero_pad_count;
	for (int i = 0; i < image_height; i++) {	//sorok
		for (int j = 0; j < image_width; j++) {	//oszlopok
			len_t base_idx = BMP_HEADER_SIZE + i * full_width + (len_t)j * BYTES_PER_PIXEL;
			len_t pixel_idx = (image_height - i - 1) * image_width + j;
			//len_t pixel_idx = i * image_width + j;
			imagedata[base_idx] = pixels[pixel_idx].b;
			imagedata[base_idx + 1] = pixels[pixel_idx].g;
			imagedata[base_idx + 2] = pixels[pixel_idx].r;
		}
		for (int k = 0; k < zero_pad_count; k++) {
			imagedata[((len_t)i + 1) * image_width + k] = 0;
		}
	}
}