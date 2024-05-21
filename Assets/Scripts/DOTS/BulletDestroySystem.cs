
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine.Rendering;

[BurstCompile]
[UpdateAfter(typeof(BulletLifecycleSystem))]
public partial struct BulletDestroySystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Bullet>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (bulletLife, localTransform, entity) in SystemAPI.Query<RefRO<BulletLife>, RefRO<LocalTransform>>()
                     .WithAll<Bullet>().WithEntityAccess())
        {
            if (bulletLife.ValueRO.life == 0)
            {
                ecb.DestroyEntity(entity);
            }
            else if (math.distancesq(localTransform.ValueRO.Position, float3.zero) > (2500.0f))
            {
                ecb.DestroyEntity(entity);
            }
        }
    }
}
