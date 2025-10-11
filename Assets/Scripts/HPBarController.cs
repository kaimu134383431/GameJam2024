using UnityEngine;
using UnityEngine.UI;

public class HPBarController : MonoBehaviour
{
    public Image fillImage;     // 塗り部分を参照
    public PlayerManager player;       // HPを持つプレイヤースクリプトを参照

    void Update()
    {
        // プレイヤーのHP割合を求めてバーに反映
        float fill = (float)player.CurrentHealth / player.MaxHealth;
        fillImage.fillAmount = fill;
    }
}