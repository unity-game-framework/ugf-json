using System.IO;
using UGF.Jsonc.Runtime;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;

namespace UGF.Jsonc.Editor
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
        [SerializeField] private bool m_compact = true;
        [SerializeField] private bool m_comments;

        /// <summary>
        /// Gets or sets the value that determines whether to format source text into compact layout to store in imported runtime asset.
        /// </summary>
        public bool Compact { get { return m_compact; } set { m_compact = value; } }

        /// <summary>
        /// Gets or sets the value that determines whether to clear comments from the source text to store in imported runtime asset.
        /// </summary>
        public bool Comments { get { return m_comments; } set { m_comments = value; } }

        public override void OnImportAsset(AssetImportContext context)
        {
            string text = File.ReadAllText(context.assetPath);

            if (m_compact)
            {
                text = JsonFormatUtility.ToCompact(text);
            }

            if (!m_comments)
            {
                text = JsonFormatUtility.ClearComments(text);
            }

            var textAsset = new TextAsset(text);

            context.AddObjectToAsset("main", textAsset);
            context.SetMainObject(textAsset);
        }
    }
}
