// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocket : MonoBehaviour
{
    public int Speed;
    Vector3 lastPos;
    public float radius;
    public float force;

    

    void Start()
    {
        lastPos = transform.position;
        Destroy(transform.gameObject,2f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);

        RaycastHit hit;

        Debug.DrawLine(lastPos, transform.position);
        if (Physics.Linecast(lastPos, transform.position, out hit))
        {
            Debug.Log(hit.collider.transform.parent);
            explore();
            Destroy(gameObject);
            
        }
        lastPos = transform.position;
    }
    
    void explore()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody rb = colliders[i].attachedRigidbody;
            if (rb != null)
            {
                rb.isKinematic = false;
                rb.AddExplosionForce(force, transform.position, radius, Random.Range(1, 5));
                if (colliders[i].transform.gameObject.tag == "obstacle")
                {
                    colliders[i].transform.gameObject.GetComponent<MeshCollider>().enabled = false;
                    Destroy(colliders[i].transform.gameObject, Random.Range(1,3));
                    
                }
            }
            
        }
    }
}
