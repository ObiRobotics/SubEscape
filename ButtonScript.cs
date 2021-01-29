using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public static bool pressing; 

    public MeshRenderer buttonMesh;
    public Light buttonLight; 
    public Color PressedColor;
    public AudioSource PressSound; 

    private Color initColor;
    private Color initLightColor;

    void Start()
    {
        initColor = GetComponent<MeshRenderer>().material.color;
        initLightColor = buttonLight.color; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            pressing = true;
            buttonMesh.material.color = PressedColor;
            buttonLight.color = PressedColor;
            PressSound.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        pressing = true; 
        buttonMesh.material.color = PressedColor;
        buttonLight.color = PressedColor;
        PressSound.Play(); 
    }

    void OnTriggerExit(Collider other)
    {
        pressing = false; 
        buttonMesh.material.color = initColor;
        buttonLight.color = initLightColor; 
    }

}
