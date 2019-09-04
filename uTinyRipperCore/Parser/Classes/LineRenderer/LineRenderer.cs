using System.Collections.Generic;
using uTinyRipper.AssetExporters;
using uTinyRipper.Classes.LineRenderers;
using uTinyRipper.SerializedFiles;
using uTinyRipper.YAML;

namespace uTinyRipper.Classes
{
	public sealed class LineRenderer : Renderer
	{
		public LineRenderer(AssetInfo assetInfo) :
			base(assetInfo)
		{
		}

		public override void Read(AssetReader reader)
		{
			base.Read(reader);
			m_Positions = reader.ReadAssetArray<Vector3f>();
			Parameters = reader.ReadAsset<LineRendererParameters>();
			UseWorldSpace = reader.ReadBoolean();
			Loop = reader.ReadBoolean();
		}

		protected override YAMLMappingNode ExportYAMLRoot(IExportContainer container)
		{
			YAMLMappingNode node = base.ExportYAMLRoot(container);
			node.Add(PositionsName, Positions.ExportYAML(container));
			node.Add(ParametersName, Parameters.ExportYAML(container));
			node.Add(UseWorldSpaceName, UseWorldSpace);
			node.Add(LoopName, Loop);
			return node;
		}

		public const string PositionsName = "m_Positions";
		public const string ParametersName = "m_Parameters";
		public const string UseWorldSpaceName = "m_UseWorldSpace";
		public const string LoopName = "m_Loop";

		public IReadOnlyList<Vector3f> Positions => m_Positions;
		public LineRendererParameters Parameters { get; private set; }
		public bool UseWorldSpace { get; private set; }
		public bool Loop { get; private set; }

		private Vector3f[] m_Positions;
	}
}
