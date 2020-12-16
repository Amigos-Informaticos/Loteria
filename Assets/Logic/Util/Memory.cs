using System;
using System.Collections.Generic;

public static class Memory
{
    public static Dictionary<string,Object> memory = new Dictionary<string, object>();

    public static void Save(string key, Object objecToSave)
    {
        if (memory.ContainsKey(key))
        {
            memory.Remove(key);
        }
        memory.Add(key, objecToSave);
    }

    public static Object Load(string key)
    {
        Object objectToLoad = null;
        if (memory.ContainsKey(key))
        {
            objectToLoad = memory[key];
        }
        return objectToLoad;
    }
}