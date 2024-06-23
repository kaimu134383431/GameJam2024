using UnityEngine;

public class ItemPuller : MonoBehaviour
{
    [SerializeField] float pullDistance = 10f; // �����񂹂鋗����臒l

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
            // �A�C�e������ʓ��ɑ��݂��邩�`�F�b�N����
            if (IsItemInScreen(item))
            {
                // Item�I�u�W�F�N�g�̌��݈ʒu���擾
                Vector3 itemPosition = item.transform.position;

                // �v���C���[�̈ʒu�Ɍ������Ĉړ������鏈��
                item.transform.position = Vector3.MoveTowards(itemPosition, playerTransform.position, pullDistance * Time.deltaTime);
            }
        }
    }

    bool IsItemInScreen(GameObject item)
    {
        // �A�C�e���̈ʒu���X�N���[�����W�ɕϊ�
        Vector3 itemScreenPosition = mainCamera.WorldToScreenPoint(item.transform.position);

        // �X�N���[�����W���J�����̃r���[�|�[�g���ɂ��邩�ǂ������`�F�b�N
        return itemScreenPosition.x >= 0 && itemScreenPosition.x <= Screen.width
            && itemScreenPosition.y >= 0 && itemScreenPosition.y <= Screen.height;
    }
}
