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
				else if (hash > rec.hash)
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

	}

}
