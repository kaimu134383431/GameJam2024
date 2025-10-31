using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class HPBarController : MonoBehaviour
{
    public Image fillImage;      // 塗り部分を参照
    public PlayerManager player; // HPを持つプレイヤースクリプトを参照

    [SerializeField] private Transform playerTransform;     // プレイヤーのTransform
    [SerializeField] private RectTransform uiElement;       // HPバーなどのUI
    [SerializeField] private float switchHeight;  // 高さの閾値
    [SerializeField] private float switchLeftX;     // 左右の閾値
    [SerializeField] private Text hpText;                   // HPバー隣の文字

    private bool isBottom = false; // 現在UIが下にあるかどうか

    void Update()
    {
        if (player == null || uiElement == null) return;

        // プレイヤーのHP割合を求めてバーに反映
        float fill = (float)player.CurrentHealth / player.MaxHealth;
        fillImage.fillAmount = fill;

        // プレイヤー座標を取得
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(playerTransform.position);
        //Debug.Log($"Player pos=({pos.x:F2}, {pos.y:F2}) | isBottom={isBottom}");


        // プレイヤーが閾値より上かつ左にいるとき → UIを下部へ移動
        if (viewportPos.y > switchHeight && viewportPos.x < switchLeftX && !isBottom)
        {
            //Debug.Log("→ Move to Bottom");
            MoveUIToBottom();
        }
        // プレイヤーが閾値より下にいるとき → UIを上部に戻す
        else if ((viewportPos.y <= switchHeight || viewportPos.x >= switchLeftX)&& isBottom)
        {
            //Debug.Log("→ Move to Top");
            MoveUIToTop();
        }
        

    }

    void MoveUIToTop()
    {
        uiElement.anchorMin = new Vector2(0.5f, 1f); // 上端基準
        uiElement.anchorMax = new Vector2(0.5f, 1f);
        uiElement.pivot = new Vector2(0.5f, 1f);
        uiElement.anchoredPosition = new Vector2(-180f, -10f); // 上から50px下
        isBottom = false;
    }

    void MoveUIToBottom()
    {
        uiElement.anchorMin = new Vector2(0.5f, 0f); // 下端基準
        uiElement.anchorMax = new Vector2(0.5f, 0f);
        uiElement.pivot = new Vector2(0.5f, 0f);
        uiElement.anchoredPosition = new Vector2(-180f, 10f); // 下から50px上
        isBottom = true;
    }
}