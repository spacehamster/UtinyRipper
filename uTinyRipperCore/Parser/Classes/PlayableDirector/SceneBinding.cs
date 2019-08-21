using System.Collections.Generic;
using uTinyRipper.AssetExporters;
using uTinyRipper.SerializedFiles;
using uTinyRipper.YAML;

namespace uTinyRipper.Classes.PlayableDirectors
{
	public struct SceneBinding : IAssetReadable, IYAMLExportable
	{
		public void Read(AssetReader reader)
		{
			m_key = reader.ReadAsset<PPtr<Object>>();
			m_value = reader.ReadAsset<PPtr<Object>>();
		}

		public YAMLNode ExportYAML(IExportContainer container)
		{
			YAMLMappingNode node = new YAMLMappingNode();
			node.Add(KeyName, Key.ExportYAML(container));
			node.Add(ValueName, Value.ExportYAML(container));
			return node;
		}
		public IEnumerable<Object> FetchDependencies(ISerializedFile file, bool isLog = false)
		{
			yield return Key.FetchDependency(file);
			yield return Key.FetchDependency(file);
		}

		public const string KeyName = "key";
		public const string ValueName = "value";

		public PPtr<Object> Key => m_key;
		public PPtr<Object> Value => m_value;

		private PPtr<Object> m_key;
		private PPtr<Object> m_value;
	}
}
