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
    }
}
