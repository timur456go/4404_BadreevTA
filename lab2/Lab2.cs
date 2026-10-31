using System;
using System.Diagnostics;
using System.Threading.Tasks;

class MatrixMultiplication
{
    static void Main()
    {
        Console.WriteLine("Настройка матрицы A");
        Console.Write("Введите количество строк матрицы A: ");
        int rowsA = int.Parse(Console.ReadLine());
        Console.Write("Введите количество столбцов матрицы A: ");
        int colsA = int.Parse(Console.ReadLine());

        Console.WriteLine("\nНастройка матрицы B");
        Console.Write("Введите количество строк матрицы B: ");
        int rowsB = int.Parse(Console.ReadLine());
        Console.Write("Введите количество столбцов матрицы B: ");
        int colsB = int.Parse(Console.ReadLine());

        if (colsA != rowsB)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[Ошибка]: Матрицы таких размеров невозможно перемножить!");
            Console.WriteLine($"Количество столбцов матрицы A ({colsA}) должно быть равно количеству строк матрицы B ({rowsB}).");
            return; 
        }

        Console.WriteLine($"\nГенерация матриц (A: {rowsA}x{colsA}, B: {rowsB}x{colsB})...");
        double[,] matrixA = GenerateMatrix(rowsA, colsA);
        double[,] matrixB = GenerateMatrix(rowsB, colsB);

        Console.WriteLine("\nЗапуск последовательного алгоритма...");
        long sequentialTime = MeasureTime(() => MultiplySequential(matrixA, matrixB, rowsA, colsA, colsB));
        Console.WriteLine($"Последовательное выполнение: {sequentialTime} мс");

        RunParallelTest(matrixA, matrixB, 2, rowsA, colsA, colsB);
        RunParallelTest(matrixA, matrixB, 4, rowsA, colsA, colsB);
        RunParallelTest(matrixA, matrixB, 8, rowsA, colsA, colsB);
    }

    static double[,] MultiplySequential(double[,] A, double[,] B, int rowsA, int colsA, int colsB)
    {
        double[,] C = new double[rowsA, colsB];

        for (int i = 0; i < rowsA; i++)
        {
            for (int j = 0; j < colsB; j++)
            {
                double sum = 0;
                for (int k = 0; k < colsA; k++)
                {
                    sum += A[i, k] * B[k, j];
                }
                C[i, j] = sum;
            }
        }
        return C;
    }

    static double[,] MultiplyParallel(double[,] A, double[,] B, int threadCount, int rowsA, int colsA, int colsB)
    {
        double[,] C = new double[rowsA, colsB];

        var options = new ParallelOptions
        {
            MaxDegreeOfParallelism = threadCount
        };

        Parallel.For(0, rowsA, options, i =>
        {
            for (int j = 0; j < colsB; j++)
            {
                double sum = 0;
                for (int k = 0; k < colsA; k++)
                {
                    sum += A[i, k] * B[k, j];
                }
                C[i, j] = sum;
            }
        });

        return C;
    }
    static void RunParallelTest(double[,] A, double[,] B, int threads, int rowsA, int colsA, int colsB)
    {
        Console.WriteLine($"\nЗапуск параллельного алгоритма ({threads} потока)...");
        long parallelTime = MeasureTime(() => MultiplyParallel(A, B, threads, rowsA, colsA, colsB));

        Console.WriteLine($"Параллельно ({threads} шт.): {parallelTime} мс");
    }
    static double[,] GenerateMatrix(int rows, int cols)
    {
        Random rand = new Random();
        double[,] matrix = new double[rows, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                matrix[i, j] = rand.NextDouble() * 10;
        return matrix;
    }
    static long MeasureTime(Action action)
    {
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Stopwatch sw = Stopwatch.StartNew();
        action();
        sw.Stop();
        return sw.ElapsedMilliseconds;
    }
}
