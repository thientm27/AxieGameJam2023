using System;
using Services;
using Spine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace ShopScene
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private ShopView view;
        [SerializeField] private ShopModel model;
        [SerializeField] private SkeletonGraphic skeletonAnimation;
        [SerializeField] private ChangeSceneController changeSceneController;


        private DisplayShop _currentChoose;
        private int _index;
        private PlayerService _playerService;
        private AudioService _audioService;

        private const string IdleAnim = "action/idle/normal";
        private const string BuyAnim = "activity/evolve";

        private void Awake()
        {
            // SERVICES
            if (GameObject.FindGameObjectWithTag(Constants.ServicesTag) != null)
            {
                var services = GameObject.FindGameObjectWithTag(Constants.ServicesTag).GetComponent<GameServices>();
                _playerService = services.GetService<PlayerService>();
                _audioService = services.GetService<AudioService>();
            }
            else
            {
                SceneManager.LoadScene(Constants.EntryScene);
                return;
            }


            // VALUES
            _index = 0;
            _currentChoose = DisplayShop.Armor;

            // INIT SHOP 1 VALUE
            int[] tmp = new int[4];
            tmp[0] = model.armorPrices[_playerService.ArmoryLevel[0]];
            tmp[1] = model.weaponPrices[_playerService.ArmoryLevel[1]];
            tmp[2] = model.bootsPrices[_playerService.ArmoryLevel[2]];
            tmp[3] = model.rocketPrices[_playerService.ArmoryLevel[3]];
            view.contentShops[0].Initialized(_playerService.ArmoryLevel.ToArray(),
                tmp);

            // INIT SHOP 2 VALUE
            tmp = new int[4];
            tmp[0] = model.accessory0[_playerService.AccessoryLevel[0]];
            tmp[1] = model.accessory1[_playerService.AccessoryLevel[1]];
            tmp[2] = model.accessory2[_playerService.AccessoryLevel[2]];
            tmp[3] = model.accessory3[_playerService.AccessoryLevel[3]];
            view.contentShops[1].Initialized(_playerService.AccessoryLevel.ToArray(),
                tmp);

            // Display first shop
            view.ChangeTab(_currentChoose);
            view.SetUserWallet(_playerService.UserCoin);

            // Init spine
            skeletonAnimation.AnimationState.SetAnimation(0, IdleAnim,
                true);
            skeletonAnimation.AnimationState.Complete += trackEntry =>
            {
                skeletonAnimation.AnimationState.SetAnimation(0, IdleAnim,
                    true);
            };
        }

        private void Start()
        {
            changeSceneController.Open();
            _audioService.PlayMusicMain();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                HandleSelector(true);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                HandleSelector(false);
            }


            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                SwitchTab();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                HandleBuy();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                changeSceneController.Close(() => SceneManager.LoadScene(Constants.MainMenu));
            }

            if (Input.GetKeyDown(KeyCode.Return))
            {
                changeSceneController.Close(() => SceneManager.LoadScene(Constants.GamePlay));
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                _playerService.UserCoin += 100;
                view.SetUserWallet(_playerService.UserCoin);
            }
        }

        private void SwitchTab()
        {
            _audioService.Button();

            _index = 0;
            view.contentShops[0].SetIndicator(_index);
            view.contentShops[1].SetIndicator(_index);
            switch (_currentChoose)
            {
                case DisplayShop.Armor:
                    _currentChoose = DisplayShop.Accessories;
                    break;
                case DisplayShop.Accessories:
                    _currentChoose = DisplayShop.Armor;
                    break;
            }

            view.ChangeTab(_currentChoose);
        }

        private void HandleSelector(bool isDown)
        {
            _audioService.Button();

            _index = isDown ? _index == 3 ? 0 : _index + 1 : _index == 0 ? 3 : _index - 1;
            switch (_currentChoose)
            {
                case DisplayShop.Armor:
                    view.contentShops[0].SetIndicator(_index);
                    break;
                case DisplayShop.Accessories:
                    view.contentShops[1].SetIndicator(_index);
                    break;
            }
        }

        private void HandleBuy()
        {
            var priceToBuy = 0;
            switch (_currentChoose)
            {
                case DisplayShop.Armor:
                    priceToBuy = model.GetPriceAmory(_index, _playerService.ArmoryLevel[_index]);
                    if (priceToBuy <= _playerService.UserCoin) // enough money
                    {
                        _audioService.BuyItem();

                        _playerService.UserCoin -= priceToBuy;
                        _playerService.ArmoryLevel[_index]++;
                        _playerService.SavePlayerData();
                        view.contentShops[0].UpdateNewValue(_index,
                            _playerService.ArmoryLevel[_index],
                            model.GetPriceAmory(_index, _playerService.ArmoryLevel[_index]));
                        skeletonAnimation.AnimationState.SetAnimation(0, BuyAnim, false);
                    }

                    break;
                case DisplayShop.Accessories:
                    priceToBuy = model.GetPriceAccessory(_index, _playerService.AccessoryLevel[_index]);
                    if (priceToBuy <= _playerService.UserCoin)
                    {
                        _audioService.BuyItem();
                        _playerService.UserCoin -= priceToBuy;
                        _playerService.AccessoryLevel[_index]++;
                        _playerService.SavePlayerData();
                        view.contentShops[1].UpdateNewValue(_index,
                            _playerService.AccessoryLevel[_index],
                            model.GetPriceAmory(_index, _playerService.AccessoryLevel[_index]));
                        skeletonAnimation.AnimationState.SetAnimation(0, BuyAnim, false);
                    }

                    break;
            }

            view.SetUserWallet(_playerService.UserCoin);
        }
    }


    public enum DisplayShop
    {
        Armor = 0,
        Accessories = 1,
    }
}