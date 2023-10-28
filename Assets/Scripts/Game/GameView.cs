using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private Image comboFill;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private GameObject[] heartsIcon;
    [SerializeField] private CanvasGroup ingameCanvasGroup;
    [SerializeField] private CanvasGroup press;
    public void SetSpeed(int speed)
    {
        speedText.text = speed.ToString();
    }
    public void SetCombo(int combo)
    {
        comboFill.fillAmount = combo / 10.0f;
    }
    public void SetHeart(int maxHeart, int heart)
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(i < maxHeart);
        }
        for (int i = 0; i < heartsIcon.Length; i++)
        {
            heartsIcon[i].SetActive(i < heart);
        }
    }
    public void ViewUIIngame()
    {
        ingameCanvasGroup.DOFade(1, 0.5f);
        press.DOFade(0, 0.5f);
    }
}
