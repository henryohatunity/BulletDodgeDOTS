using Mono.Cecil;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

[BurstCompile]
[UpdateAfter(typeof(BulletSpawnSystem))]
public partial struct BulletMoveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Bullet>();
        // state.Enabled = false;
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        // Simple
        // foreach (var (bulletMovement, bulletTransform) in SystemAPI.Query<RefRO<Movement>, RefRW<LocalTransform>>().WithAll<Bullet>())
        // {
        //     var movement = bulletMovement.ValueRO.Velocity * SystemAPI.Time.DeltaTime;
        //     var newPosition = bulletTransform.ValueRO.Position + movement;
        //     bulletTransform.ValueRW.Position = newPosition;
        // }
        
        
        // Job
        var query = SystemAPI.QueryBuilder().WithAll<Bullet, LocalTransform>().Build();
        var allBulletTransforms = query.ToComponentDataArray<LocalTransform>(state.WorldUpdateAllocator);
        var allBulletEntities = query.ToEntityArray(state.WorldUpdateAllocator);
        var deltaTime = SystemAPI.Time.DeltaTime;
        
        var job = new BulletMoveJob
        {
            allTransforms = allBulletTransforms,
            allEntities = allBulletEntities,
            DeltaTime = deltaTime
        };
        job.ScheduleParallel();
    }


    [WithAll(typeof(Bullet))]
    [BurstCompile]
    public partial struct BulletMoveJob : IJobEntity
    {
        private const float ReflectDistance = 1.0f;
        private const float ReflectDistanceSqr = ReflectDistance * ReflectDistance;

        [ReadOnly] public NativeArray<LocalTransform> allTransforms;
        [ReadOnly] public NativeArray<Entity> allEntities;
        public float DeltaTime;

        public void Execute(Entity bulletEntity, ref BulletLife bulletLife, ref LocalTransform bulletTransform, ref Movement bulletMovement)
        {
            var velocity = bulletMovement.Velocity;
            var newPosition = bulletTransform.Position + velocity * DeltaTime;

            for (var i = 0; i < allEntities.Length; i++)
            {
                var otherBulletTransform = allTransforms[i];
                if (bulletEntity == allEntities[i])
                {
                    continue;
                }

                var dis = math.distancesq(newPosition, otherBulletTransform.Position);
                if (dis > ReflectDistanceSqr)
                {
                    continue;
                }
                
                // Collision!
                var collisionSurfaceNormal = math.normalize(otherBulletTransform.Position - newPosition);
                velocity = math.reflect(velocity, collisionSurfaceNormal);
                newPosition = bulletTransform.Position + velocity * DeltaTime;
                bulletMovement.Velocity = velocity;

                var newLife = bulletLife.life - 1;
                bulletLife.life = math.max(0, newLife);
                // Debug.Log($"Hit! Entity Index:{bulletEntity.Index}, dis:{dis}, life:{bulletLife.life}");
                
                break;
            }

            bulletTransform.Position = newPosition;
        }
    }
}
