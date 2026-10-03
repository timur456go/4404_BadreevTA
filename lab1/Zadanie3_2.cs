using System;
using System.Threading;

class Task3_Method2
{
    static void Main()
    {
        int N = Convert.ToInt32(Console.ReadLine());
        ManualResetEvent[] events = new ManualResetEvent[N];
        for (int i = 0; i < N; i++)
            events[i] = new ManualResetEvent(false);

        events[N - 1].Set(); 

        Thread[] threads = new Thread[N];
        for (int i = 0; i < N; i++)
        {
            int id = i;
            threads[i] = new Thread(() =>
            {
                events[id].WaitOne();
                Console.WriteLine($"Поток {id}");
                if (id > 0)
                    events[id - 1].Set();
            });
            threads[i].Start();
        }

        foreach (Thread t in threads) t.Join();
    }
}