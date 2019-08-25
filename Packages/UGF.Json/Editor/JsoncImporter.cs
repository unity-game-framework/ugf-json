using System.IO;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UGF.Json.Editor
{
    /// <summary>
    /// Represents importer to support '.jsonc' (Json with Comments) extension.
    /// <para>
    /// The source content will be imported as TextAsset.
    /// </para>
    /// </summary>
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
