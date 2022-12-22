using SCB.TwitterAnalyzer.Domain.Models;
using SCB.TwitterAnalyzer.Domain.Services;
using System.Timers;
using System.Xml.Schema;

namespace SCB.TwitterAnalyzer.Infrastructure.App
{
    internal class ConsoleMetricWriter
    {
        private readonly IMetricService _metricService;
        private readonly System.Timers.Timer _timer;
        public ConsoleMetricWriter(IMetricService metricService)
        {
            _metricService = metricService;
            _timer = new System.Timers.Timer(10000);
            _timer.Elapsed += OnTimedEvent;
        }

        public void Start()
        {
            _timer.Enabled = true;
        }

        public void Stop()
        {
            _timer.Stop();
            _timer.Dispose();
        }

        private void OnTimedEvent(object? source, ElapsedEventArgs e)
        {
            WriteMetrics(e.SignalTime);
        }

        private void WriteMetrics(DateTime time)
        {
            var metrics = _metricService.GetMetrics();
            Console.Clear();
            Console.WriteLine("Press Esc to end the program");
            Console.WriteLine($"Metrics Refreshed at : {time:HH:mm:ss}");
            Console.WriteLine();
            Console.WriteLine($"Total Tweets Recieved: {metrics.TotalTweets}");
            Console.WriteLine();
            Console.WriteLine($"Top 10 HashTags (Min 2)");
            Console.WriteLine("-----------------------");
            WriteTopList(metrics.HashTags.ToArray(), 10, "hashtag:");
            Console.WriteLine();
            Console.WriteLine($"Distinct HashTags: {metrics.DistinctTags}");
            Console.WriteLine();
            Console.WriteLine($"Top 10 Languages (Min 2)");
            Console.WriteLine("-----------------------");
            WriteTopList(metrics.Languages.ToArray(), 10, "language:");
            Console.WriteLine();
            Console.WriteLine($"Distinct Languages: {metrics.DistinctLanguages}");

        }

        private void WriteTopList(Metric[] metrics, int count, string replace)
        {
            for (int i = 0; i < count; i++)
            {
                if (i + 1 <= metrics.Count())
                    Console.WriteLine($"{i + 1}) {metrics[i].Key.Replace(replace,"")} : {metrics[i].Value}");
                else Console.WriteLine($"{i + 1}) ---");
            }
        }
    }
}
