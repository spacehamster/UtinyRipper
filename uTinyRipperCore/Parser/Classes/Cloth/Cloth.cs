using System.Collections.Generic;
using uTinyRipper.AssetExporters;
using uTinyRipper.YAML;
using uTinyRipper.SerializedFiles;
using uTinyRipper.Classes.Cloths;

namespace uTinyRipper.Classes
{
	public sealed class Cloth : Behaviour
	{
		public Cloth(AssetInfo assetInfo) :
			base(assetInfo)
		{
		}

		public override void Read(AssetReader reader)
		{
			base.Read(reader);
			StretchingStiffness = reader.ReadSingle();
			BendingStiffness = reader.ReadSingle();
			UseTethers = reader.ReadBoolean();
			UseGravity = reader.ReadBoolean();
			reader.AlignStream(AlignType.Align4);
			Damping = reader.ReadSingle();
			ExternalAcceleration = reader.ReadAsset<Vector3f>();
			RandomAcceleration = reader.ReadAsset<Vector3f>();
			WorldVelocityScale = reader.ReadSingle();
			WorldAccelerationScale = reader.ReadSingle();
			Friction = reader.ReadSingle();
			CollisionMassScale = reader.ReadSingle();
			UseContinuousCollision = reader.ReadBoolean();
			reader.AlignStream(AlignType.Align4);
			SolverFrequency = reader.ReadSingle();
			SleepThreshold = reader.ReadSingle();
			m_coefficients = reader.ReadAssetArray<ClothSkinningCoefficient>();
			m_capsuleColliders = reader.ReadAssetArray<PPtr<CapsuleCollider>>();
			m_sphereColliders = reader.ReadAssetArray<ClothSphereColliderPair>();
			var pos = reader.BaseStream.Position;
			var data = reader.ReadBytes(50);
			reader.BaseStream.Position = pos;
			SelfCollisionDistance = reader.ReadSingle();
			SelfCollisionStiffness = reader.ReadSingle();
			m_selfAndInterCollisionIndices = reader.ReadUInt32Array();
			m_virtualParticleWeights = reader.ReadAssetArray<Vector3f>();
			m_virtualParticleIndices = reader.ReadUInt32Array();
		}

		public override IEnumerable<Object> FetchDependencies(ISerializedFile file, bool isLog = false)
		{
			foreach (Object asset in base.FetchDependencies(file, isLog))
			{
				yield return asset;
			}
			foreach(PPtr<CapsuleCollider> capsuleCollider in CapsuleColliders)
			{
				yield return capsuleCollider.FetchDependency(file, isLog, ToLogString, CapsuleCollidersName);
			}
			foreach (ClothSphereColliderPair sphereCollider in SphereColliders)
			{
				foreach(Object asset in sphereCollider.FetchDependencies(file)){
					yield return asset;
				}
			}		
		}

		protected override YAMLMappingNode ExportYAMLRoot(IExportContainer container)
		{
			YAMLMappingNode node = base.ExportYAMLRoot(container);
			node.Add(StretchingStiffnessName, StretchingStiffness);
			node.Add(BendingStiffnessName, BendingStiffness);
			node.Add(UseTethersName, UseTethers);
			node.Add(UseGravityName, UseGravity);
			node.Add(DampingName, Damping);
			node.Add(ExternalAccelerationName, ExternalAcceleration.ExportYAML(container));
			node.Add(RandomAccelerationName, RandomAcceleration.ExportYAML(container));
			node.Add(WorldVelocityScaleName, WorldVelocityScale);
			node.Add(WorldAccelerationScaleName, WorldAccelerationScale);
			node.Add(FrictionName, Friction);
			node.Add(CollisionMassScaleName, CollisionMassScale);
			node.Add(UseContinuousCollisionName, UseContinuousCollision);
			node.Add(SolverFrequencyName, SolverFrequency);
			node.Add(SleepThresholdName, SleepThreshold);
			node.Add(CoefficientsName, Coefficients.ExportYAML(container));
			node.Add(CapsuleCollidersName, CapsuleColliders.ExportYAML(container));
			node.Add(SphereCollidersName, SphereColliders.ExportYAML(container));
			node.Add(SelfCollisionDistanceName, SelfCollisionDistance);
			node.Add(SelfCollisionStiffnessName, SelfCollisionStiffness);
			node.Add(SelfAndInterCollisionIndicesName, SelfAndInterCollisionIndices.ExportYAML(false));
			node.Add(VirtualParticleWeightsName, VirtualParticleWeights.ExportYAML(container));
			node.Add(VirtualParticleIndicesName, VirtualParticleIndices.ExportYAML(false));
			return node;
		}


		public const string StretchingStiffnessName = "m_StretchingStiffness";
		public const string BendingStiffnessName = "m_BendingStiffness";
		public const string UseTethersName = "m_UseTethers";
		public const string UseGravityName = "m_UseGravity";
		public const string DampingName = "m_Damping";
		public const string ExternalAccelerationName = "m_ExternalAcceleration";
		public const string RandomAccelerationName = "m_RandomAcceleration";
		public const string WorldVelocityScaleName = "m_WorldVelocityScale";
		public const string WorldAccelerationScaleName = "m_WorldAccelerationScale";
		public const string FrictionName = "m_Friction";
		public const string CollisionMassScaleName = "m_CollisionMassScale";
		public const string UseContinuousCollisionName = "m_UseContinuousCollision";
		public const string SolverFrequencyName = "m_SolverFrequency";
		public const string SleepThresholdName = "m_SleepThreshold";
		public const string CoefficientsName = "m_Coefficients";
		public const string CapsuleCollidersName = "m_CapsuleColliders";
		public const string SphereCollidersName = "m_SphereColliders";
		public const string SelfCollisionDistanceName = "m_SelfCollisionDistance";
		public const string SelfCollisionStiffnessName = "m_SelfCollisionStiffness";
		public const string SelfAndInterCollisionIndicesName = "m_SelfAndInterCollisionIndices";
		public const string VirtualParticleWeightsName = "m_VirtualParticleWeights";
		public const string VirtualParticleIndicesName = "m_VirtualParticleIndices";


		public float StretchingStiffness { get; private set; }
		public float BendingStiffness { get; private set; }
		public bool UseTethers { get; private set; }
		public bool UseGravity { get; private set; }
		public float Damping { get; private set; }
		public Vector3f ExternalAcceleration { get; private set; }
		public Vector3f RandomAcceleration { get; private set; }
		public float WorldVelocityScale { get; private set; }
		public float WorldAccelerationScale { get; private set; }
		public float Friction { get; private set; }
		public float CollisionMassScale { get; private set; }
		public bool UseContinuousCollision { get; private set; }
		public float SolverFrequency { get; private set; }
		public float SleepThreshold { get; private set; }
		public IReadOnlyList<ClothSkinningCoefficient> Coefficients => m_coefficients;
		public IReadOnlyList<PPtr<CapsuleCollider>> CapsuleColliders => m_capsuleColliders;
		public IReadOnlyList<ClothSphereColliderPair> SphereColliders => m_sphereColliders;
		public float SelfCollisionDistance { get; private set; }
		public float SelfCollisionStiffness { get; private set; }
		public IReadOnlyList<uint> SelfAndInterCollisionIndices => m_selfAndInterCollisionIndices;
		public IReadOnlyList<Vector3f> VirtualParticleWeights => m_virtualParticleWeights;
		public IReadOnlyList<uint> VirtualParticleIndices => m_virtualParticleIndices;

		private ClothSkinningCoefficient[] m_coefficients;
		private PPtr<CapsuleCollider>[] m_capsuleColliders;
		private ClothSphereColliderPair[] m_sphereColliders;
		public uint[] m_selfAndInterCollisionIndices;
		public Vector3f[] m_virtualParticleWeights;
		public uint[] m_virtualParticleIndices;
	}
}