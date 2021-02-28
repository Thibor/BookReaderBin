using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using NSUci;
using NSChess;

namespace NSProgram
{
	class Program
	{
		static void Main(string[] args)
		{
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
			CUci Uci = new CUci();
			CPolyglot polyglot = new CPolyglot();
			CChess Chess = CPolyglot.chess;
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
			string book = String.Join(" ", listBn);
			string engine = String.Join(" ", listEf);
			string arguments = String.Join(" ", listEa);
			polyglot.LoadFromFile(book);
			Process myProcess = new Process();
			if (File.Exists(engine))
			{
				myProcess.StartInfo.FileName = engine;
				myProcess.StartInfo.WorkingDirectory = Path.GetDirectoryName(engine);
				myProcess.StartInfo.UseShellExecute = false;
				myProcess.StartInfo.RedirectStandardInput = true;
				myProcess.StartInfo.Arguments = arguments;
				myProcess.Start();
			}
			else
			{
				if (engine != "")
					Console.WriteLine("info string missing engine");
				engine = "";
			}

			while (true)
			{
				string msg = Console.ReadLine();
				Uci.SetMsg(msg);
				if ((Uci.command != "go") && (engine != ""))
					myProcess.StandardInput.WriteLine(msg);
				switch (Uci.command)
				{
					case "position":
						string fen = "";
						int lo = Uci.GetIndex("fen", 0);
						int hi = Uci.GetIndex("moves", Uci.tokens.Length);
						if (lo > 0)
						{
							if (lo > hi)
								hi = Uci.tokens.Length;
							for (int n = lo; n < hi; n++)
							{
								if (n > lo)
									fen += ' ';
								fen += Uci.tokens[n];
							}
						}
						Chess.SetFen(fen);
						lo = Uci.GetIndex("moves", 0);
						hi = Uci.GetIndex("fen", Uci.tokens.Length);
						if (lo > 0)
						{
							if (lo > hi)
								hi = Uci.tokens.Length;
							for (int n = lo; n < hi; n++)
							{
								string m = Uci.tokens[n];
								Chess.MakeMove(Chess.UmoToEmo(m));
							}
						}
						break;
					case "go":
						string move = polyglot.GetMove();
						if (move != String.Empty)
							Console.WriteLine($"bestmove {move}");
						else if (engine == "")
							Console.WriteLine("enginemove");
						else
							myProcess.StandardInput.WriteLine(msg);
						break;
				}
			}
		}
	}
}
