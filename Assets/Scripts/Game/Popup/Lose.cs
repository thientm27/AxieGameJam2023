using TMPro;
using UnityEngine;

public class Lose : MonoBehaviour
{
    [SerializeField] private GameObject[] models;
    [SerializeField] private TextMeshProUGUI[] textMeshPros;
    [SerializeField] private TextMeshProUGUI desText;
    public void Init(bool isLose, int level, int gamegold, int crgold, int rcgold, int crdistance, int rcdistance)
    {
        desText.text = isLose == true ? "restart" : "next level";
        for (int i = 0; i < models.Length; i++)
        {
            models[i].SetActive(i <= level);
        }
        textMeshPros[0].text = gamegold.ToString();
        textMeshPros[1].text = crgold.ToString();
        textMeshPros[2].text = rcgold.ToString();
        textMeshPros[3].text = crdistance.ToString();
        textMeshPros[4].text = rcdistance.ToString();
    }
}
