using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [System.Serializable] public class Item
    {
        public Sprite image;
        public int price;
        public bool isPurchased = false;
        public bool isSelected = false;
    }
    [SerializeField] List<Item> Items;
    private GameObject itemTemplate;
    private GameObject g;
    [SerializeField] Transform content;
    [SerializeField] GameObject shopPanel;
    private Button buyButton;
    private Button selectButton;

    private GameManager gameManager;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        LoadShopData();

        itemTemplate = content.GetChild(0).gameObject;
        int leng = Items.Count;
        for (int i = 0; i < leng; i++)
        {
            g = Instantiate(itemTemplate, content);
            g.transform.GetChild(1).GetComponent<Image>().sprite = Items[i].image;
            g.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = Items[i].price.ToString();
            buyButton = g.transform.GetChild(3).GetComponent<Button>();
            buyButton.interactable = !Items[i].isPurchased;
            selectButton = g.transform.GetChild(4).GetComponent<Button>();
            if (Items[i].isPurchased)
            {
                buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "PURCHASED";
                buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 14;
                selectButton.gameObject.SetActive(true);
            }
            if (Items[i].isSelected)
            {
                g.transform.GetChild(0).gameObject.SetActive(true);
                player.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                g.transform.GetChild(0).gameObject.SetActive(false);
                player.GetChild(i).gameObject.SetActive(false);
            }
            buyButton.AddEventListener(i, OnShopItemBtnClicked);
            selectButton.AddEventListener(i, OnSelectBtnClicked);
        }
        Destroy(itemTemplate);
        shopPanel.SetActive(false);
    }

    void OnShopItemBtnClicked(int itemIndex)
    {
        if(gameManager.HasEnoughCoins(Items[itemIndex].price))
        {
            FindObjectOfType<AudioManager>().Play("Click");
            Items[itemIndex].isPurchased = true;
            buyButton = content.transform.GetChild(itemIndex).GetChild(3).GetComponent<Button>();
            buyButton.interactable = false;
            buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "PURCHASED";
            buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = 14;
            selectButton = content.transform.GetChild(itemIndex).GetChild(4).GetComponent<Button>();
            selectButton.gameObject.SetActive(true);
            gameManager.BuyItem(Items[itemIndex].price);
            UpdateShopData();
        }
        else
        {
            Debug.Log("No enough coins!!");
        }
    }
    void OnSelectBtnClicked(int itemIndex)
    {
        FindObjectOfType<AudioManager>().Play("Click");
        int leng = Items.Count;
        for (int i = 0; i < leng; i++)
        {
            Items[i].isSelected = false;
            content.transform.GetChild(i).GetChild(0).gameObject.SetActive(false);
            player.GetChild(i).gameObject.SetActive(false);
        }
        Items[itemIndex].isSelected = true;
        content.transform.GetChild(itemIndex).GetChild(0).gameObject.SetActive(true);
        player.GetChild(itemIndex).gameObject.SetActive(true);
        DataManager.Instance.ballIndex = itemIndex;
        UpdateShopData();
    }
    void UpdateShopData()
    {
        int leng = Items.Count;

        bool[] selected = new bool[leng];
        bool[] purchased = new bool[leng];
        
        for (int i = 0; i < leng;i++)
        {
            selected[i] = Items[i].isSelected;
        }

        for (int i = 0; i < leng; i++)
        {
            purchased[i] = Items[i].isPurchased;
        }

        DataManager.Instance.selectData = selected;
        DataManager.Instance.purchaseData = purchased;
        DataManager.Instance.Save();
    }

    void LoadShopData()
    {
        if(DataManager.Instance.purchaseData.Length != 0)
        {
            int leng = Items.Count;
            for (int i = 0; i < leng; i++)
            {
                Items[i].isSelected = DataManager.Instance.selectData[i];
            }
            for (int i = 0; i < leng; i++)
            {
                Items[i].isPurchased = DataManager.Instance.purchaseData[i];
            }
        }
    }
}