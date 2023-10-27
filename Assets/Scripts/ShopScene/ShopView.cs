using System.Collections.Generic;
using DG.Tweening;
using ShopScene.Component;
using TMPro;
using UnityEngine;

namespace ShopScene
{
    public class ShopView : MonoBehaviour
    {
        public List<ContentShop> contentShops;
        public List<TextMeshProUGUI> textContentShops;

        public void ChangeTab(DisplayShop newTab)
        {
            switch (newTab)
            {
                case DisplayShop.Armor:
                    contentShops[0].gameObject.SetActive(true);
                    textContentShops[0].color = Color.yellow;
                    textContentShops[0].gameObject.transform.DOScale(new Vector2(1.2f, 1.2f),0.5f);
                    
                    contentShops[1].gameObject.SetActive(false);
                    textContentShops[1].color = Color.white;
                    textContentShops[1].gameObject.transform.DOScale(Vector3.one, 0.5f);
                    break;
                case DisplayShop.Accessories:
                    contentShops[0].gameObject.SetActive(false);
                    textContentShops[0].color = Color.white;
                    textContentShops[0].gameObject.transform.DOScale(Vector3.one, 0.5f);

                    contentShops[1].gameObject.SetActive(true);
                    textContentShops[1].color = Color.yellow;
                    textContentShops[1].gameObject.transform.DOScale(new Vector2(1.2f, 1.2f),0.5f);
                    break;
            }
        }
    }
}