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
    [SerializeField] GameObject healthBarPrefab; // HPバーのプレハブを追加
    [SerializeField] Canvas canvas; // スコアポップアップを表示するキャンバス
    [SerializeField] Transform playerTransform; // プレイヤーのTransform

    private Image healthFillImage; // ← Sliderの代わりにImage
    private RectTransform healthBarRect; // HPバーのRectTransformを保持

    private const float maxDistance = 5f; // 最大距離（この距離で完全に透明になる）
    private const float minDistance = 1f; // 最小距離（この距離で完全に表示される）

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

    //[SerializeField] private Image hpFillImage; // HPバーの Fill Image
    //[SerializeField] private Color flashColor = Color.yellow; // 光る色
    //[SerializeField] private float flashDuration = 0.3f;
    //private Color originalColor;

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
            items.ContainsKey(Item.ItemType.SoySauce) && items[Item.ItemType.SoySauce] >= 1)
        {
            gameManager.RemoveItem(Item.ItemType.Shimeji, 1);
            gameManager.RemoveItem(Item.ItemType.SoySauce, 1);
            //gameManager.AddScore(1000);   // スコアを加算
            ScorePopup(1000, 0);
        }

        // ミートソース
        if (items.ContainsKey(Item.ItemType.GroundMeat) && items[Item.ItemType.GroundMeat] >= 1 &&
            items.ContainsKey(Item.ItemType.Shiitake) && items[Item.ItemType.Shiitake] >= 1)
        {
            gameManager.RemoveItem(Item.ItemType.GroundMeat, 1);
            gameManager.RemoveItem(Item.ItemType.Shiitake, 1);
            //gameManager.AddScore(500);  // スコアを加算
            ScorePopup(500, 1);
        }

        // カルボナーラ
        if (items.ContainsKey(Item.ItemType.Egg) && items[Item.ItemType.Egg] >= 1 &&
            items.ContainsKey(Item.ItemType.Cheese) && items[Item.ItemType.Cheese] >= 1)
        {
            gameManager.RemoveItem(Item.ItemType.Egg, 1);
            gameManager.RemoveItem(Item.ItemType.Cheese, 1);
            //gameManager.AddScore(300);   // スコアを加算
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
            //gameManager.AddScore(100);   // スコアを加算
            ScorePopup(100, 3);
        }
    }

    void UpdateUI()
    {
        // スコアの更新
        scoreText.text = "Score: " + GameManager.Instance.GetScore();
        /*
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
        }*/
    }

    void UpdateHealthUI()
    {
        if (healthFillImage == null || playerManager == null) return;

        // --- HP更新 ---
        float ratio = (float)playerManager.GetHealth() / playerManager.GetMaxHealth();
        healthFillImage.fillAmount = ratio;

        // --- 位置調整 ---
        if (playerTransform != null && mainCamera != null)
        {
            Vector3 playerScreenPos = mainCamera.WorldToViewportPoint(playerTransform.position);

            // 画面の縦位置(0～1)で分岐
            bool isUpperHalf = playerScreenPos.y > 0.5f;

            // UIアンカーと位置を動的変更
            Vector2 anchoredPos;
            if (isUpperHalf)
            {
                // プレイヤーが上半分にいる → HPバーを下端へ
                healthBarRect.anchorMin = new Vector2(0.5f, 0f);
                healthBarRect.anchorMax = new Vector2(0.5f, 0f);
                anchoredPos = new Vector2(0f, 40f); // 下端から40px上
            }
            else
            {
                // プレイヤーが下半分にいる → HPバーを上端へ
                healthBarRect.anchorMin = new Vector2(0.5f, 1f);
                healthBarRect.anchorMax = new Vector2(0.5f, 1f);
                anchoredPos = new Vector2(0f, -40f); // 上端から40px下
            }

            healthBarRect.anchoredPosition = anchoredPos;
        }
    }

    void ScorePopup(int scoreToAdd, int spriteIndex)
    {
        // プレイヤーの頭上に表示する位置を計算
        Vector3 worldPosition = playerTransform.position + new Vector3(0.5f, 1.5f, 0); // 1.5fは頭上のオフセット
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);

        // スコアポップアップを生成し、キャンバスの子オブジェクトとして配置
        GameObject popup = Instantiate(scorePopupPrefab, canvas.transform);

        // RectTransformを取得
        RectTransform popupRect = popup.GetComponent<RectTransform>();

        // Canvas上のローカル座標に変換してセット
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPosition,
            null,
            out localPoint
        );

        popupRect.localPosition = localPoint;

        // スコア内容をセット
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

    /*public IEnumerator FlashHPBar()
    {
        if (hpFillImage == null) yield break;

        hpFillImage.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        hpFillImage.color = originalColor;
    }*/

}