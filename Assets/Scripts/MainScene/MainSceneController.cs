using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using Services;
using ShopScene;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class MainSceneController : MonoBehaviour
{
    
    [SerializeField] private GameObject loading;
    [SerializeField] private TextMeshProUGUI loadingTxt;
    [SerializeField] private ChangeSceneController changeSceneController;
    [SerializeField] private List<GameObject> buttons;
    private AudioService _audioService;
    private bool _isLoad = false;
    private bool _isFinishLoad = false;
    private int _currentSelect = 0;
    private GameObject _current;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag(Constants.ServicesTag) != null)
        {
            var services = GameObject.FindGameObjectWithTag(Constants.ServicesTag).GetComponent<GameServices>();
            _audioService = services.GetService<AudioService>();
        }
        else
        {
            SceneManager.LoadScene(Constants.EntryScene);
            return;
        }
    }

    private void Start()
    {
        Loading();
    }

    private void Update()
    {
        if (_isLoad&&Input.anyKey)
        {
            _isLoad = false;
            _isFinishLoad = true;
        }

        if (!_isFinishLoad)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            HandleSelector(true);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            HandleSelector(false);
        }

    }

    private void HandleSelector(bool isRight)
    {
        _audioService.Button();
        buttons[_currentSelect].transform.DOScale(Vector3.one, 0.25f);
        _currentSelect = isRight ? _currentSelect == 2 ? 0 : _currentSelect + 1 : _currentSelect == 0 ? 3 : _currentSelect - 1;
        buttons[_currentSelect].transform.DOScale(Vector3.one * 1.2f, 0.25f);
        
    }
    private async void Loading()
    {
        changeSceneController.Open();
        await Task.Delay(3000);
        loading.SetActive(false);
        loadingTxt.text = "Press any key to continue...";
        _isLoad = true;
    }
}
