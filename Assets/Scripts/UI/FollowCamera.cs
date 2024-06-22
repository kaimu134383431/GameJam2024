using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private GameObject target;

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    void LateUpdate()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        if (target != null)
        {
            transform.position = new Vector3(target.transform.position.x, 0, transform.position.z);
        }
    }
}
