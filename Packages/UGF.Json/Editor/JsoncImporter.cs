using System.IO;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UGF.Json.Editor
{
    [ScriptedImporter(0, "jsonc")]
    public class JsoncImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext context)
        {
            string text = File.ReadAllText(context.assetPath);
            var textAsset = new TextAsset(text);

            context.AddObjectToAsset("main", textAsset);
            context.SetMainObject(textAsset);
        }
    }
}
