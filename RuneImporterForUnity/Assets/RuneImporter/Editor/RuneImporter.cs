using System;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEditor;
using UnityEditor.AssetImporters;

// ScriptableObjectDirectoryがconst null設定の為、nullチェックを行っている箇所で警告が発生する
// この変数はユーザーが再設定する事を意図している為、警告を無視する
#pragma warning disable 0162

namespace RuneImporter
{
    public class RuneImporter
    {
        // インポーターのバージョン
        // Tool側と一致していないとバージョンが違うということになる
        //
        // @note
        // Version違いによる問題が出るかは今後の方針次第
        public const string ImporterVersion = "1.02";

        [Serializable]
        struct RuneJson
        {
            public RuneInfo Info;
            public RuneBook Book;
        }

        [Serializable]
        struct RuneInfo
        {
            public string ToolVersion;
        }

        [Serializable]
        struct RuneBook
        {
            public string Name;
            public RuneSheet[] Sheets;
        }

        [Serializable]
        struct RuneSheet
        {
            public string Name;
            public RuneTable[] Tables;
        }

        [Serializable]
        struct RuneTable
        {
            public string Name;
            public RuneType[] Types;
            public RuneValue[] Values;
        }

        [Serializable]
        struct RuneValue
        {
            public string[] Values;
        }

        [Serializable]
        struct RuneType
        {
            public RuneName TypeName;
        }

        [Serializable]
        struct RuneName
        {
            public string Kind;
            public string Value;
        }

        const string ClassPrefix = "Rune.";
        const string ValueListName = "ValueList";

        [MenuItem("Tools/RuneImporter/Import Runes")]
        public static void ImportRunes()
        {
            var files = Directory.EnumerateFiles(Config.RuneDirectory, "*.rune", SearchOption.TopDirectoryOnly);
            foreach(var file_path in files)
            {
                using(var stream = new StreamReader(file_path))
                {
                    var json = stream.ReadToEnd();
                    var data = JsonUtility.FromJson<RuneJson>(json);

                    createInstanceAndSetting(data.Book, file_path);
                    AssetDatabase.Refresh();
                }
            }
            /*
            using (var stream = new StreamReader(ctx.assetPath))
            {
                var json = stream.ReadToEnd();
                var data = JsonUtility.FromJson<RuneJson>(json);

                createInstanceAndSetting(data.Book, ctx.assetPath);
                AssetDatabase.Refresh();
            }
            */
        }

        static void createInstanceAndSetting(RuneBook book, string src_path)
        {
            Array.ForEach(book.Sheets, (s) => createInstanceAndSetting(book, s, src_path));
        }

        static void createInstanceAndSetting(RuneBook book, RuneSheet sheet, string src_path)
        {
            Array.ForEach(sheet.Tables, (t) => createInstanceAndSetting(book, t, src_path));
        }

        static void createInstanceAndSetting(RuneBook book, RuneTable table, string src_path)
        {
            var instance = createInstance(book, table);
            if (instance != null)
            {
                var instance_type_name = instance.GetType().FullName;

                var value_type_name = instance_type_name + $"+Value, {Config.AssemblyName}";
                var value_type = Type.GetType(value_type_name);
                Assert.IsNotNull(value_type, $"type={value_type_name}");

                var value_list_type_name = value_type_name + "[]";
                var value_list_type = Type.GetType(value_list_type_name);
                var value_list_info = instance.GetType().GetField(ValueListName);
                var value_list_instance = (Array)value_list_info.GetValue(instance);

                for (int i = 0; i < table.Values.Length; ++i)
                {
                    var col_value_array = table.Values[i];
                    var value_instance = Activator.CreateInstance(value_type);
                    for (int j = 0; j < col_value_array.Values.Length; ++j)
                    {
                        var type = table.Types[j];
                        if (type.TypeName.Kind == "enum")
                        {
                            continue;
                        }

                        var value_string = col_value_array.Values[j];
                        var value_object = nameToObjectValue(type, value_string);
                        var value_name = type.TypeName.Value;
                        var value_field = value_type.GetField(value_name);

                        value_field.SetValue(value_instance, value_object);
                    }
                    value_list_instance.SetValue(value_instance, i);
                }

                if (Config.ScriptableObjectDirectory != null)
                {
                    Directory.CreateDirectory(Config.ScriptableObjectDirectory);
                    AssetDatabase.CreateAsset(instance, Config.ScriptableObjectDirectory + book.Name + "_" + table.Name + ".asset");
                }
                else
                {
                    var dst_path = Path.GetDirectoryName(src_path) + "/" + book.Name + "_" + table.Name + ".asset";
                    AssetDatabase.CreateAsset(instance, dst_path);
                }
            }
        }

        static RuneScriptableObject createInstance(RuneBook book, RuneTable table)
        {
            var class_name = makeClassName(book, table);
            var type = Type.GetType(class_name);
            var instance = ScriptableObject.CreateInstance(type) as RuneScriptableObject;

            return instance;
        }

        static string makeClassName(RuneBook book, RuneTable table)
        {
            return ClassPrefix + book.Name + "_" + table.Name + ", " + Config.AssemblyName;
        }

        static object nameToObjectValue(RuneType type, string value_name)
        {
            switch (type.TypeName.Kind)
            {
                case "int":
                    return parseIntValue(value_name);
                case "int2":
                    return parseInt2Value(value_name);
                case "int3":
                    return parseInt3Value(value_name);
                case "float":
                    return parseFloatValue(value_name);
                case "float2":
                    return parseVector2Value(value_name);
                case "float3":
                    return parseVector3Value(value_name);
                case "float4":
                    return parseVector4Value(value_name);
                case "string":
                    return value_name;
            }

            return null;
        }

        static int parseIntValue(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }

            int result = 0;
            if (int.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                Debug.LogError($"値が整数ではありません:{str}");
                return 0;
            }
        }

        static int[] parseIntArrayValue(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new int[0];
            }

            var values = str.Split(',');
            var result = new int[values.Length];
            for (int i = 0; i < values.Length; ++i)
            {
                result[i] = parseIntValue(values[i]);
            }

            return result;
        }

        static Int2 parseInt2Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return Int2.zero;
            }

            var values = parseIntArrayValue(str);
            if (values.Length != 2)
            {
                Debug.LogError($"値がInt2ではありません:{str}");
                return Int2.zero;
            }

            return new Int2(values[0], values[1]);
        }

        static Int3 parseInt3Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return Int3.zero;
            }

            var values = parseIntArrayValue(str);
            if (values.Length != 3)
            {
                Debug.LogError($"値がInt3ではありません:{str}");
                return Int3.zero;
            }

            return new Int3(values[0], values[1], values[2]);
        }

        static float parseFloatValue(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return 0f;
            }

            float result = 0f;
            if (float.TryParse(str, out result))
            {
                return result;
            }
            else
            {
                Debug.LogError($"値が浮動小数ではありません:{str}");
                return 0f;
            }
        }

        static Vector2 parseVector2Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return Vector2.zero;
            }

            var values = parseFloatArrayValue(str);
            if (values.Length != 2)
            {
                Debug.LogError($"値がVector2ではありません:{str}");
                return Vector2.zero;
            }

            return new Vector2(values[0], values[1]);
        }

        static Vector3 parseVector3Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return Vector3.zero;
            }

            var values = parseFloatArrayValue(str);
            if (values.Length != 3)
            {
                Debug.LogError($"値がVector3ではありません:{str}");
                return Vector3.zero;
            }

            return new Vector3(values[0], values[1], values[2]);
        }

        static Vector4 parseVector4Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return Vector4.zero;
            }

            var values = parseFloatArrayValue(str);
            if (values.Length != 4)
            {
                Debug.LogError($"値がVector4ではありません:{str}");
                return Vector4.zero;
            }

            return new Vector4(values[0], values[1], values[2], values[3]);
        }

        static float[] parseFloatArrayValue(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return new float[0];
            }

            var values = str.Split(',');
            var result = new float[values.Length];
            for (int i = 0; i < values.Length; ++i)
            {
                result[i] = parseFloatValue(values[i]);
            }
            return result;
        }
    }
}
