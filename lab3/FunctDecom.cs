using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

class FunctionalDecompositionProgram
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

        Task<float> taskX = Task.Run(() => points.Sum(p => p.X) / points.Count);
        Task<float> taskY = Task.Run(() => points.Sum(p => p.Y) / points.Count);
        Task<float> taskZ = Task.Run(() => points.Sum(p => p.Z) / points.Count);

        Task.WaitAll(taskX, taskY, taskZ);

        sw.Stop();

        Console.WriteLine($"Точек обработано: {points.Count}");
        Console.WriteLine($"Геометрический центр: X={taskX.Result:F4}, Y={taskY.Result:F4}, Z={taskZ.Result:F4}");
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
