using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
public class Spliting : MonoBehaviour
{

	public bool isdead=false; 
	public float timeRemaining = 2;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "rocket")
        {
            Debug.Log(collision.transform.name);
        }
    }
   


}


