// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.MemoryMappedFiles;
using System.Threading;
using System;
public class EMGsignal : MonoBehaviour
{
    // Start is called before the first frame update
    private MemoryMappedFile sharedMemory;
    private Mutex mutex;
    private MemoryMappedViewAccessor reader;
    private bool upd;
    private float[] data=new float[2000];
    
    private LineRenderer lr;
    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }
    private void Start()
    {
        sharedMemory = MemoryMappedFile.OpenExisting("MemoryFile");
        mutex = Mutex.OpenExisting("testmapmutex");
        reader = sharedMemory.CreateViewAccessor(4, 2000 * 4+25);
        lr.positionCount =2000;


    }

    private void FixedUpdate()
    {
        if (mutex.WaitOne())
        {
            reader.ReadArray<float>(25, data, 0, 2000);
            mutex.ReleaseMutex();
            LineUpd();
            
        }
        
    }
        
    private void LineUpd()
    {
        for(int i = 0; i < 2000; i++)
        {
            lr.SetPosition(i, new Vector3((float)i * 100 / 2000, data[i], 1));
        }
    }
        
    
    private void OnDestroy()
    {
        sharedMemory.Dispose();
        reader.Dispose();
    }

}
