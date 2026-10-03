using System;
using System.Threading;

class Task3_Method3
{
    static void Main()
    {
        int N = Convert.ToInt32(Console.ReadLine());
        SemaphoreSlim[] semaphores = new SemaphoreSlim[N];
        for (int i = 0; i < N; i++)
            semaphores[i] = new SemaphoreSlim(0, 1);

        semaphores[N - 1].Release(); 

        Thread[] threads = new Thread[N];
        for (int i = 0; i < N; i++)
        {
            int id = i;
            threads[i] = new Thread(() =>
            {
                semaphores[id].Wait();
                Console.WriteLine($"Поток {id}");
                if (id > 0)
                    semaphores[id - 1].Release();
            });
            threads[i].Start();
        }

        foreach (Thread t in threads) t.Join();
    }
}