using System;
using GenericTensor.Core;
using GenericTensor.Functions;
using System.Linq;

namespace Sample
{
    public class TensorStringWrapper : ITensorElement<string>
    {
        private string val;
        public string GetValue() => val;
        public void SetValue(string newValue) => this.val = newValue;
        public override string ToString()
            => val;

        public ITensorElement<string> Copy()
        {
            var res = new TensorStringWrapper();
            res.val = val;
            return res;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*
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
            Console.WriteLine();Console.WriteLine();Console.WriteLine();
            for (int i = 0; i < a.Shape[0]; i++)
                Console.WriteLine(a.GetSubtensor(i));
            */
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


            var A = new Tensor<TensorStringWrapper, string>(4, 2, 3);
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 2; j++)
                for (int k = 0; k < 3; k++)
                    A[i, j, k] = "[" + i.ToString() + "  " + j.ToString() + " " + k.ToString() + "]";


            Console.WriteLine("NO TRANSPOSITION");
            Console.WriteLine();
            for (int i = 0; i < A.Shape[0]; i++)
            {
                Console.WriteLine(A.GetSubtensor(i));
                Console.WriteLine();Console.WriteLine();
            }
            Console.WriteLine();Console.WriteLine();Console.WriteLine();
            Console.WriteLine("0, 1");
            A.Transpose(0, 1);
            Console.WriteLine();
            for (int i = 0; i < A.Shape[0]; i++)
            {
                Console.WriteLine(A.GetSubtensor(i));
                Console.WriteLine();Console.WriteLine();
            }

            A.Transpose(0, 1);

            Console.WriteLine();Console.WriteLine();Console.WriteLine();
            Console.WriteLine("0, 2");
            A.Transpose(0, 2);
            Console.WriteLine();
            for (int i = 0; i < A.Shape[0]; i++)
            {
                Console.WriteLine(A.GetSubtensor(i));
                Console.WriteLine();Console.WriteLine();
            }

            A.Transpose(0, 2);

            Console.WriteLine();Console.WriteLine();Console.WriteLine();
            Console.WriteLine("1, 2");
            A.Transpose(1, 2);
            Console.WriteLine();
            for (int i = 0; i < A.Shape[0]; i++)
            {
                Console.WriteLine(A.GetSubtensor(i));
                Console.WriteLine();Console.WriteLine();
            }

            //var sub = a.GetSubtensor(1);
            //Console.WriteLine(sub.ToString());

            //var dfgd = a[3];
        }
    }
}
