﻿using System;
using System.Windows.Controls;
using Microsoft.Phone.Tasks;

namespace SilverlightRun.PhoneSpecific.UI
{
	/// <summary>
	/// Opens the browser with the url on click.
	/// </summary>
	public class ColdWebLink : HyperlinkButton
	{
		string _Url;
		public string Url
		{
			set
			{
				_Url = value;
				Content = value;
			}
			get
			{
				return _Url;
			}
		}

		public ColdWebLink()
			: base()
		{
			Click += (s, e) => BrowseTo(_Url);
		}

		private void BrowseTo(string url)
		{
			if (!url.StartsWith("http://")) url = "http://" + url;
			var browser = new WebBrowserTask();
			browser.Uri = new Uri(url);
			browser.Show();
		}
	}
}
