using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace Entities.Lesson9
{
    /// <summary>
    /// 建筑类型
    /// </summary>
    public enum BuildingType
    {
        /// <summary>
        /// 兵营
        /// </summary>
        BT_Spawner,

        /// <summary>
        /// 防御塔
        /// </summary>
        BT_DefenderTower,
        BT_MAX,
    }

    /// <summary>
    /// 护甲类型
    /// </summary>
    public enum ArmorType
    {
        /// <summary>
        /// 无甲
        /// </summary>
        AT_None = 0,

        /// <summary>
        /// 轻甲
        /// </summary>
        AT_Light,

        /// <summary>
        /// 中甲
        /// </summary>
        AT_Normal,

        /// <summary>
        /// 重甲
        /// </summary>
        AT_Heavy,

        /// <summary>
        /// 特殊类型甲
        /// </summary>
        AT_Hero,
        AT_Max,
    }

    /// <summary>
    /// 伤害类型
    /// </summary>
    public enum DamageType
    {
        /// <summary>
        /// 挥砍伤害
        /// </summary>
        DT_Slash = 0,

        /// <summary>
        /// 穿刺伤害
        /// </summary>
        DT_Pricks,

        /// <summary>
        /// 粉碎伤害
        /// </summary>
        DT_Smash,

        /// <summary>
        /// 魔法伤害
        /// </summary>
        DT_Magic,

        /// <summary>
        /// 混合型伤害
        /// </summary>
        DT_Chaos,

        /// <summary>
        /// 特殊类型伤害
        /// </summary>
        DT_Hero,
        DT_Max,
    }

    struct EntitySpawnerAllComponentData : IComponentData
    {
        /// <summary>
        /// 生成的Entity原型对象，用于实例化克隆   8byte
        /// </summary>
        public Entity EntityProtoType;

        /// <summary>
        /// 建筑类型    4byte
        /// </summary>
        public BuildingType BuildingType;

        /// <summary>
        /// 当前等级    4byte
        /// </summary>
        public int Level;

        /// <summary>
        /// 每多少秒生成一次    4byte
        /// </summary>
        public float TickTime;

        /// <summary>
        /// 每次生成几个Entity    4byte
        /// </summary>
        public int SpawnCountPerTickTime;

        /// <summary>
        /// 最大生命值   4byte
        /// </summary>
        public float MaxLife;

        /// <summary>
        /// 当前生命值   4byte
        /// </summary>
        public float CurrentLife;

        /// <summary>
        /// 护甲类型    4byte
        /// </summary>
        public ArmorType ArmorType;

        /// <summary>
        /// 伤害类型    4byte
        /// </summary>
        public DamageType DamageType;

        /// <summary>
        /// 最大攻击力   4byte
        /// </summary>
        public float MaxDamage;

        /// <summary>
        /// 最小攻击力   4byte
        /// </summary>
        ///
        public float MinDamage;

        /// <summary>
        /// 升级时间    4byte
        /// </summary>
        public float UpgradeTime;

        /// <summary>
        /// 升级费用    4byte
        /// </summary>
        public float UpgradeCost;
    }

    struct EntitySpawnerComponentData : IComponentData
    {
        /// <summary>
        /// 当前生命值   4byte
        /// </summary>
        public float CurrentLife;
    }

    struct EntitySpawnerBlobData
    {
        /// <summary>
        /// 生成的Entity原型对象，用于实例化克隆   8byte
        /// </summary>
        public Entity EntityProtoType;

        /// <summary>
        /// 建筑类型    4byte
        /// </summary>
        public BuildingType BuildingType;

        /// <summary>
        /// 当前等级    4byte
        /// </summary>
        public int Level;

        /// <summary>
        /// 每多少秒生成一次    4byte
        /// </summary>
        public float TickTime;

        /// <summary>
        /// 每次生成几个Entity    4byte
        /// </summary>
        public int SpawnCountPerTickTime;

        /// <summary>
        /// 最大生命值   4byte
        /// </summary>
        public float MaxLife;

        /// <summary>
        /// 护甲类型    4byte
        /// </summary>
        public ArmorType ArmorType;

        /// <summary>
        /// 伤害类型    4byte
        /// </summary>
        public DamageType DamageType;

        /// <summary>
        /// 最大攻击力   4byte
        /// </summary>
        public float MaxDamage;

        /// <summary>
        /// 最小攻击力   4byte
        /// </summary>
        ///
        public float MinDamage;

        /// <summary>
        /// 升级时间    4byte
        /// </summary>
        public float UpgradeTime;

        /// <summary>
        /// 升级费用    4byte
        /// </summary>
        public float UpgradeCost;
    }

    struct EntitySpawnerSettings : IComponentData
    {
        /// <summary>
        /// 8byte
        /// </summary>
        public BlobAssetReference<EntitySpawnerBlobData> BlobSettings;
    }

    public class EntitySpawnerAuthoring : MonoBehaviour
    {
        [SerializeField] private GameObject protoTypePrefab = null;
        [SerializeField] private BuildingType buildingType = BuildingType.BT_Spawner;
        [SerializeField] private int level = 1;
        [SerializeField, Range(1.0f, 10.0f)] private float tickTime = 5.0f;
        [SerializeField, Range(1, 8)] private int spawnCountPerTickTime = 1;
        [SerializeField, Range(100, 3000)] private int maxLife = 1000;
        [SerializeField] private ArmorType armorType = ArmorType.AT_Normal;
        [SerializeField] private DamageType damageType = DamageType.DT_Magic;
        [SerializeField] private float maxDamage = 0;
        [SerializeField] private float minDamage = 0;
        [SerializeField] private float upgradeTime = 100.0f;
        [SerializeField] private float upgradeCost = 100;

        public class Baker : Baker<EntitySpawnerAuthoring>
        {
            public override void Bake(EntitySpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                // ---使用AllComponentData
                // var data = new EntitySpawnerAllComponentData
                // {
                //     EntityProtoType = GetEntity(authoring.protoTypePrefab, TransformUsageFlags.Dynamic),
                //     BuildingType = authoring.buildingType,
                //     Level = authoring.level,
                //     TickTime = authoring.tickTime,
                //     SpawnCountPerTickTime = authoring.spawnCountPerTickTime,
                //     MaxLife = authoring.maxLife,
                //     ArmorType = authoring.armorType,
                //     DamageType = authoring.damageType,
                //     MaxDamage = authoring.maxDamage,
                //     MinDamage = authoring.minDamage,
                //     UpgradeTime = authoring.upgradeTime,
                //     UpgradeCost = authoring.upgradeCost,
                //     CurrentLife = authoring.maxLife
                // };
                // AddComponent(data);
                // ---

                // ---使用BlobAssets
                AddComponent(entity, new EntitySpawnerComponentData
                {
                    CurrentLife = authoring.maxLife
                });

                var settings = CreateSpawnerBlobSettings(authoring);
                AddBlobAsset(ref settings, out var hash);

                AddComponent(entity, new EntitySpawnerSettings
                {
                    BlobSettings = settings
                });
                // ---
            }

            private BlobAssetReference<EntitySpawnerBlobData> CreateSpawnerBlobSettings(EntitySpawnerAuthoring authoring)
            {
                var builder = new BlobBuilder(Allocator.Temp);
                ref EntitySpawnerBlobData spawnerBlobData = ref builder.ConstructRoot<EntitySpawnerBlobData>();
                spawnerBlobData.EntityProtoType = GetEntity(authoring.protoTypePrefab, TransformUsageFlags.Dynamic);
                spawnerBlobData.BuildingType = authoring.buildingType;
                spawnerBlobData.Level = authoring.level;
                spawnerBlobData.TickTime = authoring.tickTime;
                spawnerBlobData.SpawnCountPerTickTime = authoring.spawnCountPerTickTime;
                spawnerBlobData.MaxLife = authoring.maxLife;
                spawnerBlobData.ArmorType = authoring.armorType;
                spawnerBlobData.DamageType = authoring.damageType;
                spawnerBlobData.MaxDamage = authoring.maxDamage;
                spawnerBlobData.MinDamage = authoring.minDamage;
                spawnerBlobData.UpgradeTime = authoring.upgradeTime;
                spawnerBlobData.UpgradeCost = authoring.upgradeCost;

                var result = builder.CreateBlobAssetReference<EntitySpawnerBlobData>(Allocator.Persistent);
                builder.Dispose();
                return result;
            }
        }
    }
}