﻿using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System;
using System.Threading.Tasks;
using System.Windows;
using Windows.Phone.System.UserProfile;

namespace DailyQuote
{
	public partial class MainPage : PhoneApplicationPage
	{
		public MainPage()
		{
			InitializeComponent();
			headerDate.Text = DateTime.Now.ToShortDateString();
			Loaded += MainPage_Loaded;
		}

		async void MainPage_Loaded(object sender, RoutedEventArgs e)
		{
			if (!LockScreenManager.IsProvidedByCurrentApplication)
			{
				await LockScreenManager.RequestAccessAsync();
			}

			new UpdateBackgroundAgent().StartPeriodicAgent();

			try
			{
				await ShowQuote();
			}
			catch (Exception)
			{
				MessageBox.Show("Could not download quote. Pin to start and wait for quotes.");
			}
			finally
			{
				loadingProgressBar.Visibility = System.Windows.Visibility.Collapsed;
			}
		}

		async Task ShowQuote()
		{
			var quote = DailyQuoteLogic.Caching.GetCachedDailyQuote();

			if (quote == null)
			{
				quote = await DailyQuoteLogic.Get.Quote();
				DailyQuoteLogic.Caching.Cache(quote);
				DailyQuoteLogic.Caching.RememberTodayAsQuoteDay();
				var success = await DailyQuoteLogic.PhoneIntegration.UpdateTile(quote);
			}

			quoteTextBlock.Text = quote.ToString();
			quoteTextBlock.Background = DailyQuoteLogic.PhoneIntegration.RandomColor();
			quoteTextBlock.Visibility = System.Windows.Visibility.Visible;
		}

		void BrowseToBrainyQuote(object sender, EventArgs e)
		{
			new WebBrowserTask { Uri = new Uri("http://www.brainyquote.com/quotes_of_the_day.html", UriKind.Absolute) }.Show();
		}

		void Share(object sender, EventArgs e)
		{
			new ShareStatusTask { Status = quoteTextBlock.Text }.Show();
		}

		void Send(object sender, EventArgs e)
		{
			new SmsComposeTask { Body = quoteTextBlock.Text }.Show();
		}

		void GoToInfo(object sender, EventArgs e)
		{
			App.RootFrame.Navigate(new Uri("/Info.xaml", UriKind.Relative));
		}
	}
}