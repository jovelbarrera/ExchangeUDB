using System;
using System.Reflection;
namespace Kadevjo.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class |
        AttributeTargets.Struct)]
    /// <summary>
    /// Service cache attribute. Check service if need save data in cache 
    /// </summary>
    public class CacheableAttribute : Attribute
    {


    }
}