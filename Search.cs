using System.Collections.Generic;

namespace ZpivejmePanu
{
	public partial class Search
	{
		private List<SongData> allSongs;

		public List<SongData> FindSongs(string pattern)
		{
			List<SongData> results = new List<SongData>();

			foreach (var song in allSongs)
			{
				if (song.Match(pattern))
					results.Add(song);
			}

			return results;
		}

		public List<SongData> GetAllSongs() => allSongs;
	}
}
