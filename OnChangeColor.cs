using UnityEngine;

public class OnChangeColor : MonoBehaviour
{
    public ExperimentState expState;
    private Material _mat;

    private void Start()
    {
        _mat = GetComponent<Renderer>().material;

        if (expState.group == 0)
            _mat.SetColor("_EmissionColor", Color.yellow);
    }

    private void OnTriggerStay(Collider other)
    {
        if (expState.group != 0)
            _mat.SetColor("_EmissionColor", Color.yellow);
    }

    private void OnTriggerExit(Collider other)
    {
        if (expState.group != 0)
            _mat.SetColor("_EmissionColor", Color.black);
    }
}