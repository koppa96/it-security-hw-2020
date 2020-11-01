#include "pch.h"
#include "ImageSaver.h"
#include <vector>
#include <fstream>

#include <iostream>

std::string ImageSaver::SavePreview(ParseImage pi) {
	auto pixels = pi->GetPixels();
	len_t size = BMP_HEADER_SIZE + (len_t)pixels.size() * BYTES_PER_PIXEL;
	imagedata = new unsigned char[size];
	GenerateBitmapHeader(size);
	GenerateInformationsHeader(pi);
	WriteBitmap(pixels, size);
	std::string filename(FILE_OUTPUT_DIR);
	filename += "asd.bmp";
	SaveFile(filename, size);
	return filename;
}

void ImageSaver::GenerateBitmapHeader(len_t image_size) {
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

void ImageSaver::WriteInt(len_t starting_idx, unsigned int number) {
	for (int i = 0; i < 4; i++) {
		imagedata[starting_idx + i] = (char)(number >> i * 8) & 0x000000ff;
	}
}

/// <param name="from">Starting index (inclusive)</param>
/// <param name="to">Ending index (exclusive)</param>
void ImageSaver::FillWithZeros(len_t from, len_t to) {
	for (int i = from; i < to; i++) {
		imagedata[i] = 0;
	}
}

void ImageSaver::GenerateInformationsHeader(ParseImage pi) {
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

void ImageSaver::WriteBitmap(const std::vector<Pixel>& pixels, len_t image_size) {
	len_t base_idx;
	auto n = pixels.size();
	for (int i = 0; i < n; i++) {
		//len_t base_idx = image_size - (len_t)i * BYTES_PER_PIXEL - 1;
		base_idx = (len_t)i * BYTES_PER_PIXEL + BMP_HEADER_SIZE;
		//imagedata[base_idx] = pixels[n - i - 1].b;
		//imagedata[base_idx + 1] = pixels[n - i - 1].g;
		//imagedata[base_idx + 2] = pixels[n - i - 1].r;
		imagedata[base_idx] = pixels[i].b;
		imagedata[base_idx + 1] = pixels[i].g;
		imagedata[base_idx + 2] = pixels[i].r;
	}
	std::cout << image_size;
}

void ImageSaver::SaveFile(std::string filename, len_t image_size) {
	std::ofstream image_out(filename);
	const char* out_data = reinterpret_cast<const char*>(imagedata);
	image_out.write(out_data, image_size);
	image_out.close();
}