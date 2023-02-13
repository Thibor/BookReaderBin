# BookReaderBin
BoookReaderBin can be used as normal UCI chess engine in chess GUI like Arena.
This program reads chess openings moves from a polyglot openig book with bin extension.
To use this program you need install  <a href="https://dotnet.microsoft.com/download/dotnet-framework/net48">.NET Framework 4.8</a>

## Parameters

**-bf** chess opening Book File name<br/>
**-ef** chess Engine File name<br/>
**-ea** chess Engine Arguments<br/>
**-w** Write new moves to the book<br/>
**-lr** Limit maximum ply depth when Read from book (default 16) 0 means no limit<br/>
**-lw** Limit maximum ply depth when Write to book (default 16) 0 means no limit<br/>
**-log** create LOG file<br/>
**-info** show additional INFOrmation<br/>

## Console commands

**help** - displays basic commands<br/>
**book load** [filename].[bin|uci|pgn] - clear and add<br/>
**book save** [filename].[bin|uci|pgn] - save book to the file<br/>
**book addfile** [filename].[bin|uci|pgn] - adds moves from another book<br/>
**book clear** - clear all moves from the book<br/>
**book delete [x]** - delete x moves from the book<br/>
**book reset** - resetting the book may remove some moves<br/>
**book moves [uci]** - make sequence of moves in uci format and shows possible continuations<br/>
**book getoption** - show options<br/>
**book setoption name [option name] value [option value]** - set option<br/>
**quit** quit the program as soon as possible

### Examples

-bn book.bin -ef stockfish.exe<br/>
book.bin -ef stockfish.exe

The program will first try to find move in chess opening book named book.bin, and if it doesn't find any move in it, it will run a chess engine named stockfish.exe 


