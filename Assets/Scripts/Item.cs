using UnityEngine;

public class Item : MonoBehaviour
{
    float pullDistance = 30f; // �����񂹂鋗����臒l
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

    // �X�v���C�g��ݒ肷�邽�߂̃t�B�[���h
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
    public int amount=1; // �A�C�e���̗ʁi�񕜗ʁA�e��ʂȂǁj

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
        // Item�I�u�W�F�N�g�̌��݈ʒu���擾
        Vector3 itemPosition = this.transform.position;

        // �v���C���[�̈ʒu�Ɍ������Ĉړ������鏈��
        this.transform.position = Vector3.MoveTowards(itemPosition, playertransform.position, pullDistance * Time.deltaTime);

    }
    public bool IsInDisplay()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
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
                Destroy(gameObject); // �A�C�e�����擾������j�󂷂�
            }
        }
    }
}
