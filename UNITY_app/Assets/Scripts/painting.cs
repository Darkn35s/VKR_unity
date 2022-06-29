// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class painting : MonoBehaviour
{
    // Start is called before the first frame update
    public Material[] materials;
    void Start()
    {
        gameObject.GetComponent<Renderer>().material = materials[Random.Range(0,materials.Length)];
        
    }
}
