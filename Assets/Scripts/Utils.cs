using UnityEngine;

public static class Utils
{
    // 경우의 수를 인자로 받아 0이 나올 확률을 구한다.
    public static bool RandomByCase(int numberOfCase)
    {
        bool result = default(bool);

        if (Random.Range(0, numberOfCase) == 0) result = true;
        else result = false;

        return result;
    }
    public static bool RandomByCase(int numberOfCase, int targetNumber)
    {
        bool result = default(bool);

        if (Random.Range(0, numberOfCase) == targetNumber) result = true;
        else result = false;

        return result;
    }
}
