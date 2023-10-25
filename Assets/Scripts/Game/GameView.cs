using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private Image comboFill;
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private GameObject[] heartsIcon;
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
}
