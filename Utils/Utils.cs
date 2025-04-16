using SparkApi.Models.DTOs;

namespace SparkApi.Utils
{
    public static class Utils
    {
        public static void CalcEventDistribution(AggregatedResponse a)
        {
            int runningTotal = 0;

            if (a == null || a.AggregatedEvents == null || a.AggregatedEvents.Count == 0)
                return;

            for (int i = 0; i < a.AggregatedEvents.Count; i++)
            {
                if (i < a.AggregatedEvents.Count - 1)
                {
                    var eventTotal = a.AggregatedEvents[i].EventTotal;
                    double fraction = eventTotal / (double)a.TotalEvents;
                    int percentage = (int)Math.Round(fraction * 100, 0);

                    a.AggregatedEvents[i].EventDistribution = percentage;
                    runningTotal += percentage;
                }
                else
                {
                    // adjust final event to even out distribution
                    a.AggregatedEvents[i].EventDistribution = 100 - runningTotal;
                }
            }
        }
    }
}
