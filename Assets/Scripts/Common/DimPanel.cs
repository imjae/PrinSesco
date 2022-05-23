///------------------------------------------------------------------------------------------------
///   Author        :   kim min seok   
///
///   Description   :   SlotMachine에 Hit 되지 않는 객체들을 어둡게 처리할때 사용.
///                             
///   Notes         :   
///------------------------------------------------------------------------------------------------

using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

public sealed class DimPanel : MonoBehaviour
{
    [SerializeField] private float showDuration = 0.15f;
    [SerializeField] private Ease showEase = Ease.Linear;
    [SerializeField] private float hideDuration = 0.15f;
    [SerializeField] private Ease hideEase = Ease.Linear;

#pragma warning disable 0649
    [SerializeField] private SpriteRenderer[] targetList;
    private Dictionary<SpriteRenderer, float> defaultAlphaDic;

    private void Awake()
    {
        SetupDimPanle();
    }

    private void SetupDimPanle()
    {
        defaultAlphaDic = new Dictionary<SpriteRenderer, float>();

        if (targetList != null)
        {
            for (int i = 0; i < targetList.Length; i++)
            {
                var target = targetList[i];
                var defaultAlpha = target.color.a;
                //target.color = new Color(target.color.r, target.color.g, target.color.b, 0f);
                //target.gameObject.SetActive(false);

                defaultAlphaDic.Add(target, defaultAlpha);
            }
        }
    }

    public int GetCount()
    {
        return targetList.Length;
    }

    public void Show()
    {
        Show(showDuration);
    }

    public void Show(float duration)
    {
        for (int i = 0; i < targetList.Length; i++)
        {
            var target = targetList[i];
            var defaultAlpha = defaultAlphaDic[target];

            target.gameObject.SetActive(true);
            target.DOKill(false);
            target.DOFade(defaultAlpha, duration).SetEase(showEase);
        }
    }

    public void Hide()
    {
        Hide(hideDuration);
    }

    public void Hide(float duration)
    {
        for (int i = 0; i < targetList.Length; i++)
        {
            var target = targetList[i];
            target.DOKill(false);
            target.DOFade(0f, duration)
                    .SetEase(hideEase)
            .OnComplete(() =>
            {
                if (target.gameObject != gameObject)
                {
                    target.gameObject.SetActive(false);
                }
            });
        }
    }

    private void Start()
    {
        GetComponent<DimPanel>().Hide();
    }
}