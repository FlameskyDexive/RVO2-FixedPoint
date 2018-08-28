using System;
using System.Collections.Generic;

/// <summary>
/// 战斗随机 - 同步随机种子，统一随机数的产生
/// </summary>
public static class VRandom
{
    private static Random m_random = new Random();

    public static int seed;

    public static void SetRandomSeed(int seed)
    {
        m_random = new System.Random(seed);
        VRandom.seed = seed;
    }

    /// <summary>
    /// random一个数值，范围＝[start,end)
    /// </summary>
    public static int Random(int start, int end)
    {
        if (start > end)
        {
            return start;
        }
        int result = m_random.Next(start, end);
        return result;
    }

    /// <summary>
    /// 在[0,100)间random一个值是否成功
    /// </summary>
    public static bool IsRandomSucessIn100(int compareValue, ref int randomValue)
    {
        if (compareValue >= 100) return true;
        randomValue = Random(0, 100);
        return randomValue < compareValue;
    }

    /// <summary>
    /// 在[0,10000)间random一个值是否成功
    /// </summary>
    public static bool IsRandomSucessIn10000(int compareValue, ref int randomValue)
    {
        if (compareValue >= 10000) return true;
        randomValue = Random(0, 10000);
        return randomValue < compareValue;
    }
}
