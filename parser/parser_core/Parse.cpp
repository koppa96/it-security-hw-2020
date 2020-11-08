#include "Parse.h"
#include "Exceptions.h"

const ParseImage& ParseData::GetPreviewImage() {
	if (images.size() < 1)
		throw caff_parser_exception("Animation contains no images!");

	if (preview_index < 0 || preview_index > images.size())
		preview_index = 0;

	return images[preview_index];
}

void ParseData::AddImage(const ParseImage& image, len_t duration) {
	images.push_back(image);
	if (images.size() > image_count) {
		throw caff_parser_exception("Too many images!");
	}
	if (duration > max_image_duration) {
		max_image_duration = duration;
		preview_index = images.size() - 1;
	}
}

const len_t ParseData::GetImageCount() const {
	return image_count;
}

void ParseData::SetImageCount(len_t img_count) {
	image_count = img_count;
}

const std::string& ParseData::GetCreatorName() const {
	return creator_name;
}

void ParseData::SetCreatorName(std::string name) {
	creator_name = name;
}

size_t ParseData::GetImagesCurrentSize() const {
	return images.size();
}