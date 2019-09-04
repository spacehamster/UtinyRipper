using System.Collections.Generic;
using uTinyRipper.AssetExporters;
using uTinyRipper.SerializedFiles;
using uTinyRipper.YAML;

namespace uTinyRipper.Classes.Cloths
{
	public struct ClothSphereColliderPair : IAssetReadable, IYAMLExportable
	{
		public void Read(AssetReader reader)
		{
			First = reader.ReadAsset<PPtr<SphereCollider>>();
			Second = reader.ReadAsset<PPtr<SphereCollider>>();
		}

		public YAMLNode ExportYAML(IExportContainer container)
		{
			YAMLMappingNode node = new YAMLMappingNode();
			node.Add(FirstName, First.ExportYAML(container));
			node.Add(SecondName, Second.ExportYAML(container));
			return node;
		}

		public IEnumerable<Object> FetchDependencies(ISerializedFile file, bool isLog = false)
		{
			yield return First.FetchDependency(file, isLog, () => nameof(First), FirstName);
			yield return Second.FetchDependency(file, isLog, () => nameof(Second), SecondName);
		}

		public const string FirstName = "first";
		public const string SecondName = "second";

		public PPtr<SphereCollider> First { get; private set; }
		public PPtr<SphereCollider> Second { get; private set; }
	}
}
