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
    public class LanguageCountUpdater : MetricUpdater
    {
        private const string KEY = "language";
        public LanguageCountUpdater(IMetricStore metricStore, ILogger<MetricUpdater> logger) : base(metricStore,logger)
        {
        }

        protected override bool CanIncrementCount(Tweet? tweet)
        {
            return tweet != null && 
                !string.IsNullOrEmpty(tweet.Language);
        }

        protected override IEnumerable<string> GetKeys(Tweet tweet)
        {
            yield return $"{KEY}:{tweet.Language}";
        }
    }
}
