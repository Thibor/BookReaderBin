using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSProgram
{
	class CRec
	{
		public ulong hash = 0;
		public ushort move = 0;
		public ushort games = 1;
		public uint learn = 0;
		public string umo = string.Empty;
	}

	class CRecList : List<CRec>
	{
		public bool AddRec(CRec rec)
		{
			int index = FindRec(rec);
			if (index == Count)
				Add(rec);
			else
			{
				CRec r = this[index];
				if ((r.hash == rec.hash) && (r.move == rec.move))
				{
					this[index].games++;
					return false;
				}
				else
					Insert(index, rec);
			}
			return true;
		}

		public int GetWeight()
		{
			int result = 0;
			foreach (CRec rec in this)
				result += rec.games;
			return result;
		}

		public int RecDelete(int count)
		{
			int c = Count;
			if ((count == 0) || (count >= Count))
				Clear();
			else
			{
				SortWeight();
				RemoveRange(Count - count, count);
				SortHash();
			}
			return c - Count;
		}

		public void DeleteRec(CRec r)
		{
			for (int n = Count - 1; n >= 0; n--)
			{
				CRec rec = this[n];
				if ((rec.hash == r.hash) && (rec.move == r.move))
					RemoveAt(n);
			}
		}

		public int FindHash(ulong hash)
		{
			int first = -1;
			int last = Count;
			while (true)
			{
				if (last - first == 1)
					return last;
				int middle = (first + last) >> 1;
				CRec rec = this[middle];
				if (hash <= rec.hash)
					last = middle;
				else
					first = middle;
			}
		}

		public int CompareHash(CRec r1, CRec r2)
		{
			if (r1.hash > r2.hash)
				return 1;
			if (r1.hash < r2.hash)
				return -1;
			return r1.move - r2.move;
		}

		public int FindRec(CRec r)
		{
			int first = -1;
			int last = Count;
			while (true)
			{
				if (last - first == 1)
					return last;
				int middle = (first + last) >> 1;
				CRec rec = this[middle];
				if (CompareHash(r, rec) <= 0)
					last = middle;
				else
					first = middle;
			}
		}

		public void SortHash()
		{
			Sort(CompareHash);
		}

		public void SortWeight()
		{
			Sort(delegate (CRec r1, CRec r2)
			{
				return r2.games - r1.games;
			});
		}

	}

}
