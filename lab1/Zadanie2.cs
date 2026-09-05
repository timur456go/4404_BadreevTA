using System;
using System.Threading;

class Zad2_Static
{
    static void Main()
    {
        const int N = 16000;
        double[] a = new double[N];
        double[] b = new double[N];
        for (int i = 0; i < N; i++) a[i] = i;

        int numThreads = 8;
        int chunkSize = (N - 2 + numThreads - 1) / numThreads; // округление вверх

        Thread[] threads = new Thread[numThreads];
        for (int j = 0; j < numThreads; j++)
        {
            int threadIdx = j; 
            threads[j] = new Thread(() =>
            {
                int start = threadIdx * chunkSize + 1;
                int end = Math.Min(start + chunkSize, N - 1);
                for (int i = start; i < end; i++) {
                    b[i] = (a[i - 1] + a[i] + a[i + 1]) / 3.0;
                    Console.WriteLine($"Поток {threadIdx}: {b[i]}");
                }
            });
            threads[j].Start();
        }

        foreach (Thread th in threads) th.Join();
        Console.WriteLine("Статическое распределение завершено");
    }
}
