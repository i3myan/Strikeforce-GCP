using System;
using System.Text.Json;

namespace OCDETF.StrikeForce.Core.Library
{
    public static class CloneUtility<T>
    {
        public static T Clone(T obj)
        {
            string strJson = JsonSerializer.Serialize(obj);
            return JsonSerializer.Deserialize<T>(strJson);
        }
    }
}
