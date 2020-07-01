using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInfoLogInvoker
{
    /// <summary>
    /// Defines how component invokes InfoLog using it's singleton methods 
    /// </summary>
    void SetInfoLog();
    
    /// <summary>
    /// Defines how component updates InfoLog when it is needed
    /// </summary>
    void UpdateInfoLog();
}

public static class InfoLogInvokerExtensions
{
    /// <summary>
    /// Valid method to set InfoLog
    /// </summary>
    /// <param name="infoLogInvoker"> Use "this" as parameter </param>
    public static void SetInfoLog(this IInfoLogInvoker infoLogInvoker)
    {
        InfoLog.Instance.LastInvoker = infoLogInvoker;
        infoLogInvoker.SetInfoLog();
    }
}