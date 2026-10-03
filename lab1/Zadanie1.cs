using System;
using System.Threading;

class A
{
    static void Main()
    {
        int numThreads = Convert.ToInt32(Console.ReadLine());
       
        Thread[] threads = new Thread[numThreads];

        for (int i = 0; i < numThreads; i++)
        {
            int threadID = i; 
            threads[i] = new Thread(() =>
            {
                Console.WriteLine($"Поток {threadID+1} из {numThreads}");
            });
            threads[i].Start();
        }

        foreach (Thread t in threads)
            t.Join();
    }
}
