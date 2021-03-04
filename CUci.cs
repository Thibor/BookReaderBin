using System;

namespace NSUci
{
	class CUci
	{
		public string command;
		public string[] tokens;

		public int GetIndex(string key, int def)
		{
			for (int n = 0; n < tokens.Length; n++)
				if (tokens[n] == key)
					return n;
			return def;
		}

		public int GetInt(string key, int def)
		{
			for (int n = 0; n < tokens.Length - 1; n++)
				if (tokens[n] == key)
					if (Int32.TryParse(tokens[n + 1], out int result))
						return result;
			return def;
		}

		public bool GetValue(string name, out string value)
		{
			int i = GetIndex(name, tokens.Length) + 1;
			if (i < tokens.Length)
			{
				value = tokens[i];
				return true;
			}
			value = "";
			return false;
		}

		public string GetValue(string start, string end)
		{
			int istart = GetIndex(start, tokens.Length);
			int iend = GetIndex(end, tokens.Length);
			return GetValue(istart+1,iend-1);
		}

		public string GetValue(int start, int end)
		{
			if (end < start)
				end = tokens.Length - 1;
			string value = String.Empty;
			for (int n = start; n <= end; n++)
			{
				if (n >= tokens.Length)
					break;
				value += $" {tokens[n]}";
			}
			return value.Trim();
		}

		public string Last()
		{
			if (tokens.Length > 0)
				return tokens[tokens.Length - 1];
			return "";
		}

		public void SetMsg(string msg)
		{
			if (msg == null)
				msg = "";
			tokens = msg.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			command = tokens.Length > 0 ? tokens[0] : "";
		}
	}
}
