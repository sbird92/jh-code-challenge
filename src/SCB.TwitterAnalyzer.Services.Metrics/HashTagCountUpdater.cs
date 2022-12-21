using Microsoft.Extensions.Logging;
using SCB.TwitterAnalyzer.Domain.Models;
using SCB.TwitterAnalyzer.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCB.TwitterAnalyzer.Services.Metrics
{
    public class HashTagCountUpdater : MetricUpdater
    {
        private const string KEY = "hashtag";
        public HashTagCountUpdater(IMetricStore metricStore, ILogger<MetricUpdater> logger) : base(metricStore, logger)
        {
        }

        protected override bool CanIncrementCount(Tweet? tweet)
        {
            return tweet != null && 
                tweet.Entities.HashTags.Any();
        }

        protected override IEnumerable<string> GetKeys(Tweet tweet)
        {
            foreach (var tag in tweet.Entities.HashTags)
                yield return $"{KEY}:{tag.Tag}";
        }
    }
}
