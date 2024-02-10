using System;
using System.Diagnostics;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace RuneImporter
{
    public static class Config
    {
        // ScriptableObjectアセットの出力先ディレクトリ
        // 環境により書き換えてください
        //
        // @note 未設定の場合runeファイルと同じディレクトリに出力します
        public const string ScriptableObjectDirectory = "Assets/_ProjectAssets/MasterData/";

        public const string RuneDirectory = "Assets/_ProjectAssets/MasterData/";

        // ScriptableObjectのクラスが属するアセンブリ名
        public const string AssemblyName = "Assembly-CSharp";

        // ScriptableObjectアセットのロード方法
        // デフォルトではAddressableを使用します。プロジェクト方針により書き換えてください
        //
        // @note 現在はAsyncOperationHandleを使用する為、Addressableへの依存があります
        public static Func<string, AsyncOperationHandle> OnLoad = (path) =>
        {
            return Addressables.LoadAssetAsync<RuneScriptableObject>(path);
        };
    }
}
