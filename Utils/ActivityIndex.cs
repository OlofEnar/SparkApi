namespace SparkApi.Utils
{
    // Class for calculating user activity.
    // Adding a few different ones here to see which will be
    // more suitable. 
    public static class ActivityIndex
    {
        public static decimal CalcIndexWeighted(int eventTotal, int dateCount)
        {
            decimal dateTreshold = 5;
            decimal scalingFactor = 5;
            decimal maxScore = 100;

            if (dateCount == 0) return 0;

            decimal weight = Math.Min(1, (decimal)Math.Log(dateCount + 1) / scalingFactor);

            if (dateCount < dateTreshold)
            {
                weight *= (decimal)dateCount / dateTreshold;
            }

            decimal baseIndex = eventTotal / dateCount;
            decimal weightedIndex = baseIndex * weight;
            decimal scaledIndex = Math.Min(weightedIndex, maxScore);
            decimal result = Math.Round(scaledIndex, 1);

            return result;
            
        }

        public static decimal CalcIndexExpDecay(int eventTotal, int dateCount)
        {
            decimal decayConstant = 0.5m;

            if (dateCount == 0) return 0;

            decimal baseIndex = eventTotal / dateCount;
            decimal decayFactor = 1 - (decimal)Math.Exp((double)(-decayConstant * dateCount));
            decimal calculation = (baseIndex * decayFactor) / 100;

            decimal result = Math.Round(calculation, 1);

            return result;
        }
    }
}
