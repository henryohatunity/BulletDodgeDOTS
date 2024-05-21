using System.ComponentModel;
using System.Drawing;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering.UI;

[BurstCompile]
[UpdateAfter(typeof(BulletMoveSystem))]
public partial struct BulletLifecycleSystem : ISystem
{
    private bool useJob;
    public void OnCreate(ref SystemState state)
    {
        useJob = true;
        state.RequireForUpdate<Bullet>();
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (!useJob)
        {
            foreach (var (bulletLife, materialProperty, localTransform, entity
                         )
                     in SystemAPI.Query<RefRW<BulletLife>, RefRW<URPMaterialPropertyBaseColor>, RefRO<LocalTransform>>()
                         .WithAll<Bullet>().WithEntityAccess())
            {
                // Update color
                int life = bulletLife.ValueRO.life;
                int prevLife = bulletLife.ValueRO.prevLife;
                if (life != prevLife)
                {
                    // Red -> Yellow
                    // g: 0 -> 1
                    float4 col = materialProperty.ValueRO.Value;
                    // Debug.Log($"col:{col.x},{col.y},{col.z},{col.w}");

                    int maxLife = bulletLife.ValueRO.maxLift;
                    float gVal = 1.0f / (float)maxLife * (float)life;
                    gVal = math.max(1.0f - gVal, 0);
                    materialProperty.ValueRW.Value = new float4(1.0f, gVal, 0, 1.0f);
                    bulletLife.ValueRW.prevLife = life;
                }
            }
        }
        else
        {
            var job = new BulletLifecycleJob();
            job.ScheduleParallel();
        }
    }

    [WithAll(typeof(Bullet))]
    [BurstCompile]
    public partial struct BulletLifecycleJob : IJobEntity
    {
        public void Execute(Entity bulletEntity, ref BulletLife bulletLife, ref URPMaterialPropertyBaseColor baseColor, ref LocalTransform localTransform)
        {
            int life = bulletLife.life;
            int prevLife = bulletLife.prevLife;
            if (life != prevLife)
            {
                // Red -> Yellow
                // g: 0 -> 1
                float4 col = baseColor.Value;
                int maxLife = bulletLife.maxLift;
                float gVal = 1.0f / (float)maxLife * (float)life;
                gVal = math.max(1.0f - gVal, 0);
                baseColor.Value = new float4(1.0f, gVal, 0, 1.0f);
                bulletLife.prevLife = life;
            }
        }
    }
}
