using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System.Linq;
using Xamarin.Essentials;

namespace ZpivejmePanu
{
	[Activity(Label = "@string/app_name", Icon = "@drawable/icon", Theme = "@style/AppTheme", MainLauncher = true)]
	public class MainActivity : AppCompatActivity
	{
		EditText selectionText;
		Button searchButton;
		Button showAllButton;
		Button helpButton;
		TextView versionTextView;
		
		Search search = new Search();

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			Xamarin.Essentials.Platform.Init(this, savedInstanceState);

			SetContentView(Resource.Layout.activity_main);

			selectionText = FindViewById<EditText>(Resource.Id.SelectionText);
			searchButton = FindViewById<Button>(Resource.Id.SearchButton);
			showAllButton = FindViewById<Button>(Resource.Id.ShowAllButton);
			helpButton = FindViewById<Button>(Resource.Id.HelpButton);
			versionTextView = FindViewById<TextView>(Resource.Id.VersionTextView);

			searchButton.Click += SearchButton_Click;
			showAllButton.Click += ShowAllButton_Click;
			helpButton.Click += HelpButton_Click;

			versionTextView.Text = "verze " + VersionTracking.CurrentVersion;
		}

		private void SearchButton_Click(object sender, object e)
		{
			DoSearch(selectionText.Text);

			// reset text before next search
			selectionText.Text = "";
		}

		private void ShowAllButton_Click(object sender, object e)
		{
			DoSearch("");
		}
		
		private void DoSearch(string pattern)
		{
			var results = search.FindSongs(pattern);
			
			// nothing found -> show error
			if (results.Count == 0)
			{
				var toast = Toast.MakeText(Application, "Nic nebylo nalezeno.", ToastLength.Long);
				toast.SetGravity(Android.Views.GravityFlags.Top, 0, 0);
				toast.Show();
				return;
			}

			// exactly one result -> display the song
			if (results.Count == 1)
			{
				var intentd = new Intent(this, typeof(SongViewActivity));
				intentd.PutExtra("songFile", results[0].GetFileName());
				StartActivity(intentd);
				return;
			}

			// multiple results -> show the list
			var intent = new Intent(this, typeof(SearchResultsActivity));
			intent.PutStringArrayListExtra("searchResults", results.Select(item => item.ToString()).ToList());
			StartActivity(intent);
		}
		
        private void HelpButton_Click(object sender, object e)
        {
			var intentd = new Intent(this, typeof(SongViewActivity));
			intentd.PutExtra("songFile", "../help.html");
			StartActivity(intentd);
        }

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{
			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

			base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
		}
	}
}