using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using RuneImporter;

namespace RuneImporter
{
    public static partial class RuneLoader
    {
        public static AsyncOperationHandle Sample_SampleType_LoadInstanceAsync()
        {
            return Rune.Sample_SampleType.LoadInstanceAsync();
        }
    }
}

namespace Rune
{

public class Sample_SampleType : RuneScriptableObject
{
    public static Sample_SampleType instance { get; private set; }

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
        Assert.IsFalse(string.IsNullOrEmpty(Config.ScriptableObjectDirectory), "Config.ScriptableObjectDirectoryにAddressableディレクトリパスを設定してください");
        
        var out_dir = Config.ScriptableObjectDirectory;
        var asset_name = "Rune_Sample_SampleType.asset";
        var path = out_dir + asset_name;
        var handle = Config.OnLoad(path);
        handle.Completed += (handle) => { instance = handle.Result as Sample_SampleType; };

        return handle;
    }
}

}
