// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System;

public class ShipCtrl : MonoBehaviour
{
    public GameObject rocketpos;
    public GameObject rocketpos2;
    private CharacterController controller;
    private Animator anim;
    public GameObject model;
    public ParticleSystem blink;
    public GameObject gun;
    public float range;
    public new AudioSource audio;
    public AudioClip sound;
    public GameObject rocket;
    public GameObject UIpannels;



    private Vector3 dir;

    [SerializeField] private float speed;
    [SerializeField] private GameObject losePanel;
    
   
    private int lineToMove = 1;
    private int stage = 1;
    public float lineDistance = 10;
    public float lineDistance2 = 8;
    private float[] moves = new float[6] { 0,0,0,0,0,0};
    private MemoryMappedFile sharedMemory;
    private Mutex mutex;
    private MemoryMappedViewAccessor reader;
    private bool upd;
    public float minimp;
    void Start()
    {
        anim = model.GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        StartCoroutine(SpeedIncrease());
        Time.timeScale = 1;
        sharedMemory = MemoryMappedFile.OpenExisting("MemoryFile");
        mutex = Mutex.OpenExisting("testmapmutex");
        reader = sharedMemory.CreateViewAccessor(4, 6 * 4 + 1);

    }

    private void Update()
    {
        
        
        if (mutex.WaitOne())
        {
                
          upd=reader.ReadBoolean(24);
          reader.Write(24,false);
          if (upd)
          {
             reader.ReadArray<float>(0, moves, 0, 6);
          }
          mutex.ReleaseMutex();
        }
        
        if (moves[2] >= minimp && upd)
        {
            rocketl();
        }

        if (moves[0] >= minimp && upd)
        {
            if (lineToMove < 2)
            {
                lineToMove++;
                anim.SetTrigger("right");

            }
        }

        if (moves[1] >= minimp && upd)
        {
            if (lineToMove > 0)
            {
                lineToMove--;
                anim.SetTrigger("left");
            }
        }
        //if (moves[3] >= minimp && upd)
        //{
        //    if (stage > 1)
        //    {
        //        stage--;
        //        anim.SetTrigger("down");
        //    }
        //}

        //if (moves[4]>= minimp && upd)
        //{
        //    if (stage < 2)
        //    {
        //        stage++;
        //        anim.SetTrigger("up");
        //    }
        //}


        anim.SetBool("flying", true);


        Vector3 targetPosition = transform.position.z * transform.forward;
        if (lineToMove == 0)
            targetPosition += Vector3.left * lineDistance;
        else if (lineToMove == 2)
            targetPosition += Vector3.right * lineDistance;
        if (stage == 2)
            targetPosition += Vector3.up * lineDistance2;


        if (transform.position == targetPosition)
            return;
        Vector3 diff = targetPosition - transform.position;
        Vector3 moveDir = diff.normalized * speed / 2 * Time.deltaTime;
        if (moveDir.sqrMagnitude < diff.sqrMagnitude)
            controller.Move(moveDir);
        else
            controller.Move(diff);
       
       

    }


    void FixedUpdate()
    {
        dir.z = speed;
        controller.Move(dir * Time.fixedDeltaTime);
        uictrl("right", moves[0]);
        uictrl("left", moves[1]);
        uictrl("rock", moves[2]);

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        if (hit.gameObject.tag == "obstacle")
        {
            losePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    private IEnumerator SpeedIncrease()
    {
        yield return new WaitForSeconds(4);
        if (speed<350)
            speed += 20;
        StartCoroutine(SpeedIncrease());
    }
    

    void shoot()
    {
        audio.PlayOneShot(sound);
        blink.Play();
        RaycastHit hit;
        if (Physics.Raycast(gun.transform.position, gun.transform.forward, out hit,range))
        {
            Debug.Log(hit.collider.tag);
            GameObject obj = hit.collider.gameObject;
            Destroy(obj.transform.gameObject);
            blink.Clear();
        }
    }

    void rocketl()
    {
        Instantiate(rocket, rocketpos.transform.position, rocketpos.transform.rotation);
        Instantiate(rocket, rocketpos2.transform.position, rocketpos2.transform.rotation);
    }


    void uictrl(string PanNum, float acc)
    {
        UIpannels.transform.Find(PanNum+"0").gameObject.SetActive(false);
        UIpannels.transform.Find(PanNum+"25").gameObject.SetActive(false);
        UIpannels.transform.Find(PanNum+"50").gameObject.SetActive(false);
        UIpannels.transform.Find(PanNum +"75").gameObject.SetActive(false);
        if (acc*100>=0)
            UIpannels.transform.Find(PanNum+"0").gameObject.SetActive(true);
        if (acc * 100 >= 25)
            UIpannels.transform.Find(PanNum + "25").gameObject.SetActive(true);
        if (acc * 100 >= 50)
            UIpannels.transform.Find(PanNum + "50").gameObject.SetActive(true);
        if (acc * 100 >= 75)
            UIpannels.transform.Find(PanNum + "75").gameObject.SetActive(true);


    }


    private void OnDestroy()
    {
        sharedMemory.Dispose();
        reader.Dispose();
    }
}
