using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using NSUci;

namespace NSProgram
{
	class Program
	{
		static void Main(string[] args)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			bool isWritable = false;
			CUci Uci = new CUci();
			CPolyglot Book = new CPolyglot();
			CChessExt chess = CPolyglot.chess;
			string ax = "-bn";
			List<string> listBn = new List<string>();
			List<string> listEf = new List<string>();
			List<string> listEa = new List<string>();
			for (int n = 0; n < args.Length; n++)
			{
				string ac = args[n];
				switch (ac)
				{
					case "-bn":
					case "-ef":
					case "-ea":
						ax = ac;
						break;
					case "-w":
						ax = ac;
						isWritable = true;
						break;
					default:
						switch (ax)
						{
							case "-bn":
								listBn.Add(ac);
								break;
							case "-ef":
								listEf.Add(ac);
								break;
							case "-ea":
								listEa.Add(ac);
								break;
						}
						break;
				}
			}
			string bookName = String.Join(" ", listBn);
			string engineName = String.Join(" ", listEf);
			string arguments = String.Join(" ", listEa);
			Process myProcess = new Process();
			if (File.Exists(engineName))
			{
				myProcess.StartInfo.FileName = engineName;
				myProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(engineName);
				myProcess.StartInfo.UseShellExecute = false;
				myProcess.StartInfo.RedirectStandardInput = true;
				myProcess.StartInfo.Arguments = arguments;
				myProcess.Start();
			}
			else
			{
				if (engineName != "")
					Console.WriteLine($"info string missing engine  [{engineName}]");
				engineName = "";
			}
			if (!Book.LoadFromFile(bookName))
				Book.LoadFromFile($"{bookName}{CPolyglot.defExt}");
			Console.WriteLine($"info string book {Book.recList.Count:N0} moves");
			while (true)
			{
				string msg = Console.ReadLine().Trim();
				if ((msg == "help") || (msg == "book"))
				{
					Console.WriteLine("book load [filename].[bin|uci] - clear and add moves from file");
					Console.WriteLine("book save [filename].[bin] - save book to the file");
					Console.WriteLine("book addfile [filename].[bin|uci] - add moves to the book");
					Console.WriteLine("book clear - clear all moves from the book");
					continue;
				}
				Uci.SetMsg(msg);
				if (Uci.command == "book")
				{
					if (Uci.tokens.Length > 1)
						switch (Uci.tokens[1])
						{
							case "addfile":
								if (!Book.AddFile(Uci.GetValue(2, 0)))
									Console.WriteLine("File not found");
								else
									Book.ShowMoves(true);
								break;
							case "adduci":
								string movesUci = Uci.GetValue(2, 0);
								Book.AddUci(movesUci);
								break;
							case "clear":
								Book.Clear();
								break;
							case "load":
								if (!Book.LoadFromFile(Uci.GetValue(2, 0)))
									Console.WriteLine("File not found");
								else
									Book.ShowMoves(true);
								break;
							case "save":
								Book.SaveToFile(Uci.GetValue(2, 0));
								break;
							default:
								Console.WriteLine($"Unknown command [{Uci.tokens[1]}]");
								break;
						}
					continue;
				}
				if ((Uci.command != "go") && !String.IsNullOrEmpty(engineName))
					myProcess.StandardInput.WriteLine(msg);
				switch (Uci.command)
				{
					case "position":
						List<string> movesUci = new List<string>();
						string fen = Uci.GetValue("fen", "moves");
						chess.SetFen(fen);
						int lo = Uci.GetIndex("moves");
						if (lo++ > 0)
						{
							int hi = Uci.GetIndex("fen", Uci.tokens.Length);
							if (hi < lo)
								hi = Uci.tokens.Length;
							for (int n = lo; n < hi; n++)
							{
								string m = Uci.tokens[n];
								movesUci.Add(m);
								chess.MakeMove(m, out _);
							}
						}
						if (isWritable && String.IsNullOrEmpty(fen) && chess.Is2ToEnd(out string myMove, out string enMove))
						{
							movesUci.Add(myMove);
							movesUci.Add(enMove);
							Book.AddUci(movesUci);
							Book.SaveToFile();
						}
						break;
					case "go":
						string move = Book.GetMove();
						if (!String.IsNullOrEmpty(move))
							Console.WriteLine($"bestmove {move}");
						else if (engineName == "")
							Console.WriteLine("enginemove");
						else
							myProcess.StandardInput.WriteLine(msg);
						break;
				}
			}
		}
	}
}
