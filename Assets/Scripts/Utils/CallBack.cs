using System;

/// <summary>
///Class for Delegates
/// </summary>
public delegate void CallBack(); //Delegate without parameter
public delegate void CallBack<T>(T arg);//Delegate with 1 parameter
public delegate void CallBack<T,X>(T arg1, X arg2);//Delegate with 2 parameters
public delegate void CallBack<T, X,Y>(T arg1, X arg2,Y arg3);////Delegate with 3 parameters
public delegate void CallBack<T, X, Y,Z>(T arg1, X arg2, Y arg3, Z arg4);////Delegate with 4 parameters
public delegate void CallBack<T, X, Y, Z,W>(T arg1, X arg2, Y arg3, Z arg4,W arg5);//Delegate with 5 parameters