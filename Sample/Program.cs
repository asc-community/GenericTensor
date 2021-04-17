using System;
using GenericTensor.Core;
using GenericTensor.Functions;
using Sample;

var m2 = GenTensor<double, DoubleWrapper>.CreateMatrix(new double[,]
    {
        { 4, 2, 3 },
        { 1, 3, 3 },
    }
);

Console.WriteLine(m2.GaussianEliminationSimple());
Console.WriteLine(m2.GaussianEliminationSafeDivision());
return;

var m = GenTensor<double, DoubleWrapper>.CreateMatrix(new double[,]
    {
        { 1, 2, 3 },
        { 1, 3, 3 }
    }
);

Console.WriteLine(m);

var n = m.Copy(false);
n.TransposeMatrix();

Console.WriteLine(m);

return;

var M = GenTensor<double, DoubleWrapper>.CreateMatrix(new double[,]
{
    { -3,  0,  0,  0,   1,  -130 },
    {  0, -2,  1,  0,   0,   -39 },
    {  0,  0, -3,  2,   0,     0 },
    {  0,  0,  0, -7,   3,  -273 },
    {  0,  0,  0,  0, -26, -1729 }
}
);

Console.WriteLine(M.GaussianEliminationSafeDivision());
return;

var A = new MyMatrix(3, 2);
A[0, 0] = 5; A[0, 1] = 6;
A[1, 0] = 3; A[1, 1] = 8;
A[2, 0] = 2; A[2, 1] = 9;

var B = new MyMatrix(2, 2);
B[0, 0] = 5; B[1, 0] = 6;
B[0, 1] = 3; B[1, 1] = 8;

Console.WriteLine(A);
Console.WriteLine("\nmultiplied by \n");
Console.WriteLine(B);
Console.WriteLine("\nis \n");
Console.WriteLine(A * B);

public static class Samples
{
    public static void CreatingMatrix()
    {
        var myMatrix = GenTensor<float, FloatWrapper>.CreateMatrix(
            new float[,]
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            }
            );
        Console.WriteLine(myMatrix);
    }

    public static void CreatingMatrixAndMultiply()
    {
        var myMatrix = GenTensor<float, FloatWrapper>.CreateMatrix(
            new float[,]
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 9}
            }
        );
        var multipled = GenTensor<float, FloatWrapper>.MatrixMultiply(myMatrix, myMatrix);
        Console.WriteLine(GenTensor<float, FloatWrapper>.MatrixMultiply(myMatrix, multipled));
        var t = GenTensor<float, FloatWrapper>.Stack(myMatrix, myMatrix);
        t.Transpose(0, 2);
        Console.WriteLine(t);
    }

    public static GenTensor<T, GenericWrapper<T>> LazyWrapperMultiply<T>(T[,] a, T[,] b)
    {
        return GenTensor<T, GenericWrapper<T>>.MatrixMultiply(
            GenTensor<T, GenericWrapper<T>>.CreateMatrix(a),
            GenTensor<T, GenericWrapper<T>>.CreateMatrix(b)
        );
    }
}
