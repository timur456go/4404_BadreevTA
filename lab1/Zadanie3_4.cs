using System;
using System.Threading;

class Task3_Method4
{
    static void Main()
    {
        int N = Convert.ToInt32(Console.ReadLine());
        int current = N - 1;
        object locker = new object();
        Barrier barrier = new Barrier(N); 

        Thread[] threads = new Thread[N];
        for (int i = 0; i < N; i++)
        {
            int id = i;
            threads[i] = new Thread(() =>
            {
                barrier.SignalAndWait(); 

                while (true)
                {
                    lock (locker)
                    {
                        if (current == id)
                        {
                            Console.WriteLine($"Поток {id}");
                            current--;
                            Monitor.PulseAll(locker);
                            return;
                        }
                        Monitor.Wait(locker);
                    }
                }
            });
            threads[i].Start();
        }

        foreach (Thread t in threads) t.Join();
    }
}