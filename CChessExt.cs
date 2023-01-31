using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NSChess;

namespace NSProgram
{
	class CChessExt : CChess
	{

		public int MoveProgress(string umo, bool wt)
		{
			UmoToSD(umo, out int s, out int d);
			int sx = s % 8;
			int dx = d % 8;
			int sy = s / 8;
			int dy = d / 8;
			if (wt)
				(sy, dy) = (dy, sy);
			if (dy < sy)
				return -1;
			if (dy > sy)
				return 1;
			double ms = Math.Abs(sx - 3.5);
			double md = Math.Abs(dx - 3.5);
			if (ms < md)
				return -1;
			if (ms > md)
				return 1;
			return sy * 8 + sx > dy * 8 + dx ? -1 : 1;
		}

		public bool Is2ToEnd(out string myMov, out string enMov)
		{
			myMov = "";
			enMov = "";
			List<int> mu1 = GenerateValidMoves(out _);//my last move
			foreach (int myMove in mu1)
			{
				bool myEscape = true;
				MakeMove(myMove);
				List<int> mu2 = GenerateValidMoves(out _);//enemy mat move
				foreach (int enMove in mu2)
				{
					bool enAttack = false;
					MakeMove(enMove);
					List<int> mu3 = GenerateValidMoves(out bool mate);//my illegal move
					if (mate)
					{
						myEscape = false;
						enAttack = true;
						myMov = EmoToUmo(myMove);
						enMov = EmoToUmo(enMove);
					}
					UnmakeMove(enMove);
					if (enAttack)
						continue;
				}
				UnmakeMove(myMove);
				if (myEscape)
					return false;
			}
			return true;
		}

	}
}
