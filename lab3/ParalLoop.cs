using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

class ParallelLoopProgram
{
    static void Main()
    {
        string filePath = "E:\\Tim_Labs\\points.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Файл {filePath} не найден.");
            return;
        }
        var points = File.ReadLines(filePath)
                         .Select(ParsePoint)
                         .Where(p => p.HasValue)
                         .Select(p => p.Value)
                         .ToList();

        if (points.Count == 0) return;

        Stopwatch sw = Stopwatch.StartNew();

        Vector3 totalSum = Vector3.Zero;
        object lockObj = new object();

        Parallel.ForEach(
            points,
            () => Vector3.Zero, 
            (point, state, localSum) => localSum + point, 
            (localSum) =>
            {
                lock (lockObj) 
                {
                    totalSum += localSum;
                }
            }
        );

        Vector3 geometricCenter = totalSum / points.Count;

        sw.Stop();

        Console.WriteLine($"Точек обработано: {points.Count}");
        Console.WriteLine($"Геометрический: X={geometricCenter.X:F4}, Y={geometricCenter.Y:F4}, Z={geometricCenter.Z:F4}");
        Console.WriteLine($"Время работы алгоритма: {sw.Elapsed.TotalMilliseconds:F2} мс");
    }

    static Vector3? ParsePoint(string line)
    {
        var parts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length >= 3 &&
            float.TryParse(parts[0], out float x) &&
            float.TryParse(parts[1], out float y) &&
            float.TryParse(parts[2], out float z))
        {
            return new Vector3(x, y, z);
        }
        return null;
    }
}
