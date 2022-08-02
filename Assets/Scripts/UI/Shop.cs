using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class Shop : MonoBehaviour
{
    [System.Serializable] public class Item
    {
        public Sprite image;
        public int price;
        public bool isPurchased = false;
    }
    [SerializeField] List<Item> Items;
    private GameObject itemTemplate;
    private GameObject g;
    [SerializeField] Transform content;
    [SerializeField] GameObject shopPanel;
    private Button buyButton;

    // Start is called before the first frame update
    void Start()
    {
        itemTemplate = content.GetChild(0).gameObject;
        int leng = Items.Count;
        for (int i = 0; i < leng; i++)
        {
            g = Instantiate(itemTemplate, content);
            g.transform.GetChild(0).GetComponent<Image>().sprite = Items[i].image;
            g.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = Items[i].price.ToString();
            buyButton = g.transform.GetChild(2).GetComponent<Button>();
            buyButton.interactable = !Items[i].isPurchased;
            buyButton.AddEventListener(i, OnShopItemBtnClicked);
        }
        Destroy(itemTemplate);
        shopPanel.SetActive(false);
    }

    void OnShopItemBtnClicked(int itemIndex)
    {
        Items[itemIndex].isPurchased = true;
        buyButton = content.transform.GetChild(itemIndex).GetChild(2).GetComponent<Button>();
        buyButton.interactable = false;
        buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "PURCHASED";
        buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 14;
    }

}
