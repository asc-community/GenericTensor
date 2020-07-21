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
            
            var a = new TensorInt(3, 5);
            for (int i = 0; i < 3; i++)
            for (int j = 0; j < 5; j++)
            {
                var el = 5 * i + j;
                a[i, j] = el;
            }
            a.Transpose(0, 1);
            for (int i = 0; i < 5; i++)
                a[i, 0] = -1;
            Console.WriteLine(a);
            /*
            Console.WriteLine(a);
            a.Transpose(0, 1);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(a.ToString());
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            a.Transpose(0, 1);
            Console.WriteLine(a);*/
            
            Console.WriteLine(a);
            Console.WriteLine();Console.WriteLine();Console.WriteLine();
            var sub = a.GetSubtensor(1);
            Console.WriteLine(sub.ToString());
            
            //var dfgd = a[3];
        }
    }
}
