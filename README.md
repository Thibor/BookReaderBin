# BookReaderBin
BoookReaderBin can be used as normal UCI chess engine in chess GUI like Arena.
This program reads chess openings moves from a polyglot openig book with bin extension.
To use this program you need install  <a href="https://dotnet.microsoft.com/download/dotnet-framework/net48">.NET Framework 4.8</a>

## Parameters

**-bf** chess opening Book File name<br/>
**-ef** chess Engine File name<br/>
**-ea** chess Engine Arguments<br/>
**-w** Write new moves to the book<br/>
**-lr** Limit maximum ply depth when Read from book (default 32) 0 means no limit<br/>
**-lw** Limit maximum ply depth when Write to book (default 32) 0 means no limit<br/>
**-log** Create LOG file<br/>
**-info** show additional INFOrmation<br/>

## Console commands

**book load** [filename].[bin|uci|pgn] - clear and add<br/>
**book save** [filename].[bin] - save book to the file<br/>
**book addfile** [filename].[bin|uci|pgn] - adds moves from another book<br/>
**book delete** [number x] - delete x moves from the book<br/>
**book clear** - clear all moves from the book<br/>
**book adduci** [uci] - adds a sequence of moves in uci format<br/>
**book moves** [uci] - make sequence of moves in uci format and shows possible continuations<br/>
**book getoption** - show options<br/>
**book setoption name [option name] value [option value]** - set option<br/>
**quit** quit the program as soon as possible

### Examples

-bn book.bin -ef stockfish.exe<br/>
book.bin -ef stockfish.exe

The program will first try to find move in chess opening book named book.bin, and if it doesn't find any move in it, it will run a chess engine named stockfish.exe 


