using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
#region을 사용하여 확장메소드 구분 정리합시다.
*/

public static class UnityExtensions
{
    #region Animator Extentions
    public static void SetTriggerDistinct(this Animator animator, string name)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(name) == false)
        {
            animator.SetTrigger(name);
        }
    }

    public static bool IsStateName(this Animator animator, string name)
    {
        return animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }

    public static bool HasParameter(this Animator animator, string paramName, AnimatorControllerParameterType type)
    {
        if (string.IsNullOrEmpty(paramName) == false)
        {
            AnimatorControllerParameter[] parameters = animator.parameters;
            foreach (AnimatorControllerParameter currParam in parameters)
            {
                if (currParam.type == type && currParam.name == paramName)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool HasParameter(this Animator animator, int paramHash, AnimatorControllerParameterType type)
    {
        AnimatorControllerParameter[] parameters = animator.parameters;
        foreach (AnimatorControllerParameter currParam in parameters)
        {
            if (currParam.type == type && currParam.nameHash == paramHash)
            {
                return true;
            }
        }
        return false;
    }

    public static void ResetAllTriggers(this Animator animator)
    {
        foreach (var param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(param.name);
            }
        }
    }
    #endregion
    #region RectTransform Extentions

    public static void SetSizeDelta(this RectTransform self, Vector2 newSize)
    {
        Vector2 oldSize = self.rect.size;
        Vector2 deltaSize = newSize - oldSize;
        self.offsetMin = self.offsetMin - new Vector2(deltaSize.x * self.pivot.x, deltaSize.y * self.pivot.y);
        self.offsetMax = self.offsetMax + new Vector2(deltaSize.x * (1f - self.pivot.x), deltaSize.y * (1f - self.pivot.y));
    }

    public static void SetWidth(this RectTransform self, float newSize)
    {
        SetSizeDelta(self, new Vector2(newSize, self.rect.size.y));
    }
    public static void SetHeight(this RectTransform self, float newSize)
    {
        SetSizeDelta(self, new Vector2(self.rect.size.x, newSize));
    }

    public static float GetWidth(this RectTransform self)
    {
        return self.rect.width;
    }

    public static float GetHeight(this RectTransform self)
    {
        return self.rect.height;
    }

    public static void FullArea(this RectTransform self)
    {
        self.pivot = new Vector2(0.5f, 0.5f);

        self.anchorMin = new Vector2(0f, 0f);
        self.anchorMax = new Vector2(1f, 1f);

        self.offsetMin = new Vector2(0f, 0f);
        self.offsetMax = new Vector2(0f, 0f);
    }
    #endregion
    #region IEnumerable Extentions
    public static string ToEachString<T>(this IEnumerable<T> collection, string separator = ", ", string prefix = null, string suffix = null)
    {
        if (collection == null) return string.Empty;

        return string.Join(separator, collection.Select(s =>
        {
            return string.Format("{0}{1}{2}", prefix, s != null ? s.ToString() : "null", suffix);
        }).ToArray());
    }
    #endregion
}
