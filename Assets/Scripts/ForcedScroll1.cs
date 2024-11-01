using UnityEngine;

public class ForcedScroll1 : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float moveTime = 1.0f;
    public BossEnemy boss;
    public bool isToGoal = false;

    private float elapsedTime = 0.0f;
    private bool isMoving = false;

    void Start()
    {
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
            //transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // �ړ����Ԃ��I�������疳�G�����ƕ\�����s��
            if (t >= 1.0f)
            {
                isMoving = false;
                if (boss != null)
                {
                    boss.ShowHealthBar();
                    boss.setInvincible(false);
                    //�ǉ�
                    boss.isWait = false;
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
