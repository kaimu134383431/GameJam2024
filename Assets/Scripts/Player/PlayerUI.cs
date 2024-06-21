using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] GameObject itemUIContainer; // �A�C�e��UI��z�u����e�I�u�W�F�N�g
    [SerializeField] GameObject itemUIPrefab; // �A�C�e��UI�̃v���n�u

    private Dictionary<Item.ItemType, GameObject> itemUIInstances = new Dictionary<Item.ItemType, GameObject>(); // �A�C�e��UI�C���X�^���X���Ǘ�
    private Dictionary<Item.ItemType, Sprite> itemSprites = new Dictionary<Item.ItemType, Sprite>(); // �A�C�e���X�v���C�g���Ǘ�

    [SerializeField] private Sprite shimejiSprite;
    [SerializeField] private Sprite soySauceSprite;
    [SerializeField] private Sprite onionSprite;
    [SerializeField] private Sprite groundMeatSprite;
    [SerializeField] private Sprite shiitakeSprite;
    [SerializeField] private Sprite tomatoSprite;
    [SerializeField] private Sprite bellPepperSprite;
    [SerializeField] private Sprite eggSprite;
    [SerializeField] private Sprite cheeseSprite;
    [SerializeField] private Sprite baconSprite;

    void Start()
    {
        itemSprites[Item.ItemType.Shimeji] = shimejiSprite;
        itemSprites[Item.ItemType.SoySauce] = soySauceSprite;
        itemSprites[Item.ItemType.Onion] = onionSprite;
        itemSprites[Item.ItemType.GroundMeat] = groundMeatSprite;
        itemSprites[Item.ItemType.Shiitake] = shiitakeSprite;
        itemSprites[Item.ItemType.Tomato] = tomatoSprite;
        itemSprites[Item.ItemType.BellPepper] = bellPepperSprite;
        itemSprites[Item.ItemType.Egg] = eggSprite;
        itemSprites[Item.ItemType.Cheese] = cheeseSprite;
        itemSprites[Item.ItemType.Bacon] = baconSprite;

        UpdateUI();
    }

    public void PickUpItem(Item item)
    {
        GameManager.Instance.AddItem(item.itemType, item.amount);
        CheckItemCombination();
        UpdateUI();
    }

    void CheckItemCombination()
    {
        // �Q�[���}�l�[�W���[����A�C�e���̐����擾���ă��V�s���`�F�b�N
        GameManager gameManager = GameManager.Instance;
        Dictionary<Item.ItemType, int> items = gameManager.GetAllItems();
        // ���̂��X�p�Q�b�e�B
        if (items.ContainsKey(Item.ItemType.Shimeji) && items[Item.ItemType.Shimeji] >= 1 &&
            items.ContainsKey(Item.ItemType.SoySauce) && items[Item.ItemType.SoySauce] >= 1 &&
            items.ContainsKey(Item.ItemType.Onion) && items[Item.ItemType.Onion] >= 1)
        {
            gameManager.RemoveItem(Item.ItemType.Shimeji, 1);
            gameManager.RemoveItem(Item.ItemType.SoySauce, 1);
            gameManager.RemoveItem(Item.ItemType.Onion, 1);
            gameManager.AddScore(1000);   // �X�R�A�����Z
        }

        // �~�[�g�\�[�X
        if (items.ContainsKey(Item.ItemType.GroundMeat) && items[Item.ItemType.GroundMeat] >= 1 &&
            items.ContainsKey(Item.ItemType.Shiitake) && items[Item.ItemType.Shiitake] >= 1 &&
            items.ContainsKey(Item.ItemType.Tomato) && items[Item.ItemType.Tomato] >= 1 &&
            items.ContainsKey(Item.ItemType.BellPepper) && items[Item.ItemType.BellPepper] >= 1)
        {
            gameManager.RemoveItem(Item.ItemType.GroundMeat, 1);
            gameManager.RemoveItem(Item.ItemType.Shiitake, 1);
            gameManager.RemoveItem(Item.ItemType.Tomato, 1);
            gameManager.RemoveItem(Item.ItemType.BellPepper, 1);
            gameManager.AddScore(500);  // �X�R�A�����Z
        }

        // �J���{�i�[��
        if (items.ContainsKey(Item.ItemType.Egg) && items[Item.ItemType.Egg] >= 1 &&
            items.ContainsKey(Item.ItemType.Cheese) && items[Item.ItemType.Cheese] >= 1 &&
            items.ContainsKey(Item.ItemType.Bacon) && items[Item.ItemType.Bacon] >= 1)
        {
            gameManager.RemoveItem(Item.ItemType.Egg, 1);
            gameManager.RemoveItem(Item.ItemType.Cheese, 1);
            gameManager.RemoveItem(Item.ItemType.Bacon, 1);
            gameManager.AddScore(300);   // �X�R�A�����Z
        }

        // �i�|���^��
        if (items.ContainsKey(Item.ItemType.Onion) && items[Item.ItemType.Onion] >= 1 &&
            items.ContainsKey(Item.ItemType.BellPepper) && items[Item.ItemType.BellPepper] >= 1 &&
            items.ContainsKey(Item.ItemType.Bacon) && items[Item.ItemType.Bacon] >= 1 &&
            items.ContainsKey(Item.ItemType.Tomato) && items[Item.ItemType.Tomato] >= 1)
        {
            gameManager.RemoveItem(Item.ItemType.Onion, 1);
            gameManager.RemoveItem(Item.ItemType.BellPepper, 1);
            gameManager.RemoveItem(Item.ItemType.Bacon, 1);
            gameManager.RemoveItem(Item.ItemType.Tomato, 1);
            gameManager.AddScore(100);   // �X�R�A�����Z
        }
    }

    void UpdateUI()
    {
        // �X�R�A�̍X�V
        scoreText.text = "Score: " + GameManager.Instance.GetScore();

        // �A�C�e��UI�̍X�V
        foreach (var veg in GameManager.Instance.GetAllItems())
        {
            if (veg.Value >= 1)
            {
                if (!itemUIInstances.ContainsKey(veg.Key))
                {
                    // �V�����A�C�e��UI���쐬
                    GameObject itemUI = Instantiate(itemUIPrefab, itemUIContainer.transform);
                    itemUIInstances[veg.Key] = itemUI;
                    itemUI.GetComponentInChildren<Image>().sprite = itemSprites[veg.Key];
                }
                // �A�C�e���̌����X�V
                itemUIInstances[veg.Key].GetComponentInChildren<Text>().text = veg.Value.ToString();
            }
            else
            {
                if (itemUIInstances.ContainsKey(veg.Key))
                {
                    // �A�C�e��UI���폜
                    Destroy(itemUIInstances[veg.Key]);
                    itemUIInstances.Remove(veg.Key);
                }
            }
        }
    }
}
