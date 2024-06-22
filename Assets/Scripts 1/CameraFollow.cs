using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    public float moveTime = 1.0f;

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
            transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // �ړ����Ԃ��I��������ړ����~����
            if (t >= 1.0f)
            {
                isMoving = false;
            }
        }
    }

    public void StartScroll()
    {
        // �����X�N���[�����J�n����
        transform.position = startPosition;
        elapsedTime = 0.0f;
        isMoving = true;
    }
}

