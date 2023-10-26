using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameModel model;
    private int combo = 0;
    private int maxHeart = 3;
    private int speed = 0;
    private void Awake()
    {
        player.Initialized(model.AxieCharacters[0]);
    }
}
