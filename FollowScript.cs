using UnityEngine;

public class FollowScript : MonoBehaviour
{
    public Transform target;
    public Vector3 PosionOffset;
    public Vector3 RotationOffset;

    public bool rotation;

    private void Update()
    {
        transform.position = target.position + PosionOffset;

        if (rotation)
            transform.rotation = Quaternion.Euler(target.eulerAngles + RotationOffset);
    }
}