using System.Collections.Generic;
using uTinyRipper.AssetExporters;
using uTinyRipper.SerializedFiles;
using uTinyRipper.YAML;

namespace uTinyRipper.Classes.Cloths
{
	public struct ClothSkinningCoefficient : IAssetReadable, IYAMLExportable
	{
		public void Read(AssetReader reader)
		{
			MaxDistance = reader.ReadSingle();
			CollisionSphereDistance = reader.ReadSingle();
		}

		public YAMLNode ExportYAML(IExportContainer container)
		{
			YAMLMappingNode node = new YAMLMappingNode();
			node.Add(MaxDistanceName, MaxDistance);
			node.Add(CollisionSphereDistanceName, CollisionSphereDistance);
			return node;
		}


		public const string MaxDistanceName = "maxDistance";
		public const string CollisionSphereDistanceName = "collisionSphereDistance";

		public float MaxDistance { get; private set; }
		public float CollisionSphereDistance { get; private set; }
	}
}
