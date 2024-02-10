# RuneImporterForUnity
XlsToRuneで出力したrune(json)ファイルをUnity上でパラメータとして扱えるようにする<br>
インポータを作成するリポジトリです
- jsonファイルからScriptableObjectを作成
- 各型別のロード関数、一括ロード関数を自動生成
- 各パラメータテーブルにはロード後、静的にアクセスが可能

## 使用方法
Releases ページのunityパッケージを使用してください
または、リポジトリのAssets/RuneImporterフォルダをコピーして使用します
RuneImporterの動作に必要なファイルはこのフォルダ以下にまとまっています

## アンインストール
Assets/RuneImporterフォルダを削除してください

## 設定調整
Assets/RuneImporter/Config.cs で
- ScriptableObjectの出力フォルダ
- アセットLoad関数の上書き
が可能です

## 依存関係
Addressableパッケージに依存します

## ChangeLog

### v1.02
- XlsToRuneがint2,3 float2,3,4に対応したので、対応
- Config.ScriptableObjectDirectoryが未設定の場合はruneファイルと
  同じディレクトリに出力するように
- インポータ側にバージョン表記を忘れていたので追加
