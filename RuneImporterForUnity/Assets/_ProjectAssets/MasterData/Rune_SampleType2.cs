using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using RuneImporter;

namespace RuneImporter
{
    public static partial class RuneLoader
    {
        public static AsyncOperationHandle Rune_SampleType2_LoadInstanceAsync()
        {
            return Rune_SampleType2.LoadInstanceAsync();
        }
    }
}

public class Rune_SampleType2 : RuneScriptableObject
{
    public static Rune_SampleType2 instance { get; private set; }

    [SerializeField]
    public Value[] ValueList = new Value[3];

    [Serializable]
    public struct Value
    {
        public string name;
        public Vector3 position3;
        public Vector4 position4;
        public Vector2 position2;
    }

    public static AsyncOperationHandle LoadInstanceAsync() {
        var src_dir = "sample_data/";
        var out_dir = string.IsNullOrEmpty(Config.ScriptableObjectDirectory) ? src_dir : Config.ScriptableObjectDirectory;
        var asset_name = "SampleType2.asset";
        var path = out_dir + asset_name;
        var handle = Config.OnLoad(path);
        handle.Completed += (handle) => { instance = handle.Result as Rune_SampleType2; };

        return handle;
    }
}
