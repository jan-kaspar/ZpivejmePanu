﻿using Android.App;using Android.Content;using Android.OS;using Android.Webkit;namespace ZpivejmePanu{	[Activity(Label = "SongViewActivity")]	public class SongViewActivity : Activity	{		protected override void OnCreate(Bundle savedInstanceState)		{			base.OnCreate(savedInstanceState);			SetContentView(Resource.Layout.SongViewLayout);						string songFile = Intent.Extras.GetString("songFile");			WebView textView = FindViewById<WebView>(Resource.Id.webView);			textView.LoadUrl("file:///android_asset/Songs/" + songFile);			textView.Settings.BuiltInZoomControls = true;		}	}}