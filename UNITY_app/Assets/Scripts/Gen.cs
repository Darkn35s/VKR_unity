// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gen : MonoBehaviour
{
    public Material material;
    public GameObject[] tilePrefabs;
    private List<GameObject> activeTiles = new List<GameObject>();
    private float spawnPos = 0;
    private float tileLength = 600;
    private Color color = Color.white;
    public float em;

    [SerializeField] private Transform player;
    private int startTiles = 8;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < startTiles; i++)
        {
            SpawnTile(0);
        }
        StartCoroutine(Colorupd());
    }

    // Update is called once per frame
    void Update()
    {
        if (player.position.z - 600 > spawnPos - (startTiles * tileLength))
        {
            SpawnTile(Random.Range(0, tilePrefabs.Length));
            DeleteTile();
        }
        material.SetColor("_EmissionColor", Color.Lerp(material.GetColor("_EmissionColor"), color, 0.003f));

    }
    private void FixedUpdate()
    {
       
        
    }

    private void SpawnTile(int tileIndex)
    {
        GameObject nextTile = Instantiate(tilePrefabs[tileIndex], new Vector3(0f,-8f, transform.forward.z * spawnPos), transform.rotation);
        activeTiles.Add(nextTile);
        spawnPos += tileLength;
        

    }
    private void DeleteTile()
    {
        Destroy(activeTiles[0]);
        activeTiles.RemoveAt(0);
    }
    private IEnumerator Colorupd()
    {
        yield return new WaitForSeconds(4);
        color = new Color(Random.Range(1, 255) * em, Random.Range(1, 255) * em, Random.Range(1, 255)*em);
        StartCoroutine(Colorupd());
    }
}
