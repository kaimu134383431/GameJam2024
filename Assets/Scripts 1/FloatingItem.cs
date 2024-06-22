using UnityEngine;

public class FloatingItem : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private float moveDuration = 1f; // �ړ��ɂ����鎞��
    private float elapsedTime = 0f;
    private bool isMoving = false;

    public void SetTargetPosition(Vector3 target)
    {
        startPosition = transform.position;
        targetPosition = target;
        isMoving = true;
        elapsedTime = 0f;
    }

    void Update()
    {
        if (isMoving)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / moveDuration;
            if (t > 1f)
            {
                t = 1f;
                isMoving = false;
            }

            // ���񂾂�x���Ȃ�悤�ɐi�s�x�𔽓]������
            float easeOutT = Mathf.Sin(t * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(startPosition, targetPosition, easeOutT);
        }
    }
}
