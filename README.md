![Test](https://github.com/WhiteBlackGoose/GenericTensor/workflows/Test/badge.svg)
![GitHub](https://img.shields.io/github/license/WhiteBlackGoose/GenericTensor?color=blue)

# GenericTensor

It is the only fully-implemented generic-tensor library for C#. Allows to work with tensors of custom types.
Is still under development, as well as documentation.

Tensor - is an extentension of matrices, a N-D dimensional array. Soon you will find all common functions that are
defined for matrices and vectors here. In order to make it custom, Tensor class is generic, which means that
you could use not only built-in types like int, float, etc., but also your own types.

#### Hello world

For full correct work you will need some methods for your type to be defined. But first, we could
create our first int tensor:

```cs
var myTensor = new GenTensor<int>(3, 4, 5);
```

Alright, your first tensor is created. You can now access its members:
```cs
myTensor[2, 0, 3] = 5;
Console.WriteLine(myTensor[1, 1, 1]);
```

However, if you try to do anything with it,
for example, add it to itself:

```cs
var b = GenTensor<int>.PiecewiseAdd(myTensor, myTensor);
```

it will require you to first define static method Add:

```cs
ConstantsAndFunctions<int>.Add = (a, b) => a + b;
```

There is how BuiltinTypeInitter.InitForInt() is implemented (call it to make Tensor<int> work):

```cs
public static void InitForInt()
{
    ConstantsAndFunctions<int>.Add = (a, b) => a + b;
    ConstantsAndFunctions<int>.Subtract = (a, b) => a - b;
    ConstantsAndFunctions<int>.Multiply = (a, b) => a * b;
    ConstantsAndFunctions<int>.Divide = (a, b) => a / b;
    ConstantsAndFunctions<int>.CreateZero = () => 0;
    ConstantsAndFunctions<int>.CreateOne = () => 1;
    ConstantsAndFunctions<int>.AreEqual = (a, b) => a == b;
    ConstantsAndFunctions<int>.Negate = a => -a;
    ConstantsAndFunctions<int>.IsZero = a => a == 0;
    ConstantsAndFunctions<int>.Copy = a => a;
}
```

Just call it in the beginning of your program. You can define these methods for any type
including your own.

More methods' documentation is coming soon
