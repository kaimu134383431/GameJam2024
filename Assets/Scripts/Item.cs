using UnityEngine;

public class Item : MonoBehaviour
{
    float pullDistance = 30f; // 引き寄せる距離の閾値
    public bool pullflg = false;
    private Transform playertransform;
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

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetSprite();
        playertransform=GameObject.Find("Player").transform;

    }
    void Update()
    {
        if (pullflg)
        {
            movetoplayer();
        }
    }
    void movetoplayer()
    {
        // Itemオブジェクトの現在位置を取得
        Vector3 itemPosition = this.transform.position;

        // プレイヤーの位置に向かって移動させる処理
        this.transform.position = Vector3.MoveTowards(itemPosition, playertransform.position, pullDistance * Time.deltaTime);

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
        if (other.CompareTag("PlayerCollider"))
        {
            PlayerUI player = other.transform.parent.GetComponent<PlayerUI>();
            if (player != null)
            {
                SEManager.Instance.PlaySE("GetItem");
                player.PickUpItem(this);
                Destroy(gameObject); // アイテムを取得したら破壊する
            }
        }
    }
}
