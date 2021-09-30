# BookReaderBin
BoookReaderBin can be used as normal UCI chess engine in chess GUI like Arena.
This program reads chess openings moves from a polyglot openig book with bin extension.
To use this program you need install  <a href="https://dotnet.microsoft.com/download/dotnet-framework/net48">.NET Framework 4.8</a>

## Parameters

**-bn** polyglot opening Book file Name<br/>
**-ef** chess Engine File name<br/>
**-ea** chess Engine Arguments<br/>
**-lr** Limit maximum ply depth when Read from book (default 32) 0 means no limit<br/>
**-lw** Limit maximum ply depth when Write to book (default 32) 0 means no limit<br/>

### Examples

-bn book.bin -ef stockfish.exe<br/>
book.bin -ef stockfish.exe

The program will first try to find move in chess opening book named book.bin, and if it doesn't find any move in it, it will run a chess engine named stockfish.exe 


