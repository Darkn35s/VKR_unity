// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class points : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Transform ship;
    [SerializeField] private Text score;

    private void Update()
    {
        score.text=((int)(ship.position.z/2)).ToString();
    }
}
