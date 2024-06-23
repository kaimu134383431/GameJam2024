using UnityEngine;
//�X�N���[���̂��߂̃I�u�W�F�N�g
public class ForcedScroll : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float moveTime = 1.0f;
    public BossEnemy boss;
    public bool isToGoal = false;

    private float elapsedTime = 0.0f;
    private bool isMoving = false;

    // �v���C���[�I�u�W�F�N�g�̎Q��
    private GameObject player;
    private GameObject goal;
    // �X�N���[���̑��x
    private float scrollSpeed;
    // �v���C���[�̈ړ����x
    private float playerMoveSpeed;

    void Start()
    {
        // �X�N���[�����x���v�Z
        scrollSpeed = Vector3.Distance(startPosition, endPosition) / moveTime;

        // �v���C���[�I�u�W�F�N�g���擾
        player = GameObject.FindGameObjectWithTag("Player");
        GameObject bossObject = GameObject.FindGameObjectWithTag("Boss");
        if (bossObject != null)
        {
            boss = bossObject.GetComponent<BossEnemy>();
        }
        goal = GameObject.FindGameObjectWithTag("Goal");
        /*
        if (isToGoal)
        {
            if (boss != null) startPosition = boss.transform.position;
            if (goal != null)
            {
                Vector3 newPosition = goal.transform.position;
                newPosition.x -= 5.0f;
                endPosition = newPosition;
            }
        }
        else
        {
            if (player != null) startPosition = player.transform.position;
            if (boss != null)
            {
                Vector3 newPosition = boss.transform.position;
                newPosition.x -= 5.0f;
                endPosition = newPosition;
            }
        }*/

        // �v���C���[�̈ړ����x��ݒ�
        if (player != null)
        {
            playerMoveSpeed = scrollSpeed;
        }

        // �X�N���[�����J�n����
        StartScroll();
    }

    void Update()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveTime); // 0����1�͈̔͂ŕ�ԗ����v�Z

            // �J�n�ʒu����I���ʒu�܂ňړ�����
            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // �v���C���[���X�N���[���̕����Ɉړ�������
            if (player != null)
            {
                Vector3 scrollDirection = (endPosition - startPosition).normalized;
                player.transform.position += scrollDirection * playerMoveSpeed * Time.deltaTime;
            }

            // �ړ����Ԃ��I��������ړ����~����
            if (t >= 1.0f)
            {
                isMoving = false;
                if (boss != null) { 
                    boss.ShowHealthBar();
                    boss.setInvincible(false);
                }
                Destroy(gameObject);
            }
        }
    }

    public void StartScroll()
    {
        // �����X�N���[�����J�n����
        transform.position = startPosition;
        elapsedTime = 0.0f;
        isMoving = true;

        // FollowCamera�̑Ώۂɂ���
        GameObject followCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (followCamera != null)
        {
            followCamera.GetComponent<FollowCamera>().SetTarget(gameObject);
        }
    }
}
