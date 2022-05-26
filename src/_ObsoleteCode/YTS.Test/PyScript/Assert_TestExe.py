

temp = '''
    public static void TestExe<T1, R>(this Func<T1, R> func, T1 arg1, params R[] answer)
    {
        TestExe(func, arg1, TestEqualsJSON, answer);
    }
    public static void TestExe<T1, R>(this Func<T1, R> func, T1 arg1, Func<R, R, bool> equalsMethod, params R[] answers)
    {
        R result = func(arg1);
        if (!answers.Any(a => equalsMethod(a, result)))
        {
            TestFailException.OutputTestFail(func.GetType(), answers, result, arg1);
        }
    }
'''


for i in range(1, 16+1):
    rstr = temp
    rstr = rstr.replace("T1,", ",".join(
        ["T{0}".format(x) for x in range(1, i+1)]) + ",")
    rstr = rstr.replace("T1 arg1,", ",".join(
        ["T{0} arg{0}".format(x) for x in range(1, i+1)]) + ",")
    rstr = rstr.replace(
        ", arg1", "," + ",".join(["arg{0}".format(x) for x in range(1, i+1)]))
    rstr = rstr.replace(
        "(arg1)", "(" + ",".join(["arg{0}".format(x) for x in range(1, i+1)]) + ")")
    print(rstr)
