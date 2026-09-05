using System;
using System.Threading;

class Zad3_1
{
    static void Main()
    {
        int N = 8;
        int expected = N - 1;
        object locker = new object();

        Thread[] threads = new Thread[N];
        for (int i = 0; i < N; i++)
        {
            int id = i;
            threads[i] = new Thread(() =>
            {
                while (true)
                {
                    lock (locker)
                    {
                        if (expected == id)
                        {
                            Console.WriteLine($"Поток {id}");
                            expected--;
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
