![Test](https://github.com/WhiteBlackGoose/GenericTensor/workflows/Test/badge.svg)
![GitHub](https://img.shields.io/github/license/WhiteBlackGoose/GenericTensor?color=blue)

# GenericTensor

It is the only fully-implemented generic-tensor library for C#. Allows to work with tensors of custom types.
Is still under development, as well as documentation.

Tensor - is an extentension of matrices, a N-D dimensional array. Soon you will find all common functions that are
defined for matrices and vectors here. In order to make it custom, Tensor class is generic, which means that
you could use not only built-in types like int, float, etc., but also your own types.

#### Hello world

Class's definition:
```cs
public class Tensor<TWrapper, TPrimitive> where TWrapper : ITensorElement<TPrimitive>, new()
```

We need to use a primitive type and a wrapper for it. For example, you can use built-in types:
```cs
var myTensor = new Tensor<TensorIntWrapper, int>(3, 4, 5); // you created a tensor with the shape of 3 x 4 x 5
```

You can now access its members:
```cs
myTensor[2, 0, 3] = 5;
Console.WriteLine(myTensor[1, 1, 1]);
```

More methods' documentation is coming soon

#### Custom type

Use this template:

```cs
public class TensorYourTypeWrapper : ITensorElement<YourType>
{
    private YourType val;
    public YourType GetValue() => val;
    public void SetValue(YourType newValue) => this.val = newValue;

    public override string ToString()
        => val.ToString();

    public ITensorElement<YourType> Copy()
    {
        var res = new TensorIntWrapper();
        res.SetValue(val);
        return res;
    }

    public ITensorElement<YourType> Forward()
    {
        var res = new TensorIntWrapper();
        res.SetValue(val);
        return res;
    }

    public void SetZero()
        => throw new NotImplementedException("This operation needs SetZero to be defined");

    public void SetOne()
        => throw new NotImplementedException("This operation needs SetOne to be defined");

    void ITensorElement<YourType>.Add(ITensorElement<YourType> other)
    {
        => throw new NotImplementedException("This operation needs Add to be defined");
    }

    void ITensorElement<YourType>.Multiply(ITensorElement<YourType> other)
    {
        => throw new NotImplementedException("This operation needs Multiply to be defined");
    }

    void ITensorElement<YourType>.Subtract(ITensorElement<YourType> other)
    {
        => throw new NotImplementedException("This operation needs Subtract to be defined");
    }

    void ITensorElement<YourType>.Divide(ITensorElement<YourType> other)
    {
        => throw new NotImplementedException("This operation needs Divide to be defined");
    }

    void ITensorElement<YourType>.Negate()
    {
        => throw new NotImplementedException("This operation needs Negate to be defined");
    }

}
```

You do not have to implement all these methods, only those that are required by functions you use.