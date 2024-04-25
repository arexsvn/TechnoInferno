using System;
using System.Threading.Tasks;
using UnityEngine;

public interface IUITransitions
{
    public Task FadeIn(CanvasGroup canvasGroup, GameObject target = null);

    public Task FadeOut(CanvasGroup canvasGroup, GameObject target = null, bool deactivateOnComplete = true);
}
