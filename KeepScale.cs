using UnityEngine;

public class KeepScale : MonoBehaviour
{
    private Vector3 origScale;

    // Start is called before the first frame update
    private void Start()
    {
        origScale = new Vector3(1f, 1f, 1f); //transform.localScale;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.localScale = origScale;
    }
}