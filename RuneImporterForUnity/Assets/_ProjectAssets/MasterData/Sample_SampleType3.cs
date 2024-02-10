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
        public static AsyncOperationHandle Sample_SampleType3_LoadInstanceAsync()
        {
            return Rune.Sample_SampleType3.LoadInstanceAsync();
        }
    }
}

namespace Rune
{

public class Sample_SampleType3 : RuneScriptableObject
{
    public static Sample_SampleType3 instance { get; private set; }

    [SerializeField]
    public Value[] ValueList = new Value[2];

    [Serializable]
    public struct Value
    {
        public string name;
    }

    public static AsyncOperationHandle LoadInstanceAsync() {
        Assert.IsFalse(string.IsNullOrEmpty(Config.ScriptableObjectDirectory), "Config.ScriptableObjectDirectoryにAddressableディレクトリパスを設定してください");
        
        var out_dir = Config.ScriptableObjectDirectory;
        var asset_name = "Sample_SampleType3.asset";
        var path = out_dir + asset_name;
        var handle = Config.OnLoad(path);
        handle.Completed += (handle) => { instance = handle.Result as Sample_SampleType3; };

        return handle;
    }
}

}
