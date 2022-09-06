using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        public Mesh mesh;
        public Material[] materials;
        public int price;
        public bool isPurchased = false;
        public bool isSelected = false;
        public Vector3 scale;
    }
    [SerializeField] List<Item> Items;
    private GameObject itemTemplate;
    private GameObject g;
    [SerializeField] Transform content;
    [SerializeField] GameObject shopPanel;
    private Button buyButton;
    private Button selectButton;
    private int purchasedTextFontSize = 17;

    private GameManager gameManager;
    private Transform player;
    public Material _material;

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
            g.transform.GetChild(1).GetComponent<MeshFilter>().sharedMesh = Items[i].mesh;
            g.transform.GetChild(1).transform.localScale = Items[i].scale;

            List<Material> listMaterial = new List<Material>();
            for (int a = 0; a < Items[i].materials.Length; a++)
            {
                listMaterial.Add(Items[i].materials[a]);
            }
            g.transform.GetChild(1).GetComponent<Renderer>().materials = listMaterial.ToArray();

            g.transform.GetChild(2).GetChild(1).GetComponent<TextMeshProUGUI>().text = Items[i].price.ToString();
            buyButton = g.transform.GetChild(3).GetComponent<Button>();
            buyButton.interactable = !Items[i].isPurchased;
            selectButton = g.transform.GetChild(4).GetComponent<Button>();
            if (Items[i].isPurchased)
            {
                TextMeshProUGUI buyBtnText = buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                buyBtnText.text = "PURCHASED";
                buyBtnText.fontSize = purchasedTextFontSize;
                buyBtnText.color = Color.gray;
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
        if (gameManager.HasEnoughCoins(Items[itemIndex].price))
        {
            FindObjectOfType<AudioManager>().Play("Click");
            Items[itemIndex].isPurchased = true;
            buyButton = content.transform.GetChild(itemIndex).GetChild(3).GetComponent<Button>();
            buyButton.interactable = false;
            buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "PURCHASED";
            buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().fontSize = purchasedTextFontSize;
            buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.gray;
            selectButton = content.transform.GetChild(itemIndex).GetChild(4).GetComponent<Button>();
            selectButton.gameObject.SetActive(true);
            gameManager.BuyItem(Items[itemIndex].price);
            UpdateShopData();
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

        for (int i = 0; i < leng; i++)
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
        if (DataManager.Instance.purchaseData.Length != 0)
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