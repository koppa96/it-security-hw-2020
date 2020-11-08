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