// This is a personal academic project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

using Keras.Models;
using Keras.Layers;
using Keras.Optimizers;
using Numpy;
using System.IO.MemoryMappedFiles;
using System.Net.Sockets;
using System.Text;
using System;

class Program
{
    private static int port = 3000;
    static int  TRANSPORT_BLOCK_HEADER_SIZE = 16;
    static int  SAMPLES_PER_TRANSPORT_BLOCK = 64;
    static int TCP_PACKET_SIZE = (int)(TRANSPORT_BLOCK_HEADER_SIZE / 4 + SAMPLES_PER_TRANSPORT_BLOCK) * 4;
    static BaseModel reconstructed_model = Sequential.LoadModel(@"C:\Users\Home\Desktop\ConvNet\modelNET_tr_for_5.h5");
    static bool mutexCreated;
    static Mutex mutex;
    static MemoryMappedFile sharedMemory;
    static MemoryMappedViewAccessor writer;

    static void Main()
    {
        mutex = new Mutex(true, "testmapmutex", out mutexCreated);
        
        sharedMemory = MemoryMappedFile.CreateOrOpen("MemoryFile", 6 * 4 + 4+2000*4 + 1);
        writer = sharedMemory.CreateViewAccessor(0, 6 * 4 + 4 + 2000 * 4 + 1);
        float[] a = new float[6];
        float[] samp = new float[2000];
        double k = 0.0000228;
        NDarray temp = new float[2000];
        int dataframes = 0;
        byte[] data = new byte[2048];
        NDarray samples= np.array(new float[0]);
        TcpClient client = new TcpClient("ip_address", port);
        NetworkStream stream = client.GetStream();
        mutex.ReleaseMutex();
        var coe = np.loadtxt(@"filter.txt");
        int bytes;
        
        while (true) {
            
            bytes = stream.Read(data, 0, 272);
            
            for (int sample = 16; sample < 272; sample += 4)
            {

                var ins = np.array(new float[] { BitConverter.ToInt32(data, sample) });
                samples = np.append(samples, ins);
                ins.Dispose();
                if (samples.len == 2001)
                {
                    samples = np.delete(samples, 0);
                }


            }
            if (samples.len == 2000)
            {
                for (int i = 0; i < 2000; i++)
                    temp[i] = samples[i];
                double mean = np.mean(samples);
                temp = temp.isub(mean);
                temp = temp.imul(k);
                temp = np.convolve(temp, coe, "same");
                
                
               
                NDarray maxid = np.argmax(temp);
                if ((float)temp[maxid] >= 0.2)
                {

                    if (968 <= (float)maxid&& 1032 >= (float)maxid)
                    {

                        
                        for (int i = 0; i < 2000; i++)
                        {
                            samp[i] = ((float)temp[i]);
                        }

                        Console.WriteLine(k+"       "+dataframes);
                        var result = reconstructed_model.Predict(x: temp.reshape(1, 2000));
                        
                        for (int i = 0; i < 3; i++)
                        {
                            a[i] = ((float)result[0][i]);
                        }
                        
                        a[3] = 0;
                        a[4] = 0;
                        a[5] = 0;
                        mutex.WaitOne();
                        writer.Write(0, 6);
                        writer.WriteArray<float>(4, a, 0, a.Length);
                        writer.Write(28, true);
                        writer.WriteArray<float>(29, samp, 0, samp.Length);
                        mutex.ReleaseMutex();
                        Console.WriteLine(result);

                        //samples = np.array(new int[0]);
                        
                        
                    }

                }
            }
        }
    }

   

    
}




