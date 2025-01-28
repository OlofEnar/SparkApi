namespace SparkApi.Utils
{
    // Class for calculating user activity.
    // Adding a few different ones here to see which will be
    // more suitable. 
    public static class ActivityIndex
    {
        public static decimal CalcIndexWeighted(int eventTotal, int dateCount, int movingAverageWindow = 7)
        {
            decimal dateThreshold = 5; // Minimum dates for weight adjustment
            decimal scalingFactor = 5; // Logarithmic scaling factor
            decimal maxScore = 100;
            decimal minWeightFactor = 0.5m; // Minimum weight for users below dateThreshold
            decimal normalizationFactor = 20; // Normalization for event scaling

            if (dateCount == 0) return 0;

            // Moving average factor for smoothing activity
            decimal movingAverageFactor = Math.Min(1, (decimal)dateCount / movingAverageWindow);

            // Weight calculation based on logarithmic growth
            decimal weight = Math.Min(1, (decimal)Math.Log(dateCount + 1) / scalingFactor);

            // Adjust weight further for low date counts
            if (dateCount < dateThreshold)
            {
                weight *= Math.Max(minWeightFactor, (decimal)dateCount / dateThreshold);
            }

            // Combine weight with moving average smoothing
            weight *= movingAverageFactor;

            // Base index adjusted by normalization to prevent excessive scores
            decimal baseIndex = ((decimal)eventTotal / dateCount) / normalizationFactor;

            // Weighted and scaled index
            decimal weightedIndex = baseIndex * weight;
            decimal scaledIndex = Math.Min(weightedIndex, maxScore);

            // Rounded result
            return Math.Round(scaledIndex, 1);
        }

        public static decimal CalcHighValueActivityIndex(Dictionary<string, int> eventCounts, HashSet<string> highValueEventNames, int dateCount)
        {
            decimal highValueWeight = 2.0m; // High-value events are worth more
            decimal normalizationFactor = 10; // Normalize high-value events
            decimal maxScore = 100;

            if (dateCount == 0) return 0;

            int highValueEventTotal = eventCounts
                .Where(kvp => highValueEventNames.Contains(kvp.Key))
                .Sum(kvp => kvp.Value);

            int generalEventTotal = eventCounts.Values.Sum();

            // Calculate high-value index relative to general activity
            decimal baseIndex = (decimal)highValueEventTotal / Math.Max(1, generalEventTotal);
            baseIndex *= highValueWeight;

            // Normalize and scale
            decimal normalizedIndex = baseIndex / normalizationFactor;
            decimal scaledIndex = Math.Min(normalizedIndex, maxScore);

            return Math.Round(scaledIndex, 1);
        }
    }
}
