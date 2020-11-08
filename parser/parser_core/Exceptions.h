#pragma once
#include <exception>
#include <string>

class caff_parser_exception : public std::exception {
	std::string message;
public:
	caff_parser_exception(std::string message) : message(message), std::exception() {}

	std::string getMessage() const;
};