using ZpivejmePanu;

using NUnit.Framework;

namespace ZpivejmePanuTests
{
	[TestFixture]
	public class SongDataTests
	{
		SongData sut;

		[SetUp]
		public void Setup()
		{
			sut = new SongData("number", "title", "author", "source");
		}

		[Test]
		public void TestGetters()
		{
			Assert.AreEqual("number", sut.GetNumber());
			Assert.AreEqual("title", sut.GetTitle());
			Assert.AreEqual("author", sut.GetAuthor());
			Assert.AreEqual("source", sut.GetSource());
		}

		[Test]
		public void ToStringContainsNumberAndTitle()
		{
			var s = sut.ToString();

			StringAssert.Contains(sut.GetNumber(), s);
			StringAssert.Contains(sut.GetTitle(), s);
		}

		[Test]
		[TestCase("1", "1", true)]
		[TestCase("123", "123", true)]
		[TestCase("123", "12", false)]
		[TestCase("123", "1", false)]
		public void TestMatchNumber(string number, string key, bool expectation)
		{
			SongData sut = new SongData(number, "t", "a", "s");
			Assert.That(sut.Match(key), Is.EqualTo(expectation));
		}

		[Test]
		[TestCase("title", "title", true)]
		[TestCase("TiTlE", "title", true)]
		[TestCase("something", "som", true)]
		[TestCase("something", "thi", true)]
		[TestCase("SomeThing", "eth", true)]
		[TestCase("ěščřžýáíé", "escrzyaie", true)]
		[TestCase("ĚŠČŘŽÝÁÍÉ", "ESCRZYAIE", true)]
		[TestCase("whatever", "", true)]
		[TestCase("bla", "neco", false)]
		public void TestMatchTitle(string value, string key, bool expectation)
		{
			SongData sut1 = new SongData("n", value, "a", "s");
			Assert.That(sut1.Match(key), Is.EqualTo(expectation));
		}
	}
}