![Test](https://github.com/WhiteBlackGoose/GenericTensor/workflows/Test/badge.svg)
![GitHub](https://img.shields.io/github/license/WhiteBlackGoose/GenericTensor?color=blue)

# GenericTensor

It is the only fully-implemented generic-tensor library for C#. Allows to work with tensors of custom types.
Is still under development, as well as documentation.

Tensor - is an extentension of matrices, a N-D dimensional array. Soon you will find all common functions that are
defined for matrices and vectors here. In order to make it custom, Tensor class is generic, which means that
you could use not only built-in types like int, float, etc., but also your own types.

### Hello world

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
    ConstantsAndFunctions<int>.Forward = a => a;
}
```

Forward is an essential function. It allows you to only copy a wrapper while
keeping pointer to the same inner data. For example, you have a class
```cs
class Wrapper
{
    private MyData innerData;

    public Wrapper(MyData data) { innerData = data; }
}
```

Then it might look like

```cs
ConstantsAndFunctions<Wrapper>.Copy = a => new Wrapper(innerData.Copy());
ConstantsAndFunctions<Wrapper>.Forward = a => new Wrapper(innerData());
```


### Functionality

Here we list and explain all methods from GT. We use O() syntax to show
asymptotics of an algorithm, where N is a side of a tensor, V is its volume.

#### Composition (structure)

<details><summary><strong>GetSubtensor</strong></summary><p>
```cs
public GenTensor<T> GetSubtensor(params int[] indecies)
```

Allows to get a subtensor with SHARED data (so that any changes to
intial tensor or the subtensor will be reflected in both).

For example, Subtensor of a matrix is a vector (row).

Works for O(1)
</p></details>

<details><summary><strong>SetSubtensor</strong></summary><p>
```cs
public void SetSubtensor(GenTensor<T> sub, params int[] indecies);
```

Allows to set a subtensor by forwarding all elements from sub to this. Override
ConstantsAndFunctions<T>.Forward to enable it.

Works for O(V)
</p></details>

<details><summary><strong>Transposition</strong></summary><p>
```cs
public void Transpose(int axis1, int axis2);
public void TransposeMatrix();
```

Swaps axis1 and axis2 in this.
TransposeMatrix swaps the last two axes.

Works for O(1)
</p></details>

<details><summary><strong>Concatenation</strong></summary><p>
```cs
public static GenTensor<T> Concat(GenTensor<T> a, GenTensor<T> b);
```

Conatenates two tensors by their first axis. For example, concatenation of
two tensors of shape [4 x 5 x 6] and [7 x 5 x 6] is a tensor of shape
[11 x 5 x 6]. 

Works for O(N)
</p></details>

<details><summary><strong>Stacking</strong></summary><p>
```cs
public static GenTensor<T> Stack(params GenTensor<T>[] elements);
```

Unites all same-shape elements into one tensor with 1 dimension more.
For example, if t1, t2, and t3 are of shape [2 x 5], Stack(t1, t2, t3) will
return a tensor of shape [3 x 2 x 5]

Works for O(V)
</p></details>

<details><summary><strong>Slicing</strong></summary><p>
```cs
public GenTensor<T> Slice(int leftIncluding, int rightExcluding);
```

Slices this into another tensor with data-sharing. Syntax and use is similar to
python's numpy:

```py
v = myTensor[2:3]
```

is the same as

```cs
var v = myTensor.Slice(2, 3);
```

Works for O(N)
</p></details>