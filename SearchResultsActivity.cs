using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using System;

namespace ZpivejmePanu
{
	[Activity(Label = "Výsledek hledání")]
	[Obsolete]
	public class SearchResultsActivity : ListActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var searchResults = Intent.Extras.GetStringArrayList("searchResults");
			ListAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, searchResults);

			ListView.ItemClick += ItemClick;
		}

		void ItemClick(object sender, AdapterView.ItemClickEventArgs args)
		{
			var packedString = ((TextView)args.View).Text;
			var songNumber = SongData.ExtractSongNumber(packedString);

			var intent = new Intent(this, typeof(SongViewActivity));
			intent.PutExtra("songFile", SongData.FileNameFromSongNumber(songNumber));
			StartActivity(intent);
		}
	}
}