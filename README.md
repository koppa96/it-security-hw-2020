# CAFF parser

## Building
Normally the program will take a set input and output file and treat it as it's default input/output. If you want to pipe input from/to the standard input/output, read the [Building with input piping](https://github.com/koppa96/it-security-hw-2020/blob/main/README.md#building-with-input-piping-eg-for-fuzzing) paragraph.

### Building with Visual Studio
- Make sure parser_test is the startup project
- Make sure parser_core build output (Properties >> Configuration Properties >> General >> Configuration Type) is set to Static Library.
- Make sure precompiled headers are disabled (Properties >> C/C++ >> Precompiled Headers >> Precompiled Header >> *Not Using Precompiled Headers* and set the value of Precompiled Header File to *empty*)
- Build project.

### Building with Make
- Navigate to the parser directory.
- Normal builds:
  `$ make `
- If you want to build the app for fuzzing:
  `$ make fuzzing`

### Building with input piping (e.g. for fuzzing)
- Open parser/parser_test/parser_test.cpp
- Set the value INOUT_PIPING define at the start of the file to 1
- Save the file and rebuild the project with one of the methods detailed above.

## Running the app

### Running with Visual Studio
- To run the app, simply start it through the Visual Studio Debugger.

### Running with Make
After issuing the make command while sitting in the parser directory:
```sh
$ cd Debug
$ ./caffparser
```
