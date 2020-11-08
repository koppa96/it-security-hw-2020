CXX = g++
AFL_CXX = afl-g++

compilation:
	${CXX} parser_core/*.cpp parser_test/*.cpp -o Debug/caffparser

fuzzing:
	${AFL_CXX} parser_core/*cpp parser_test/*.cpp -o Debug/afl_caffparser

clean:
	rm Debug/caffparser
	rm Debug/afl_caffparser