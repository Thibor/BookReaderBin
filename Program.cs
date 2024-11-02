using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using NSUci;
using RapLog;

namespace NSProgram
{
	class Program
	{
		public static bool isLog = false;
		public static CBook book = new CBook();
		public static CRapLog log = new CRapLog();

		static void Main(string[] args)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			bool isInfo = false;
			/// <summary>
			/// Book can write new moves.
			/// </summary>
			bool isW = false;
			/// <summary>
			/// Limit ply to read.
			/// </summary>
			int bookLimitR = 16;
			/// <summary>
			/// Limit ply to write.
			/// </summary>
			int bookLimitW = 16;
			CUci uci = new CUci();
			CChessExt chess = CBook.chess;
			string ax = "-bf";
			List<string> listBf = new List<string>();
			List<string> listEf = new List<string>();
			List<string> listEa = new List<string>();
			for (int n = 0; n < args.Length; n++)
			{
				string ac = args[n];
				switch (ac)
				{
					case "-bf":
					case "-ef":
					case "-ea":
					case "-lr"://limit read in half moves
					case "-lw"://limit write in half moves
						ax = ac;
						break;
					case "-w":
						ax = ac;
						isW = true;
						break;
					case "-log"://add log
						ax = ac;
						isLog = true;
						break;
					case "-info":
						ax = ac;
						isInfo = true;
						break;
					default:
						switch (ax)
						{
							case "-bf":
								listBf.Add(ac);
								break;
							case "-ef":
								listEf.Add(ac);
								break;
							case "-ea":
								listEa.Add(ac);
								break;
							case "-lr":
								bookLimitR = int.TryParse(ac, out int lr) ? lr : 0;
								break;
							case "-lw":
								bookLimitW = int.TryParse(ac, out int lw) ? lw : 0;
								break;
							case "-w":
								ac = ac.Replace("K", "000").Replace("M", "000000");
								book.maxRecords = int.TryParse(ac, out int m) ? m : 0;
								break;
						}
						break;
				}
			}
			string bookFile = String.Join(" ", listBf);
			string engineFile = String.Join(" ", listEf);
			string engineArguments = String.Join(" ", listEa);
			Console.WriteLine($"info string {CBook.name} ver {CBook.version}");
			bool bookLoaded = SetBookFile(bookFile);
			Process engineProcess = null;
			if (File.Exists(engineFile))
			{
				engineProcess = new Process();
				engineProcess.StartInfo.FileName = engineFile;
				engineProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(engineFile);
				engineProcess.StartInfo.UseShellExecute = false;
				engineProcess.StartInfo.RedirectStandardInput = true;
				engineProcess.StartInfo.Arguments = engineArguments;
				engineProcess.Start();
				Console.WriteLine($"info string engine on");
			}
			else
			{
				if (engineFile != String.Empty)
					Console.WriteLine($"info string missing engine  [{engineFile}]");
				engineFile = String.Empty;
			}
			do
			{
				string msg = Console.ReadLine().Trim();
				if ((msg == "help") || (msg == "book"))
				{
					Console.WriteLine("book load [filename].[bin|pgn|uci] - clear and add moves from file");
					Console.WriteLine("book save [filename].[bin|uci] - save book to the file");
					Console.WriteLine("book addfile [filename].[bin|pgn|uci] - add moves to the book");
					Console.WriteLine("book clear - clear all moves from the book");
					Console.WriteLine("book delete [x] - delete x moves from the book");
					Console.WriteLine("book moves [uci] - make sequence of moves and shows possible continuations");
					continue;
				}
				uci.SetMsg(msg);
				if (uci.command == "book")
				{
					if (uci.tokens.Length > 1)
						switch (uci.tokens[1])
						{
							case "addfile":
								book.AddFileInfo(uci.GetValue("addfile"),true);
								break;
							case "adduci":
								book.AddUci(uci.GetValue("adduci"));
								break;
							case "clear":
								book.Clear();
								Console.WriteLine("Book is empty");
								break;
							case "load":
								book.LoadFromFile(uci.GetValue("load"),true);
								break;
							case "save":
								book.SaveToFile(uci.GetValue("save"));
								Console.WriteLine("Book is saved");
								break;
							case "moves":
								book.InfoMoves(uci.GetValue("moves"));
								break;
							case "delete":
								int c = book.Delete(uci.GetInt("delete"));
								Console.WriteLine($"{c:N0} moves was deleted");
								break;
							case "reset":
								book.Reset();
								break;
							case "getoption":
								Console.WriteLine($"option name book_file type string default book{CBook.defExt}");
								Console.WriteLine($"option name write type check default false");
								Console.WriteLine($"option name log type check default false");
								Console.WriteLine($"option name limit_read_ply type spin default {bookLimitR} min 0 max 100");
								Console.WriteLine($"option name limit_write_ply type spin default {bookLimitW} min 0 max 100");
								Console.WriteLine("optionend");
								break;
							case "setoption":
								switch (uci.GetValue("name", "value").ToLower())
								{
									case "book_file":
										SetBookFile(uci.GetValue("value"));
										break;
									case "write":
										isW = uci.GetValue("value") == "true";
										break;
									case "log":
										isLog = uci.GetValue("value") == "true";
										break;
									case "limit_read_ply":
										bookLimitR = uci.GetInt("value");
										break;
									case "limit_write_ply":
										bookLimitW = uci.GetInt("value");
										break;
								}
								break;
							default:
								Console.WriteLine($"Unknown command [{uci.tokens[1]}]");
								break;
						}
					continue;
				}
				if ((uci.command != "go") && !String.IsNullOrEmpty(engineFile))
					engineProcess.StandardInput.WriteLine(msg);
				switch (uci.command)
				{
					case "position":
						string fen = uci.GetValue("fen", "moves");
						string moves = uci.GetValue("moves","fen");
						chess.SetFen(fen);
						chess.MakeMoves(moves);
						if (isW && bookLoaded && String.IsNullOrEmpty(fen) && chess.Is2ToEnd(out string myMove, out string enMove))
						{
							book.AddUci($"{moves} {myMove} {enMove}", bookLimitW);
							book.SaveToFile();
						}
						break;
					case "go":
						string move = String.Empty;
						if ((bookLimitR == 0) || (bookLimitR > chess.halfMove))
						{
							move = book.GetMove();
							if (!chess.IsValidMove(move, out _))
								move = String.Empty;
						}
						if (!String.IsNullOrEmpty(move))
							Console.WriteLine($"bestmove {move}");
						else if (engineProcess == null)
							Console.WriteLine("enginemove");
						else
							engineProcess.StandardInput.WriteLine(msg);
						break;
				}
			} while (uci.command != "quit");


			bool SetBookFile(string bn)
			{
				bookFile = bn;
				bookLoaded = book.LoadFromFile(bookFile);
				if (bookLoaded)
				{
					if ((book.recList.Count > 0) && File.Exists(bookFile))
						Console.WriteLine($"info string book on {book.recList.Count:N0} moves 128 bpm");
					if (isW)
						Console.WriteLine($"info string write on");
					if (isInfo)
						book.ShowInfo();
				}
				else
					isW = false;
				return bookLoaded;
			}
		}

	}
}
