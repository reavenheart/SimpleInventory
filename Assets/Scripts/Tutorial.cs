using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Canvas tutorialCanvas = null;
    [SerializeField] private RectTransform tutorialContainer = null;
    [SerializeField] [Range(1, 10)] private float tutorialDuration = 5;

    private void Awake()
    {
        tutorialContainer.localScale = Vector3.zero;
    }

    void Start()
    {
        StartCoroutine(ShowTutorial());
    }

    private IEnumerator ShowTutorial()
    {
        tutorialContainer.DOScale(Vector3.one, 1.0f).SetEase(Ease.InOutCubic);
        yield return new WaitForSeconds(tutorialDuration);
        tutorialContainer.DOScale(Vector3.zero, 1.0f).SetEase(Ease.InOutCubic).OnComplete(() =>
        {
            tutorialCanvas.gameObject.SetActive(false);
        });
    }
}
