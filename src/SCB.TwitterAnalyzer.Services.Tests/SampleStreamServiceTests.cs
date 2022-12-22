using FluentAssertions;
using SCB.TwitterAnalyzer.Domain.Models;
using SCB.TwitterAnalyzer.Domain.Services;
using SCB.TwitterAnalyzer.Services.TwitterStream;
using System.Globalization;

namespace SCB.TwitterAnalyzer.Services.Tests
{
    public class SampleStreamServiceTests : IClassFixture<SampleStreamClientTestFixture>
    {
        private readonly SampleStreamClientTestFixture _sampleStreamFixture;
        private readonly Mock<ILogger<SampleStreamService>> _loggerMock = new();
        private readonly Mock<ITweetQueue> _tweetQueueMock = new();

        private readonly SampleStreamService _sut;
        public SampleStreamServiceTests(SampleStreamClientTestFixture sampleStreamFixture) 
        { 
            _sampleStreamFixture = sampleStreamFixture;
            _sut = new SampleStreamService(_sampleStreamFixture, _tweetQueueMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async void StartAsync_all_deserializable_tweets_are_sent_to_the_queue()
        {
            _sampleStreamFixture.LoadTestData("tweet-samples.txt");
            await _sut.StartAsync();
            //
            _tweetQueueMock.Verify(m => m.EnqueueAsync(It.IsAny<Tweet>()), Times.Exactly(14));
        }

        [Fact]
        public async void StartAsync_single_tweet_is_deserialized_with_expected_values()
        {
            _sampleStreamFixture.LoadTestData("tweet-samples-single-tweet.txt");
            await _sut.StartAsync();
            _tweetQueueMock.Verify(m => m.EnqueueAsync(It.Is<Tweet>(t => VerifySingleTweet(t))), Times.Exactly(1));
        }

        private static bool VerifySingleTweet(Tweet arg)
        {
            arg.CreatedAt.Should().Be(
                DateTime.ParseExact("2022-12-18T18:47:14.000Z", "yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal));
            arg.EditHistoryTweetIds.Should().BeEquivalentTo(new[] { "1604548862953148418" });
            arg.Entities.Should().NotBeNull();
            arg.Entities?.HashTags.Should().BeEquivalentTo(new[]
            {
                new HashTag()
                {
                    Start = 3,
                    End = 8,
                    Tag = "tweet"
                }
            });
            arg.Id.Should().Be("1604548862953148418");
            arg.Language.Should().Be("en");
            arg.Source.Should().Be("Twitter for iPad");
            arg.Text.Should().Be("AA #tweet A tweet text");
            return true;
        }
    }
}