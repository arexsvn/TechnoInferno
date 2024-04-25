using UnityEngine;
using DG.Tweening;
using System;
using System.Threading.Tasks;

public class DOTweenUITransitions : IUITransitions
{
	public const float FADE_TIME = 0.2f;

    public async Task FadeIn(CanvasGroup canvasGroup, GameObject target = null)
    {
        await fadeIn(canvasGroup, target);    
    }

    public async Task FadeOut(CanvasGroup canvasGroup, GameObject target = null, bool deactivateOnComplete = true)
    {
        await fadeOut(canvasGroup, target);
    }

    public static async Task fadeIn(CanvasGroup canvasGroup, GameObject target = null)
    {
        if (target == null)
        {
            target = canvasGroup.gameObject;
        }
        
        await fade(canvasGroup, 1f, -1, target);
    }

    public static async Task fadeOut(CanvasGroup canvasGroup, GameObject target = null, bool deactivateOnComplete = true)
    {
        if (target == null)
        {
            target = canvasGroup.gameObject;
        }

        await fade(canvasGroup, 0f, -1, target, deactivateOnComplete);
    }

    public static async Task fade(CanvasGroup canvasGroup, float alpha, float fadeTime = -1f, GameObject target = null, bool deactivateOnComplete = false)
	{
        if (target == null)
        {
            target = canvasGroup.gameObject;
        }

        if (Mathf.Approximately(alpha, 0f))
        {
            canvasGroup.DOComplete(false);
            canvasGroup.alpha = alpha;

            if (alpha == 0f && deactivateOnComplete)
            {
                target.SetActive(false);
            }
            return;
        }

        await applyFade(canvasGroup.DOFade, canvasGroup.DOComplete, target, alpha, fadeTime, deactivateOnComplete);
    }

    private static async Task applyFade(Func<float, float, Tween> fadeAction, 
                                  Func<bool, int> completeAction, 
                                  GameObject target,
                                  float alpha, 
                                  float time = -1f, 
                                  bool deactivateOnComplete = false)
    {
        if (time == -1f)
        {
            time = FADE_TIME;
        }

        completeAction(false);

        target.SetActive(true);

        await fadeAction(alpha, time).AsyncWaitForCompletion();

        if (alpha == 0f && deactivateOnComplete)
        {
            target.SetActive(false);
        }
    }
}
