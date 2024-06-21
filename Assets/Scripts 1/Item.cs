using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemType
    {
        Onion,
        BellPepper,
        Bacon,
        Tomato,
        Egg,
        Cheese,
        GroundMeat,
        Shiitake,
        Shimeji,
        SoySauce

    }

    // スプライトを設定するためのフィールド
    [SerializeField] Sprite onionSprite;
    [SerializeField] Sprite bellPepperSprite;
    [SerializeField] Sprite baconSprite;
    [SerializeField] Sprite tomatoSprite;
    [SerializeField] Sprite eggSprite;
    [SerializeField] Sprite cheeseSprite;
    [SerializeField] Sprite groundMeatSprite;
    [SerializeField] Sprite shiitakeSprite;
    [SerializeField] Sprite shimejiSprite;
    [SerializeField] Sprite soySauceSprite;


    public ItemType itemType;
    public int amount=1; // アイテムの量（回復量、弾薬量など）
    public Sprite itemSprite; // アイテムのスプライトを追加

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetSprite();
    }

    void SetSprite()
    {
        switch (itemType)
        {
            case ItemType.Onion:
                spriteRenderer.sprite = onionSprite;
                break;
            case ItemType.BellPepper:
                spriteRenderer.sprite = bellPepperSprite;
                break;
            case ItemType.Bacon:
                spriteRenderer.sprite = baconSprite;
                break;
            case ItemType.Tomato:
                spriteRenderer.sprite = tomatoSprite;
                break;
            case ItemType.Egg:
                spriteRenderer.sprite = eggSprite;
                break;
            case ItemType.Cheese:
                spriteRenderer.sprite =cheeseSprite;
                break;
            case ItemType.GroundMeat:
                spriteRenderer.sprite = groundMeatSprite;
                break;
            case ItemType.Shiitake:
                spriteRenderer.sprite = shiitakeSprite;
                break;
            case ItemType.Shimeji:
                spriteRenderer.sprite = shimejiSprite;
                break;
            case ItemType.SoySauce:
                spriteRenderer.sprite = soySauceSprite;
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerUI player = other.GetComponent<PlayerUI>();
            if (player != null)
            {
                player.PickUpItem(this);
                Destroy(gameObject); // アイテムを取得したら破壊する
            }
        }
    }
}
