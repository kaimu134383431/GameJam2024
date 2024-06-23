using UnityEngine;

public class ItemPuller : MonoBehaviour
{
    [SerializeField] float pullDistance = 10f; // 引き寄せる距離の閾値

    private Camera mainCamera;
    private Transform playerTransform;

    private void Start()
    {
        mainCamera = Camera.main;
        playerTransform = transform;
    }

    private void Update()
    {
        // プレイヤーの現在位置を取得
        Vector3 playerPosition = playerTransform.position;

        // メインカメラの右端の70%の位置を計算
        float rightBoundary = mainCamera.ViewportToWorldPoint(new Vector3(0.7f, 0, 0)).x;

        // プレイヤーが右端の70%に達したかどうかをチェック
        if (playerPosition.x >= rightBoundary)
        {
            PullItems(); // アイテムを引き寄せる処理を呼び出す
        }
    }

    void PullItems()
    {
        // 画面内に存在する全てのItemタグを持つオブジェクトを取得
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject item in items)
        {
            // アイテムが画面内に存在するかチェックする
            if (IsItemInScreen(item))
            {
                // Itemオブジェクトの現在位置を取得
                Vector3 itemPosition = item.transform.position;

                // プレイヤーの位置に向かって移動させる処理
                item.transform.position = Vector3.MoveTowards(itemPosition, playerTransform.position, pullDistance * Time.deltaTime);
            }
        }
    }

    bool IsItemInScreen(GameObject item)
    {
        // アイテムの位置をスクリーン座標に変換
        Vector3 itemScreenPosition = mainCamera.WorldToScreenPoint(item.transform.position);

        // スクリーン座標がカメラのビューポート内にあるかどうかをチェック
        return itemScreenPosition.x >= 0 && itemScreenPosition.x <= Screen.width
            && itemScreenPosition.y >= 0 && itemScreenPosition.y <= Screen.height;
    }
}
