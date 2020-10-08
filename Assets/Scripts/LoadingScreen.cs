using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    private CanvasGroup m_canvasGroup = default;

    public bool IsFading { get; private set; }

    [SerializeField]
    private float m_fadeSpeed = 0.5f;

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        Fade(1f);
    }

    private void OnDisable()
    {
        Fade(0f);
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(0));
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(1));
    }

    private IEnumerator Fade(float target)
    {
        IsFading = true;
        float newFadeSpeed = 0;

        if (target < m_canvasGroup.alpha)
            newFadeSpeed = -Mathf.Abs(m_fadeSpeed);
        else
            newFadeSpeed = Mathf.Abs(m_fadeSpeed);

        while (Mathf.Abs(m_canvasGroup.alpha - target) > 0.01f)
        {
            m_canvasGroup.alpha += Time.deltaTime * newFadeSpeed;
            yield return null;
        }

        if (newFadeSpeed < 0)
            m_canvasGroup.alpha = 0;
        else
            m_canvasGroup.alpha = 1;

        IsFading = false;
    }
}
