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
		public ushort weight = 1;
		public uint learn = 0;
	}

	class CRecList : List<CRec>
	{

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
			for(int n= Count - 1; n >= 0; n--)
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

		public void SortHash()
		{
			Sort(delegate (CRec r1, CRec r2)
			{
				if (r1.hash > r2.hash)
					return 1;
				if (r1.hash < r2.hash)
					return -1;
				return r1.move - r2.move;
			});
		}

		public void SortWeight()
		{
			Sort(delegate (CRec r1, CRec r2)
			{
				return r2.weight - r1.weight;
			});
		}

	}

}
