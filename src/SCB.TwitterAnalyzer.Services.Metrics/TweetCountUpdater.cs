using Microsoft.Extensions.Logging;
using SCB.TwitterAnalyzer.Domain.Models;
using SCB.TwitterAnalyzer.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SCB.TwitterAnalyzer.Services.Metrics;

public class TweetCountUpdater : MetricUpdater
{
    private const string KEY = "total-tweets";
    public TweetCountUpdater(IMetricStore metricStore, ILogger<MetricUpdater> logger) : base(metricStore, logger)
    {
    }

    protected override IEnumerable<string> GetKeys(Tweet _)
    {
        yield return KEY;
    }

    protected override bool CanIncrementCount(Tweet? tweet)
    {
        return tweet != null;
    }
}
