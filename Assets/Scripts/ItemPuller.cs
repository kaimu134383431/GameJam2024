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
        // �v���C���[�̌��݈ʒu���擾
        Vector3 playerPosition = playerTransform.position;

        // ���C���J�����̉E�[��70%�̈ʒu���v�Z
        float rightBoundary = mainCamera.ViewportToWorldPoint(new Vector3(0.7f, 0, 0)).x;

        // �v���C���[���E�[��70%�ɒB�������ǂ������`�F�b�N
        if (playerPosition.x >= rightBoundary)
        {
            PullItems(); // �A�C�e���������񂹂鏈�����Ăяo��
        }
    }

    void PullItems()
    {
        // ��ʓ��ɑ��݂���S�Ă�Item�^�O�����I�u�W�F�N�g���擾
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");

        foreach (GameObject item in items)
        {
            Item script = item.GetComponent<Item>();
            if (script.IsInDisplay()){ // Item�̏o�͂Ȃǂ݂ȂƂɂȂǂ݂ȂƂɂȂǂ݂ȂƂɂȂǂ݂ȂƂɂȂǂ݂ȂƁ
                script.pullflg = true;
            }
 
        }
    }

    bool IsItemInScreen(GameObject item)
    {
        // �A�C�e���̈ʒu���X�N���[�����W�ɕϊ�
        Vector3 itemScreenPosition = mainCamera.WorldToScreenPoint(item.transform.position);

        // �X�N���[�����W���J�����̃r���[�|�[�g���ɂ��邩�ǂ������`�F�b�N
        return itemScreenPosition.x >= -0.1 && itemScreenPosition.x <= Screen.width + 0.1
            && itemScreenPosition.y >= -0.1 && itemScreenPosition.y <= Screen.height + 0.1;
    }
}
