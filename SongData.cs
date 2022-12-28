using System;
using System.Globalization;

namespace ZpivejmePanu
{
	public class SongData
	{
		string number;
		string title;
		string author;
		string source;
		string file_name;

		public SongData(string number, string title, string file_name = null)
		{
			this.number = number;
			this.title = title;
			author = "";
			source = "";
			this.file_name = file_name ?? FileNameFromSongNumber(number);
		}

		public string GetNumber() => number;
		public string GetTitle() => title;
		public string GetAuthor() => author;
		public string GetSource() => source;
		public string GetFileName() => file_name;

		private static bool IsMatch(string whereToSearch, string whatToSearch)
		{
			if (whatToSearch.Length == 0)
				return true;

			for (int i = 0; i < whereToSearch.Length - whatToSearch.Length + 1; ++i)
			{
				if (string.Compare(whereToSearch.Substring(i, whatToSearch.Length), whatToSearch,
						CultureInfo.CurrentCulture, CompareOptions.IgnoreNonSpace | CompareOptions.IgnoreCase) == 0)
					return true;
			}

			return false;
		}

		public bool Match(string key)
		{
			if (string.Equals(key, number, StringComparison.OrdinalIgnoreCase))
				return true;

			if (IsMatch(title, key))
				return true;

			if (IsMatch(author, key))
				return true;

			if (IsMatch(source, key))
				return true;

			return false;
		}

		public override string ToString()
		{
			string s = number + ": " + title;
			if (author != "")
				s += $" ({author})";
			if (source != "")
				s += $" [{source}]";

			return s;
		}

        public static string ExtractSongNumber(string s)
        {
            return s.Split(':')[0];
        }

        public static string FileNameFromSongNumber(string songNumber)
        {
            return songNumber + ".html";
        }
	}
}
