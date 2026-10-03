using System;
using System.Collections.Concurrent;
using System.Threading;

class Task3_Method5
{
    static void Main()
    {
        int N = Convert.ToInt32(Console.ReadLine());
        ConcurrentStack<int> stack = new ConcurrentStack<int>();

        Thread[] threads = new Thread[N];
        for (int i = 0; i < N; i++)
        {
            int id = i;
            threads[i] = new Thread(() =>
            {
                stack.Push(id);
            });
            threads[i].Start();
        }

        foreach (Thread t in threads) t.Join();

        while (stack.TryPop(out int id))
            Console.WriteLine($"Поток {id}");
    }
}