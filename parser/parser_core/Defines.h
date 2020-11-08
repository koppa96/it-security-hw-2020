#pragma once

#define len_t unsigned long long

constexpr auto LENGTH_BLOCK_SIZE = 8;
constexpr auto FILE_TYPE_SIZE = 4;
constexpr auto DATE_BLOCK_SIZE = 6;
constexpr auto BYTES_PER_PIXEL = 3;
constexpr auto BMP_HEADER_SIZE = 54;
constexpr auto CAFF_HEADER_SIZE = 20;
constexpr auto CAFF_HEADER_BLOCK_TYPE = 1;
constexpr auto BMP_REQUIRED_PAD_SIZE_PER_ROW = 4;
constexpr auto MAX_ALLOWED_CREATOR_LENGTH = 200;
constexpr auto MAX_ALLOWED_CIFF_CAPTION_LENGTH = 500;
constexpr auto MAX_ALLOWED_CIFF_TAG_LENGTH = 100;
constexpr auto MAX_ALLOWED_CIFF_HEADER_SIZE = 4000;
constexpr auto MAX_ALLOWED_CIFF_CONTENT_SIZE = 60000000; //~60MB ~4472x4472px