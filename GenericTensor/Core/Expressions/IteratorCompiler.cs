﻿#region copyright
/*
 * MIT License
 * 
 * Copyright (c) 2020-2021 WhiteBlackGoose
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
#endregion


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GenericTensor.Core.Expressions
{
    internal static class ExpressionCompiler<T, TWrapper> where TWrapper : struct, IOperations<T>
    {
#pragma warning disable CA2211 // Non-constant fields should not be visible
        public static Func<Expression, Expression, Expression> Addition
            = (a, b) =>  Expression.Call(
                Expression.Default(typeof(TWrapper)),
                typeof(IOperations<T>).GetMethod(nameof(IOperations<T>.Add)),
                a, b
            );

        public static Func<Expression, Expression, Expression> Subtraction
            = (a, b) =>  Expression.Call(
                Expression.Default(typeof(TWrapper)),
                typeof(IOperations<T>).GetMethod(nameof(IOperations<T>.Subtract)),
                a, b
            );

        public static Func<Expression, Expression, Expression> Multiplication
            = (a, b) =>  Expression.Call(
                Expression.Default(typeof(TWrapper)),
                typeof(IOperations<T>).GetMethod(nameof(IOperations<T>.Multiply)),
                a, b
            );

        public static Func<Expression, Expression, Expression> Division
            = (a, b) =>  Expression.Call(
                Expression.Default(typeof(TWrapper)),
                typeof(IOperations<T>).GetMethod(nameof(IOperations<T>.Divide)),
                a, b
            );
#pragma warning restore CA2211 // Non-constant fields should not be visible
        private static Expression CreateLoop(ParameterExpression var, Expression until, Expression onIter)
        {
            var label = Expression.Label();

            var loop = Expression.Loop(
                Expression.Block(
                    Expression.IfThenElse(

                    Expression.LessThan(var, until),

                    Expression.Block(
                        onIter
                    ),

                    Expression.Break(label)
                ),
                Expression.PostIncrementAssign(var)
                    ),
                label);

            
            return Expression.Block(
                Expression.Assign(var, Expression.Constant(0)),
                loop
                );
        }

        private static Expression CompileNestedLoops(Expression[] shapes, Func<ParameterExpression[], Expression> onIter) 
        {
            var acts = new List<Expression>();

            var locals = new ParameterExpression[shapes.Length];
            for (int i = 0; i < shapes.Length; i++)
                locals[i] = Expression.Parameter(typeof(int), "x_" + i);
            
            var localShapes = new ParameterExpression[shapes.Length];
            for (int i = 0; i < shapes.Length; i++)
                localShapes[i] = Expression.Parameter(typeof(int), "shape_" + i);
            var localShapesAssigned = new Expression[shapes.Length];
            for (int i = 0; i < shapes.Length; i++)
                localShapesAssigned[i] = Expression.Assign(localShapes[i], shapes[i]);

            Expression currExpr = onIter(locals);

            for (int i = shapes.Length - 1; i >= 0; i--) {
                currExpr = CreateLoop(locals[i], localShapes[i], currExpr);
            }

            acts.AddRange(localShapesAssigned);
            acts.Add(currExpr);

            var localVariables = new List<ParameterExpression>();
            localVariables.AddRange(locals);
            localVariables.AddRange(localShapes);

            return Expression.Block(localVariables, Expression.Block(acts));
        }

        private static Expression CompileNestedLoops(Expression[] shapes, Func<ParameterExpression[], Expression> onIter,
            bool parallel, ParameterExpression[] localsToPass)
        {
            if (!parallel)
                return CompileNestedLoops(shapes, onIter);
            else
            {
                var x = Expression.Parameter(typeof(int), "outerLoopVar");

                Expression newOnIter(ParameterExpression[] exprs)
                {
                    var arr = new ParameterExpression[exprs.Length + 1];
                    arr[0] = x;
                    for (int i = 0; i < exprs.Length; i++)
                        arr[i + 1] = exprs[i];
                    return onIter(arr);
                }

                var shape0 = shapes[0];
                var others = new ArraySegment<Expression>(shapes, 1, shapes.Length - 1).ToArray();
                var compiledLoops = CompileNestedLoops(others, newOnIter);

                var newArgs = localsToPass.ToList().Append(x).ToArray();

                var del = Expression.Lambda(compiledLoops, newArgs);

                var actions = new List<Expression>();

                var localDelCall = Expression.Invoke(del, newArgs);

                var finalLambda = Expression.Lambda(
                    localDelCall,
                    x
                );

                var enu = typeof(Parallel)
                    .GetMethods()
                    .Where(mi => mi.Name == nameof(Parallel.For))
                    .Where(mi => mi.GetParameters().Length == 3)
                    .Where(mi => mi.GetParameters()[2].ParameterType == typeof(Action<int>)).ToArray();
                if (enu.Length != 1)
                    throw new InvalidEnumArgumentException();

                var mi = enu.FirstOrDefault();
                
                var call = Expression.Call(null, mi, Expression.Constant(0), shape0, finalLambda);
                return call;
            }
        }

        private static Expression BuildIndexToData(ParameterExpression[] vars, ParameterExpression[] blocks)
        {
            if (vars.Length != blocks.Length)
                throw new InvalidShapeException();
            Expression res = Expression.Multiply(vars[0], blocks[0]);
            for (int i = 1; i < blocks.Length; i++)
                res = Expression.Add(Expression.Multiply(vars[i], blocks[i]), res);
            return res;
        }

        private static (ParameterExpression[] locals, Expression[] assignes) CompileLocalBlocks(ParameterExpression arr, int N, string pref)
        {
            var blocks = new ParameterExpression[N];
            for (int i = 0; i < N; i++)
                blocks[i] = Expression.Parameter(typeof(int), pref + "blocks_" + i);
            var assignes = new Expression[N];
            for (int i = 0; i < N; i++)
            {
                assignes[i] = Expression.Assign(blocks[i],
                    Expression.ArrayIndex(
                        Expression.Field(arr, typeof(GenTensor<T, TWrapper>).GetField(nameof(GenTensor<T, TWrapper>.blocks)))
                        ,
                        Expression.Constant(i)
                    )
                );
            }
            return (blocks, assignes);
        }

        private static Action<GenTensor<T, TWrapper>, GenTensor<T, TWrapper>, GenTensor<T, TWrapper>> CompileForNDimensions(int N, Func<Expression, Expression, Expression> operation, bool parallel)
        {
            var a = Expression.Parameter(typeof(GenTensor<T, TWrapper>), "a");
            var b = Expression.Parameter(typeof(GenTensor<T, TWrapper>), "b");
            var res = Expression.Parameter(typeof(GenTensor<T, TWrapper>), "res");

            var aData = Expression.Parameter(typeof(T[]), "aData");
            var bData = Expression.Parameter(typeof(T[]), "bData");
            var resData = Expression.Parameter(typeof(T[]), "resData");

            var actions = new List<Expression>();

            var (local_aBlocks, actABlocks) = CompileLocalBlocks(a, N, "a");

            // ablocks_0 = a.blocks[0]
            // ablocks_1 = a.blocks[1]
            // ...
            actions.AddRange(actABlocks);

            var (local_bBlocks, actBBlocks) = CompileLocalBlocks(b, N, "b");

            // bblocks_0 = b.blocks[0]
            // bblocks_1 = b.blocks[1]
            // ...
            actions.AddRange(actBBlocks);
            var (local_resBlocks, actResBlocks) = CompileLocalBlocks(res, N, "res");

            // resblocks_0 = res.blocks[0]
            // resblocks_1 = res.blocks[1]
            // ...
            actions.AddRange(actResBlocks);

            var fieldInfo = typeof(GenTensor<T, TWrapper>).GetField(nameof(GenTensor<T, TWrapper>.data));

            // aData = a.data
            actions.Add(Expression.Assign(aData, Expression.Field(a, fieldInfo)));

            // bData = b.data
            actions.Add(Expression.Assign(bData, Expression.Field(b, fieldInfo)));

            // resData = res.data
            actions.Add(Expression.Assign(resData, Expression.Field(res, fieldInfo)));

            var local_ALinOffset = Expression.Parameter(typeof(int), "aLin");

            // aLin = a.LinOffset
            actions.Add(Expression.Assign(local_ALinOffset, 
                Expression.Field(a, typeof(GenTensor<T, TWrapper>).GetField(nameof(GenTensor<T, TWrapper>.LinOffset)))));

            var local_BLinOffset = Expression.Parameter(typeof(int), "bLin");

            // bLin = b.LinOffset
            actions.Add(Expression.Assign(local_BLinOffset, 
                Expression.Field(b, typeof(GenTensor<T, TWrapper>).GetField(nameof(GenTensor<T, TWrapper>.LinOffset)))));

            Expression onIter(ParameterExpression[] vars)
            {
                // ablocks_0 * x_0 + ablocks_1 * x_1 + ...
                var aIndex = ExpressionCompiler<T, TWrapper>.BuildIndexToData(vars, local_aBlocks);

                // bblocks_0 * x_0 + bblocks_1 * x_1 + ...
                var bIndex = ExpressionCompiler<T, TWrapper>.BuildIndexToData(vars, local_bBlocks);

                // + aLinOffset
                aIndex = Expression.Add(aIndex, local_ALinOffset);

                // + bLinOffset
                bIndex = Expression.Add(bIndex, local_BLinOffset);

                // a.data
                var aDataField = aData;

                // d.data
                var bDataField = bData;

                // a.data[aIndex]
                var aDataIndex = Expression.ArrayIndex(aDataField, aIndex);

                // b.data[bIndex]
                var bDataIndex = Expression.ArrayIndex(bDataField, bIndex);

                // a.data[aIndex] + b.data[bIndex]
                var added = operation(aDataIndex, bDataIndex);

                // resblocks_0 * x_0 + ...
                var resIndex = ExpressionCompiler<T, TWrapper>.BuildIndexToData(vars, local_resBlocks);

                // res.data
                var resField = resData;

                // res.data[resIndex] = 
                var accessRes = Expression.ArrayAccess(resField, resIndex);

                var assign = Expression.Assign(accessRes, added);

                return assign;
            }

            var locals = new List<ParameterExpression>();
            locals.AddRange(local_aBlocks);
            locals.AddRange(local_bBlocks);
            locals.AddRange(local_resBlocks);
            locals.Add(local_ALinOffset);
            locals.Add(local_BLinOffset);
            locals.Add(aData);
            locals.Add(bData);
            locals.Add(resData);

            var shapeInfo = typeof(GenTensor<T, TWrapper>).GetProperty(nameof(GenTensor<T, TWrapper>.Shape));
            var shapeFieldInfo = typeof(TensorShape).GetField(nameof(TensorShape.shape));
            var shapeProperty = Expression.Property(res, shapeInfo);
            var shapeField = Expression.Field(shapeProperty, shapeFieldInfo);

            var loops = ExpressionCompiler<T, TWrapper>.CompileNestedLoops(
                Enumerable.Range(0, N).Select(
                    id =>
                        Expression.ArrayIndex(
                            shapeField,
                            Expression.Constant(id))
                ).ToArray(),
                onIter, parallel, locals.ToArray()
            );

            actions.Add(loops);

            Expression bl = Expression.Block(
                locals, actions.ToArray()
                );

            if (bl.CanReduce)
                bl = bl.Reduce();

            return Expression.Lambda<Action<GenTensor<T, TWrapper>, GenTensor<T, TWrapper>, GenTensor<T, TWrapper>>>(bl, a, b, res).Compile();
        }

        public enum OperationType
        {
            Addition,
            Subtraction,
            Multiplication,
            Division
        }

        private static readonly Dictionary<(OperationType opId, int N, bool parallel), Action<GenTensor<T, TWrapper>, GenTensor<T, TWrapper>, GenTensor<T, TWrapper>>> storage 
                 = new Dictionary<(OperationType opId, int N, bool parallel), Action<GenTensor<T, TWrapper>, GenTensor<T, TWrapper>, GenTensor<T, TWrapper>>>();

        private static Action<GenTensor<T, TWrapper>, GenTensor<T, TWrapper>, GenTensor<T, TWrapper>> GetFunc(int N, Func<Expression, Expression, Expression> operation, bool parallel, OperationType ot)
        {
            var key = (ot, N, parallel);
            if (!storage.ContainsKey(key))
                storage[key] = CompileForNDimensions(N, operation, parallel);
            return storage[key];
        }

        public static GenTensor<T, TWrapper> PiecewiseAdd(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b, bool parallel)
        {
            
            #if ALLOW_EXCEPTIONS
            if (a.Shape != b.Shape)
                throw new InvalidShapeException();
            #endif
            if (a.Shape.Length == 0)
                return GenTensor<T, TWrapper>.CreateTensor(a.Shape, _ => default(TWrapper).Add(a.data[a.LinOffset], b.data[b.LinOffset]));
            var res = new GenTensor<T, TWrapper>(a.Shape);
            GetFunc(a.Shape.Length, Addition, parallel, OperationType.Addition)(a, b, res);
            return res;
        }

        public static GenTensor<T, TWrapper> PiecewiseSubtract(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b, bool parallel)
        {
            #if ALLOW_EXCEPTIONS
            if (a.Shape != b.Shape)
                throw new InvalidShapeException();
            #endif
            if (a.Shape.Length == 0)
                return GenTensor<T, TWrapper>.CreateTensor(a.Shape, _ => default(TWrapper).Subtract(a.data[a.LinOffset], b.data[b.LinOffset]));
            var res = new GenTensor<T, TWrapper>(a.Shape);
            GetFunc(a.Shape.Length, Subtraction, parallel, OperationType.Subtraction)(a, b, res);
            return res;
        }

        public static GenTensor<T, TWrapper> PiecewiseMultiply(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b, bool parallel)
        {
            #if ALLOW_EXCEPTIONS
            if (a.Shape != b.Shape)
                throw new InvalidShapeException();
            #endif
            if (a.Shape.Length == 0)
                return GenTensor<T, TWrapper>.CreateTensor(a.Shape, _ => default(TWrapper).Multiply(a.data[a.LinOffset], b.data[b.LinOffset]));
            var res = new GenTensor<T, TWrapper>(a.Shape);
            GetFunc(a.Shape.Length, Multiplication, parallel, OperationType.Multiplication)(a, b, res);
            return res;
        }

        public static GenTensor<T, TWrapper> PiecewiseDivision(GenTensor<T, TWrapper> a, GenTensor<T, TWrapper> b, bool parallel)
        {
            #if ALLOW_EXCEPTIONS
            if (a.Shape != b.Shape)
                throw new InvalidShapeException();
            #endif
            if (a.Shape.Length == 0)
                return GenTensor<T, TWrapper>.CreateTensor(a.Shape, _ => default(TWrapper).Divide(a.data[a.LinOffset], b.data[b.LinOffset]));
            var res = new GenTensor<T, TWrapper>(a.Shape);
            GetFunc(a.Shape.Length, Division, parallel, OperationType.Division)(a, b, res);
            return res;
        }
    }
}
