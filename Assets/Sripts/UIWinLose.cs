using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIWinLose : MonoBehaviour
{
    public Image background;
    public Button next, again;
    public Image starsBackground;
    public Image[] stars;
    public Image levelComplete;
    public Image gameOver;
    public int starsQuantity;

    public ParticleSystem[] confetti;

    static public UIWinLose instance;
    private void Start()
    {
        if (UIWinLose.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        UIWinLose.instance = this;
    }
    private void PlayConfetti()
    {
        int lenght = confetti.Length;
        for (int i = 0; i < lenght; i++)
        {
            confetti[i].Play();
        }
    }

    public void ButtonFunction()
    {
        next.DOKill();
        again.DOKill();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Win()
    {
        FingerDirection.instance.directionCircleZone.gameObject.SetActive(false);
        FingerDirection.instance.useFingerDirection = false;

        var seq = DOTween.Sequence();
        seq.Append(background.gameObject.transform.DOScale(1f, 0.25f));
        seq.Append(levelComplete.gameObject.transform.DOScale(1.5f, 0.25f));
        seq.Append(levelComplete.gameObject.transform.DOScale(1f, 0.25f).SetEase(Ease.InSine));

        if (starsQuantity > stars.Length)
            starsQuantity = stars.Length;
        else if (starsQuantity < 0)
            starsQuantity = 0;

        for (int i = 0; i < starsQuantity; i++)
        {
            seq.Append(stars[i].gameObject.transform.DOScale(1.5f, 0.25f));
            seq.Append(stars[i].gameObject.transform.DOScale(1f, 0.25f).SetEase(Ease.InSine));
        }

        if (starsQuantity > 0)
        {
            seq.Append(stars[0].gameObject.transform.DOShakeRotation(1f, new Vector3(0, 0, 40f)).SetEase(Ease.InCirc).OnComplete(PlayConfetti));
            for (int i = 1; i < starsQuantity; i++)
            {
                seq.Join(stars[i].gameObject.transform.DOShakeRotation(1f, new Vector3(0, 0, 40f)).SetEase(Ease.InCirc));
            }
        }

        seq.Append(next.gameObject.transform.DOScale(1.5f, 0.25f));
        seq.Append(next.gameObject.transform.DOScale(1f, 0.25f).SetEase(Ease.InSine));
    }

    public void Lose()
    {
        FingerDirection.instance.directionCircleZone.gameObject.SetActive(false);
        FingerDirection.instance.useFingerDirection = false;

        var seq = DOTween.Sequence();
        starsBackground.color = new Color(1, 0, 0, 0.5f);
        background.color = new Color(1, 0, 0, 0.5f);
        seq.Append(background.gameObject.transform.DOScale(1f, 0.25f));
        seq.Append(gameOver.gameObject.transform.DOScale(1.5f, 0.25f));
        seq.Append(gameOver.gameObject.transform.DOScale(1f, 0.25f).SetEase(Ease.InSine));

        seq.Append(again.gameObject.transform.DOScale(1.5f, 0.25f));
        seq.Append(again.gameObject.transform.DOScale(1f, 0.25f).SetEase(Ease.InSine));
    }
}
