using UnityEngine;
using System.Collections;

namespace PayUnity
{
    public static class OftenMethod
    {
        public static bool RandomPercent(float percent)
        {
            return Random.Range(0, 100) < percent;
        }

        public static bool InRandomRange(float randomPercentNum, float startNum, float endNum)
        {
            return (randomPercentNum >= startNum && randomPercentNum < endNum);
        }

        public static int ProbabilityDistribution(float baseValue, 
            float minimumMultiply, float maximumMultiply, int divide)
        {
            if(baseValue > 0f)
            {
                float baseValueMod = baseValue / divide,
                    valueMinimumMod = baseValueMod * minimumMultiply,
                    valueMaximumMod = baseValueMod * maximumMultiply,
                    result = 0f;

                for (int i = 0; i < divide; i++)
                    result += Random.Range(valueMinimumMod, valueMaximumMod);

                return Mathf.RoundToInt(result);
            }
            return 0;
        }
    }
}
