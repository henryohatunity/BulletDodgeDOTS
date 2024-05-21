using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    public float Velocity;
    public int life;
    private class BulletBaker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<Bullet>(entity);
            AddComponent<Movement>(entity);
            AddComponent(entity, new BulletLife(){maxLift = authoring.life, prevLife = authoring.life, life = authoring.life});
            // AddComponent(entity, new Bullet(){});
            // AddComponent(entity, new Movement(){});
        }
    }
}
