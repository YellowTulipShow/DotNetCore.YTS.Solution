using System;
using System.Linq;
using System.Collections.Generic;
using YTS.Test;
using YTS.Tools;

/// <summary>
/// 验证静态类
/// </summary>
public static class Assert
{
    #region 执行测试方法, 输出异常
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<R>(this Func<R> func, params R[] answer)
    {
        TestExe(func, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<R>(this Func<R> func, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func();
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, R>(this Func<T1, R> func, T1 arg1, params R[] answer)
    {
        TestExe(func, arg1, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, R>(this Func<T1, R> func, T1 arg1, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, R>(this Func<T1, T2, R> func, T1 arg1, T2 arg2, params R[] answer)
    {
        TestExe(func, arg1, arg2, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, R>(this Func<T1, T2, R> func, T1 arg1, T2 arg2, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, R>(this Func<T1, T2, T3, R> func, T1 arg1, T2 arg2, T3 arg3, params R[] answer)
    {
        TestExe(func, arg1, arg2, arg3, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, R>(this Func<T1, T2, T3, R> func, T1 arg1, T2 arg2, T3 arg3, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2, arg3);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2, arg3);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, params R[] answer)
    {
        TestExe(func, arg1, arg2, arg3, arg4, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, R>(this Func<T1, T2, T3, T4, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2, arg3, arg4);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2, arg3, arg4);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, R>(this Func<T1, T2, T3, T4, T5, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, params R[] answer)
    {
        TestExe(func, arg1, arg2, arg3, arg4, arg5, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, R>(this Func<T1, T2, T3, T4, T5, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2, arg3, arg4, arg5);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2, arg3, arg4, arg5);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, R>(this Func<T1, T2, T3, T4, T5, T6, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, params R[] answer)
    {
        TestExe(func, arg1, arg2, arg3, arg4, arg5, arg6, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, R>(this Func<T1, T2, T3, T4, T5, T6, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2, arg3, arg4, arg5, arg6);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2, arg3, arg4, arg5, arg6);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, R>(this Func<T1, T2, T3, T4, T5, T6, T7, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, params R[] answer)
    {
        TestExe(func, arg1, arg2, arg3, arg4, arg5, arg6, arg7, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, R>(this Func<T1, T2, T3, T4, T5, T6, T7, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, params R[] answer)
    {
        TestExe(func, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, params R[] answer)
    {
        TestExe(func, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, params R[] answer)
    {
        TestExe(func, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, params R[] answer)
    {
        TestExe(func, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, params R[] answer)
    {
        TestExe(func, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, params R[] answer)
    {
        TestExe(func, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, params R[] answer)
    {
        TestExe(func, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, params R[] answer)
    {
        TestExe(func, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        }
    }


    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, params R[] answer)
    {
        TestExe(func, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16, TestEqualsJSON, answer);
    }
    /// <summary>
    /// 执行测试方法
    /// </summary>
    public static void TestExe<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R>(this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, R> func, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.Method, answers, result, arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15, arg16);
        }
    }
    #endregion

    #region 数据相等判断
    /// <summary>
    /// 测试相等方法
    /// </summary>
    public static bool TestEquals<T1, T2>(this T1 t1, T2 t2, Func<T1, T2, bool> isTestEquals)
    {
        return isTestEquals(t1, t2);
    }

    /// <summary>
    /// 测试相等方法
    /// </summary>
    public static bool TestEqualsJSON<T>(this T t1, T t2)
    {
        return t1.ToJSONString() == t2.ToJSONString();
    }

    /// <summary>
    /// 测试相等方法
    /// </summary>
    public static bool TestEquals(this object t1, object t2)
    {
        return TestEquals(t1, t2, (v1, v2) => v1.ToJSONString() == v2.ToJSONString());
    }

    /// <summary>
    /// 测试相等方法
    /// </summary>
    public static bool TestEquals<S>(this S t1, S t2) where S : struct
    {
        return TestEquals(t1, t2, (v1, v2) => v1.Equals(v2));
    }

    /// <summary>
    /// 测试相等方法
    /// </summary>
    public static bool TestEquals<S>(this S t1, params S[] t2s) where S : struct
    {
        return t2s.Contains(t1);
    }

    /// <summary>
    /// 测试相等方法
    /// </summary>
    public static bool TestEquals<T>(this IList<T> t1, IList<T> t2)
    {
        return TestEquals(t1, t2, (v1, v2) => v1.ToJSONString() == v2.ToJSONString());
    }

    /// <summary>
    /// 测试相等方法
    /// </summary>
    public static bool TestEquals<T>(this IEnumerable<T> t1, IEnumerable<T> t2)
    {
        return TestEquals(t1, t2, (v1, v2) => v1.ToJSONString() == v2.ToJSONString());
    }

    /// <summary>
    /// 测试相等方法
    /// </summary>
    public static bool TestEquals<T>(this IEnumerator<T> t1, IEnumerator<T> t2)
    {
        return TestEquals(t1, t2, (v1, v2) => v1.ToJSONString() == v2.ToJSONString());
    }
    #endregion
}
