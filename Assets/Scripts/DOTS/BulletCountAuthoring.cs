using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BulletCountAuthoring : MonoBehaviour
{
    private class BulletCountBaker : Baker<BulletCountAuthoring>
    {
        public override void Bake(BulletCountAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new BulletCount() { cnt = 0 });
            // Debug.Log($"BulletCount Entity Index:{entity.Index}, Version:{entity.Version}");
            // or
            // AddComponent<BulletCount>(entity);
        }
    }
}
