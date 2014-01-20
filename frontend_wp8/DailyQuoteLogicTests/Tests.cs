﻿using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DailyQuoteLogicTests
{
	[TestClass]
	public class Tests
	{
		[TestMethod]
		public void GetQuote_ReturnsQuoteHtml()
		{
			var quoteFuture = DailyQuoteLogic.DailyQuoteLogic.GetQuoteFromWebsite();
			quoteFuture.Wait();
			var quote = quoteFuture.Result;
			Assert.IsTrue(string.IsNullOrEmpty(quote) == false);
		}


		[TestMethod]
		public void Parse_Html_TextAndAuhtor()
		{
			var quote = new DailyQuoteLogic.Quote(
@"class=""bqQuoteLink""><a title=""view quote"" href=""/quotes/quotes/m/martinluth101309.html"">We must learn to live together as brothers or perish together as fools.</a></span><br>
<span class=""bodybold""><a title=""view author"" href=""/quotes/authors/m/martin_luther_king_jr.html"">Martin Luther King, Jr.</a></span>
</div>");

			Assert.AreEqual("We must learn to live together as brothers or perish together as fools.", quote.Text);
			Assert.AreEqual("Martin Luther King, Jr.", quote.Author);
		}
	}
}
