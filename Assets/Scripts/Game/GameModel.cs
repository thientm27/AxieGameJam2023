using Spine.Unity;
using System.Collections.Generic;
using UnityEngine;

public class GameModel : MonoBehaviour
{
    [SerializeField] private List<AxieCharacter> axieCharacters;
    public List<AxieCharacter> AxieCharacters { get => axieCharacters; set {  axieCharacters = value; } }
}
[System.Serializable]
public class AxieCharacter
{
    [SerializeField] private GameObject skeletonAnimation;
    [SerializeField] private int maxHP;
    [SerializeField] private int chanceEva;
    [SerializeField] private int bonusGold;
    [SerializeField] private bool isDown;
    [SerializeField] private bool decreaseSpeedWhenMiss;
    public GameObject SkeletonAnimation { get => skeletonAnimation; set => skeletonAnimation = value; }
    public int MaxHP { get => maxHP; set => maxHP = value; }
    public int ChanceEva { get => chanceEva; set => chanceEva = value; }
    public int BonusGold { get => bonusGold; set => bonusGold = value; }
    public bool IsDownOverTime { get => isDown; set => isDown = value; }
    public bool DescreaseSpeedWhenMiss { get => decreaseSpeedWhenMiss; set => decreaseSpeedWhenMiss = value; }
}