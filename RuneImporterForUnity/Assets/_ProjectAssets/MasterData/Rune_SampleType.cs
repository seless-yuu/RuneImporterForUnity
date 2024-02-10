using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using RuneImporter;

namespace RuneImporter
{
    public static partial class RuneLoader
    {
        public static AsyncOperationHandle Rune_SampleType_LoadInstanceAsync()
        {
            return Rune_SampleType.LoadInstanceAsync();
        }
    }
}

public class Rune_SampleType : RuneScriptableObject
{
    public static Rune_SampleType instance { get; private set; }

    [SerializeField]
    public Value[] ValueList = new Value[4];

    [Serializable]
    public struct Value
    {
        public string name;
        public int number;
        public Int2 size2;
        public Int3 size3;
        public float position;
    }

    public static AsyncOperationHandle LoadInstanceAsync() {
        var src_dir = "sample_data/";
        var out_dir = string.IsNullOrEmpty(Config.ScriptableObjectDirectory) ? src_dir : Config.ScriptableObjectDirectory;
        var asset_name = "SampleType.asset";
        var path = out_dir + asset_name;
        var handle = Config.OnLoad(path);
        handle.Completed += (handle) => { instance = handle.Result as Rune_SampleType; };

        return handle;
    }
}
