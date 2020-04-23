# WikiTemplate Parser
This program works with the WikiResRequest program.  It uses the output from the WikiRestRequest (a text file) and parses the contents (which is in Wikipedia's Template format) and gathers episode details and stores them in list in runtime.

This program is a practice in the following topics:
* File reading.
* Parsing (specifically for Wikipedia files).
* Regular Expressions (aka regexes).
* Clean code.

## Example use:
This will require a file created from the WikiRestRequest program.  Place the text file in the same location as this program and it will store the episode and its details in `seasonList`.