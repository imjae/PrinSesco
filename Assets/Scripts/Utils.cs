using UnityEngine;

public static class Utils
{
    // 인자가 클수록 확률이 낮아진다.
    public static bool RandomPer(int per)
    {
        bool result = default(bool);

        if (Random.Range(0, per) == 0) result = true;
        else result = false;

        return result;
    }
}
