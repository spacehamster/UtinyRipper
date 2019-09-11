using uTinyRipper.AssetExporters;
using uTinyRipper.Classes.AnimationClips;
using uTinyRipper.Classes.ParticleSystems;
using uTinyRipper.YAML;

namespace uTinyRipper.Classes.LineRenderers
{
	public struct LineRendererParameters : IAssetReadable, IYAMLExportable
	{

		private static int GetSerializedVersion(Version version)
		{
			if(version.IsGreaterEqual(2018, 3))
			{
				return 3;
			}
			return 2;
			// unknown
			//return 1;
		}
		/// <summary>
		/// 2018.3 and greater
		/// </summary>
		private static bool IsReadShadowBias(Version version)
		{
			return version.IsGreaterEqual(2018, 3);
		}
		public void Read(AssetReader reader)
		{
			WidthMultiplier = reader.ReadSingle();
			WidthCurve = reader.ReadAsset<AnimationCurveTpl<Float>>();
			ColorGradient = reader.ReadAsset<Gradient>();
			NumCornerVertices = reader.ReadInt32();
			NumCapVertices = reader.ReadInt32();
			Alignment = (LineAlignment)reader.ReadInt32();
			TextureMode = (LineTextureMode)reader.ReadInt32();
			if (IsReadShadowBias(reader.Version))
			{
				ShadowBias = reader.ReadSingle();
			}
			GenerateLightingData = reader.ReadBoolean();
			reader.AlignStream(AlignType.Align4);
		}

		public YAMLNode ExportYAML(IExportContainer container)
		{
			YAMLMappingNode node = new YAMLMappingNode();
			node.AddSerializedVersion(GetSerializedVersion(container.ExportVersion));
			node.Add(WidthMultiplierName, WidthMultiplier);
			node.Add(WidthCurveName, WidthCurve.ExportYAML(container));
			node.Add(ColorGradientName, ColorGradient.ExportYAML(container));
			node.Add(NumCornerVerticesName, NumCornerVertices);
			node.Add(NumCapVerticesName, NumCapVertices);
			node.Add(AlignmentName, (int)Alignment);
			node.Add(TextureModeName, (int)TextureMode);
			node.Add(ShadowBiasName, ShadowBias);
			node.Add(GenerateLightingDataName, GenerateLightingData);
			return node;
		}

		public const string WidthMultiplierName = "widthMultiplier";
		public const string WidthCurveName = "widthCurve";
		public const string ColorGradientName = "colorGradient";
		public const string NumCornerVerticesName = "numCornerVertices";
		public const string NumCapVerticesName = "numCapVertices";
		public const string AlignmentName = "alignment";
		public const string TextureModeName = "textureMode";
		public const string ShadowBiasName = "shadowBias";
		public const string GenerateLightingDataName = "generateLightingData";

		public float WidthMultiplier { get; private set; }
		public AnimationCurveTpl<Float> WidthCurve { get; private set; }
		public Gradient ColorGradient { get; private set; }
		public int NumCornerVertices { get; private set; }
		public int NumCapVertices { get; private set; }
		public LineAlignment Alignment { get; private set; }
		public LineTextureMode TextureMode { get; private set; }
		public float ShadowBias { get; private set; }
		public bool GenerateLightingData { get; private set; }
	}
}
