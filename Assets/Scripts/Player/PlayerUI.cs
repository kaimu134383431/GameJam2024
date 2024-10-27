using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] Text scoreText;
    [SerializeField] GameObject itemUIContainer; // アイテムUIを配置する親オブジェクト
    [SerializeField] GameObject itemUIPrefab; // アイテムUIのプレハブ
    [SerializeField] GameObject scorePopupPrefab; // スコアポップアップのプレハブ
    [SerializeField] GameObject healthSliderPrefab; // HPバーのプレハブを追加
    [SerializeField] Canvas canvas; // スコアポップアップを表示するキャンバス
    [SerializeField] Transform playerTransform; // プレイヤーのTransform

    private const float maxDistance = 5f; // 最大距離（この距離で完全に透明になる）
    private const float minDistance = 1f; // 最小距離（この距離で完全に表示される）

    private Slider healthSliderInstance;

    private Dictionary<Item.ItemType, GameObject> itemUIInstances = new Dictionary<Item.ItemType, GameObject>(); // アイテムUIインスタンスを管理
    private Dictionary<Item.ItemType, Sprite> itemSprites = new Dictionary<Item.ItemType, Sprite>(); // アイテムスプライトを管理
    private Camera mainCamera;
    private PlayerManager playerManager; // PlayerManagerのインスタンスを保持する変数

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
        mainCamera = Camera.main;
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
        playerManager = FindObjectOfType<PlayerManager>(); // PlayerManagerのインスタンスを取得

        // HPバーのインスタンスを作成
        GameObject healthSliderObject = Instantiate(healthSliderPrefab, canvas.transform);
        healthSliderInstance = healthSliderObject.GetComponent<Slider>(); // Sliderコンポーネントを取得

        UpdateUI();
    }

    void FixedUpdate()
    {
        UpdateHealthUI();
    }

    public void PickUpItem(Item item)
    {
        GameManager.Instance.AddItem(item.itemType, item.amount);
        CheckItemCombination();
        UpdateUI();
    }

    void CheckItemCombination()
    {
        // ゲームマネージャーからアイテムの数を取得してレシピをチェック
        GameManager gameManager = GameManager.Instance;
        Dictionary<Item.ItemType, int> items = gameManager.GetAllItems();
        // きのこスパゲッティ
        if (items.ContainsKey(Item.ItemType.Shimeji) && items[Item.ItemType.Shimeji] >= 1 &&
            items.ContainsKey(Item.ItemType.SoySauce) && items[Item.ItemType.SoySauce] >= 1 &&
            items.ContainsKey(Item.ItemType.Onion) && items[Item.ItemType.Onion] >= 1)
        {
            gameManager.RemoveItem(Item.ItemType.Shimeji, 1);
            gameManager.RemoveItem(Item.ItemType.SoySauce, 1);
            gameManager.RemoveItem(Item.ItemType.Onion, 1);
            gameManager.AddScore(1000);   // スコアを加算
            ScorePopup(1000, 0);
        }

        // ミートソース
        if (items.ContainsKey(Item.ItemType.GroundMeat) && items[Item.ItemType.GroundMeat] >= 1 &&
            items.ContainsKey(Item.ItemType.Shiitake) && items[Item.ItemType.Shiitake] >= 1 &&
            items.ContainsKey(Item.ItemType.Tomato) && items[Item.ItemType.Tomato] >= 1 &&
            items.ContainsKey(Item.ItemType.BellPepper) && items[Item.ItemType.BellPepper] >= 1)
        {
            gameManager.RemoveItem(Item.ItemType.GroundMeat, 1);
            gameManager.RemoveItem(Item.ItemType.Shiitake, 1);
            gameManager.RemoveItem(Item.ItemType.Tomato, 1);
            gameManager.RemoveItem(Item.ItemType.BellPepper, 1);
            gameManager.AddScore(500);  // スコアを加算
            ScorePopup(500, 1);
        }

        // カルボナーラ
        if (items.ContainsKey(Item.ItemType.Egg) && items[Item.ItemType.Egg] >= 1 &&
            items.ContainsKey(Item.ItemType.Cheese) && items[Item.ItemType.Cheese] >= 1 &&
            items.ContainsKey(Item.ItemType.Bacon) && items[Item.ItemType.Bacon] >= 1)
        {
            gameManager.RemoveItem(Item.ItemType.Egg, 1);
            gameManager.RemoveItem(Item.ItemType.Cheese, 1);
            gameManager.RemoveItem(Item.ItemType.Bacon, 1);
            gameManager.AddScore(300);   // スコアを加算
            ScorePopup(300, 2);
        }

        // ナポリタン
        if (items.ContainsKey(Item.ItemType.Onion) && items[Item.ItemType.Onion] >= 1 &&
            items.ContainsKey(Item.ItemType.BellPepper) && items[Item.ItemType.BellPepper] >= 1 &&
            items.ContainsKey(Item.ItemType.Bacon) && items[Item.ItemType.Bacon] >= 1 &&
            items.ContainsKey(Item.ItemType.Tomato) && items[Item.ItemType.Tomato] >= 1)
        {
            gameManager.RemoveItem(Item.ItemType.Onion, 1);
            gameManager.RemoveItem(Item.ItemType.BellPepper, 1);
            gameManager.RemoveItem(Item.ItemType.Bacon, 1);
            gameManager.RemoveItem(Item.ItemType.Tomato, 1);
            gameManager.AddScore(100);   // スコアを加算
            ScorePopup(100, 3);
        }
    }

    void UpdateUI()
    {
        // スコアの更新
        scoreText.text = "Score: " + GameManager.Instance.GetScore();

        // アイテムUIの更新
        foreach (var veg in GameManager.Instance.GetAllItems())
        {
            if (veg.Value >= 1)
            {
                if (!itemUIInstances.ContainsKey(veg.Key))
                {
                    // 新しいアイテムUIを作成
                    GameObject itemUI = Instantiate(itemUIPrefab, itemUIContainer.transform);
                    itemUIInstances[veg.Key] = itemUI;
                    itemUI.GetComponentInChildren<Image>().sprite = itemSprites[veg.Key];
                }
                // アイテムの個数を更新
                itemUIInstances[veg.Key].GetComponentInChildren<Text>().text = veg.Value.ToString();
            }
            else
            {
                if (itemUIInstances.ContainsKey(veg.Key))
                {
                    // アイテムUIを削除
                    Destroy(itemUIInstances[veg.Key]);
                    itemUIInstances.Remove(veg.Key);
                }
            }
        }
    }

    void UpdateHealthUI()
    {
        if (healthSliderInstance != null && playerManager != null) // playerManagerがnullでないことを確認
        {
            // HPの割合を計算してスライダーに反映
            healthSliderInstance.value = (float)playerManager.GetHealth() / playerManager.GetMaxHealth();
        }
    }

        void ScorePopup(int scoreToAdd, int spriteIndex)
    {
        // プレイヤーの頭上に表示する位置を計算
        Vector3 worldPosition = playerTransform.position + new Vector3(1.0f, 0.5f, 0); // 1.5fは頭上のオフセット
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // スコアポップアップを生成し、キャンバスの子オブジェクトとして配置
        GameObject popup = Instantiate(scorePopupPrefab, canvas.transform);
        popup.transform.position = screenPosition;
        ScorePopup scorePopup = popup.GetComponent<ScorePopup>();
        if (scorePopup != null)
        {
            scorePopup.SetScore(scoreToAdd, spriteIndex); // スコアとスプライトのインデックスを設定
        }
        else
        {
            Debug.LogError("ScorePopup component not found on instantiated prefab");
        }
    }


}
