// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Сamctrl : MonoBehaviour
{
    [SerializeField] private Transform player;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - player.position;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newpos = new Vector3(player.position.x, offset.y+player.position.y, offset.z + player.position.z);
        transform.position = newpos;
    }
}
