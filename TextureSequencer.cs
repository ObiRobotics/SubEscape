using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureSequencer : MonoBehaviour
{

    public Texture[] textures;
    public int currentTexture;

    Renderer _renderer;

    void Start()
    {
        _renderer = GetComponent<Renderer>();

        StartCoroutine(TextureLooper());
    }

    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    currentTexture++;
        //    currentTexture %= textures.Length;

        //    _renderer.material.mainTexture = textures[currentTexture];
        //}
    }

    IEnumerator TextureLooper()
    {
        while(true) 
        {
            currentTexture++;
            currentTexture %= textures.Length;
            
            _renderer.material.mainTexture = textures[currentTexture];

            yield return new WaitForSeconds(0.05f); 
        }

    }
}
