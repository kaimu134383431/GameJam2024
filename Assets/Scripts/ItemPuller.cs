using UnityEngine;

public class ItemPuller : MonoBehaviour
{

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
            Item script = item.GetComponent<Item>();
            script.pullflg = true;
 
        }
    }

    bool IsItemInScreen(GameObject item)
    {
        // アイテムの位置をスクリーン座標に変換
        Vector3 itemScreenPosition = mainCamera.WorldToScreenPoint(item.transform.position);

        // スクリーン座標がカメラのビューポート内にあるかどうかをチェック
        return itemScreenPosition.x >= -0.1 && itemScreenPosition.x <= Screen.width + 0.1
            && itemScreenPosition.y >= -0.1 && itemScreenPosition.y <= Screen.height + 0.1;
    }
}
