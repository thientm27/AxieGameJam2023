using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ActionsController : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button quitButton;

    void Awake()
    {
        playButton.onClick.AddListener(() => SceneManager.LoadScene(Constants.GamePlay));
        shopButton.onClick.AddListener(() => SceneManager.LoadScene(Constants.ShopScene));
        quitButton.onClick.AddListener(() => Application.Quit());
    }
}
