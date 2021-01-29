using UnityEngine;

public class RotateWithParent : MonoBehaviour
{
    public Transform targetTransform;

    private void Start()
    {
    }

    private void Update()
    {
        Vector3 targetRot = targetTransform.localRotation.eulerAngles;
        transform.localRotation = Quaternion.Euler(targetRot.x, 180f - targetRot.y, targetRot.z); //Quaternion.Euler(transform.eulerAngles.x, targetRot.y, transform.eulerAngles.z);
    }
}