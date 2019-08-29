using System;
using System.Collections.Generic;
using uTinyRipper.AssetExporters;
using uTinyRipper.YAML;
using uTinyRipper.SerializedFiles;
using uTinyRipper.Classes.PlayableDirectors;

namespace uTinyRipper.Classes
{
	public sealed class PlayableDirector : Behaviour
	{
		public PlayableDirector(AssetInfo assetInfo) :
			base(assetInfo)
		{
		}

		private static int GetSerializedVersion(Version version)
		{
			return 3;
			// TODO unknown
			//return 1;
		}

		public override void Read(AssetReader reader)
		{
			base.Read(reader);
			PlayableAsset = reader.ReadAsset<PPtr<Object>>();
			InitialState = reader.ReadInt32();
			WrapMode = (DirectorWrapMode)reader.ReadInt32();
			DirectorUpdateMode = (DirectorUpdateMode)reader.ReadInt32();
			InitialTime = reader.ReadDouble();
			m_sceneBindings = reader.ReadAssetArray<SceneBinding>();
			m_exposedReferences = reader.ReadAssetArray<ExposedReference>();
		}

		public override IEnumerable<Object> FetchDependencies(ISerializedFile file, bool isLog = false)
		{
			foreach (Object asset in base.FetchDependencies(file, isLog))
			{
				yield return asset;
			}
			yield return PlayableAsset.FetchDependency(file, isLog, ToLogString, PlayableAssetName);
			foreach(SceneBinding sceneBinding in SceneBindings)
			{
				foreach(Object asset in sceneBinding.FetchDependencies(file, isLog))
				{
					yield return asset;
				}
			}
			foreach (ExposedReference exposedReference in ExposedReferences)
			{
				foreach (Object asset in exposedReference.FetchDependencies(file, isLog))
				{
					yield return asset;
				}
			}
		}

		protected override YAMLMappingNode ExportYAMLRoot(IExportContainer container)
		{
			YAMLMappingNode node = base.ExportYAMLRoot(container);
			node.AddSerializedVersion(GetSerializedVersion(container.ExportVersion));
			node.Add(PlayableAssetName, PlayableAsset.ExportYAML(container));
			node.Add(InitialStateName, InitialState);
			node.Add(WrapModeName, (int)WrapMode);
			node.Add(DirectorUpdateModeName, (int)DirectorUpdateMode);
			node.Add(InitialTimeName, (float)InitialTime);
			node.Add(SceneBindingsName, SceneBindings.ExportYAML(container));
			YAMLMappingNode exposedReferencesNode = new YAMLMappingNode();
			exposedReferencesNode.Add("m_References", ExposedReferences.ExportYAML(container));
			node.Add(ExposedReferencesName, exposedReferencesNode);
			return node;
		}

		public const string PlayableAssetName = "m_PlayableAsset";
		public const string InitialStateName = "m_InitialState";
		public const string WrapModeName = "m_WrapMode";
		public const string DirectorUpdateModeName = "m_DirectorUpdateMode";
		public const string InitialTimeName = "m_InitialTime";
		public const string SceneBindingsName = "m_SceneBindings";
		public const string ExposedReferencesName = "m_ExposedReferences";

		public PPtr<Object> PlayableAsset { get; private set; }
		public int InitialState { get; private set; }
		public DirectorWrapMode WrapMode { get; private set; }
		public DirectorUpdateMode DirectorUpdateMode { get; private set; }
		public double InitialTime { get; private set; }
		public IReadOnlyList<SceneBinding> SceneBindings => m_sceneBindings;
		public IReadOnlyList<ExposedReference> ExposedReferences => m_exposedReferences;

		private SceneBinding[] m_sceneBindings;
		private ExposedReference[] m_exposedReferences;
	}
}
