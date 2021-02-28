# BookReaderCdb
BoookReaderCdb can be used as normal UCI chess engine in chess GUI like Arena.
This program reads chess openings moves from the server <a href="https://www.chessdb.cn/cloudbookc_api_en.html">www.chessdb.cn</a>.
To use this program you need install  <a href="https://dotnet.microsoft.com/download/dotnet-framework/net48">.NET Framework 4.8</a>

## Parameters

**-ef** chess Engine File name<br/>
**-ea** chess Engine Arguments<br/>

### Examples

-ef stockfish.exe<br/>
stockfish.exe

The program will first try to find move in <a href="https://www.chessdb.cn/cloudbookc_api_en.html">www.chessdb.cn</a> database, and if it doesn't find any move in it, it will run a chess engine called stockfish.exe 


