using System;
using System.Threading;

class Zad2_Dynamic
{
    static void Main()
    {
        const int N = 16000;
        double[] a = new double[N];
        double[] b = new double[N];
        for (int i = 0; i < N; i++) a[i] = i;

        int numThreads = 8;
        int nextIndex = 1;       
        object locker = new object();

        Thread[] threads = new Thread[numThreads];
        for (int t = 0; t < numThreads; t++)
        {
            threads[t] = new Thread(() =>
            {
                while (true)
                {
                    int i;
                    lock (locker)
                    {
                        if (nextIndex >= N - 1) break; 
                        i = nextIndex;
                        nextIndex++;
                    }
                    b[i] = (a[i - 1] + a[i] + a[i + 1]) / 3.0;
                    Console.WriteLine($"Поток {t}: {b[i]}");
                }
            });
            threads[t].Start();
        }

        foreach (Thread th in threads) th.Join();
        Console.WriteLine("Динамическое распределение завершено");
    }
}