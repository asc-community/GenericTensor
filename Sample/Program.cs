using System;
using GenericTensor.Core;
using GenericTensor.Functions;
using System.Linq;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            //var a = new TensorInt();
            var arr = new int[30];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = i;
            var a = new DataArray<int>(arr);
            Console.WriteLine(a);
            var sliced1 = a.Slice(2, 16, 2);
            Console.WriteLine(sliced1);
            var sliced2 = sliced1.Slice(1, 3, 1);
            Console.WriteLine(sliced2);
        }
    }
}
