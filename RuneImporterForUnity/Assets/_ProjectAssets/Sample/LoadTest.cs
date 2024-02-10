using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class LoadTest : MonoBehaviour
{
    void Awake()
    {
        var handle = Addressables.InitializeAsync();
        handle.Completed += (handle) =>
        {
            LoadAll();
        };

        // テーブル型からロード
        //LoadFromType();

        // ロード関数から一括でロード
    }

    void LoadFromType()
    {
        var handle = Rune.Sample_SampleType.LoadInstanceAsync();
        handle.Completed += (handle) =>
        {
            var result = handle.Result as Rune.Sample_SampleType;
            foreach (var v in result.ValueList)
            {
                Debug.Log(v.name);
                Debug.Log(v.number);
                Debug.Log(v.position);
            }
        };
    }

    async void LoadAll()
    {
        await RuneImporter.RuneLoader.LoadAllAsync();

        Debug.Log(Rune.Sample_SampleType.instance.name);
        var sample_data = Rune.Sample_SampleType.instance.ValueList;
        foreach (var v in sample_data)
        {
            Debug.Log(v.name);
        }

        Debug.Log(Rune.Sample_SampleType2.instance.name);
        var sample_data2 = Rune.Sample_SampleType2.instance.ValueList;
        foreach (var v in sample_data2)
        {
            Debug.Log(v.name);
        }
    }
}
