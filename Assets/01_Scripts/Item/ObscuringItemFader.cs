using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObscuringItemFader : MonoBehaviour
{
    private SpriteRenderer sp;

    private void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    public void FadeOut()
    {
        StartCoroutine(FadeOutRoutine());
    }


    public void FadeIn()
    {
        StartCoroutine(FadeInRoutine());
    }
    private IEnumerator FadeOutRoutine()
    {
        float currentAlpha = sp.color.a;
        float distance = currentAlpha - Settings.targetAlpha;
        while (currentAlpha - Settings.targetAlpha > 0.01f)
        {
            currentAlpha = currentAlpha - distance / Settings.fadeOutSeconds * Time.deltaTime;
            sp.color = new Color(1, 1, 1, currentAlpha);
            yield return null;
        }
        sp.color = new Color(1, 1, 1, Settings.targetAlpha);
    }

    private IEnumerator FadeInRoutine()
    {
        float currentAlpha = sp.color.a;
        float distance = 1f - currentAlpha;
        while (1f - currentAlpha > 0.01f)
        {
            currentAlpha = currentAlpha + distance / Settings.fadeInSeconds * Time.deltaTime;
            sp.color = new Color(1, 1, 1, currentAlpha);
            yield return null;
        }
        sp.color = new Color(1, 1, 1, 1);
    }
}
