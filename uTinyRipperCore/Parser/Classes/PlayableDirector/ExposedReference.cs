using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uTinyRipper.AssetExporters;
using uTinyRipper.SerializedFiles;
using uTinyRipper.YAML;

namespace uTinyRipper.Classes.PlayableDirectors
{
	public struct ExposedReference : IAssetReadable, IYAMLExportable
	{
		public void Read(AssetReader reader)
		{
			PropertyNameId = reader.ReadInt32();
			Value = reader.ReadAsset<PPtr<Object>>();
		}

		public YAMLNode ExportYAML(IExportContainer container)
		{
			YAMLMappingNode node = new YAMLMappingNode();
			node.Add(PropertyNameId.ToHexString(), Value.ExportYAML(container));
			return node;
		}

		public IEnumerable<Object> FetchDependencies(ISerializedFile file, bool isLog = false)
		{
			yield return Value.FetchDependency(file);
		}

		public int PropertyNameId { get; private set; }
		public PPtr<Object> Value { get; private set; }

	}
}
