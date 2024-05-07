using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms; 
using UnityEngine;
using Random = Unity.Mathematics.Random;

[BurstCompile]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct BulletSpawnSystem : ISystem
{
    private Random random;
    public void OnCreate(ref SystemState state)
    {
        if(!SystemAPI.HasSingleton<BulletSpawner>())
            Debug.LogError("[OnCreate] No BulletSpawner Found!");
        
        random = new Random(999);
        state.RequireForUpdate<BulletSpawner>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if(!SystemAPI.HasSingleton<BulletSpawner>())
            Debug.LogError("[OnUpdate] No BulletSpawner Found!");

        float3 aimPoint = new float3(0, 0, 0);
        
        BulletSpawner bulletSpawner = SystemAPI.GetSingleton<BulletSpawner>();
        if (bulletSpawner.nextSpawnTime < SystemAPI.Time.ElapsedTime)
        {
            int spawnCount = random.NextInt(1, bulletSpawner.maxConcurrentSpawnCnt);
            for (int i = 0; i < spawnCount; i++)
            {
                Entity bullet = state.EntityManager.Instantiate(bulletSpawner.bulletPrefab);

                float radius = bulletSpawner.spawnRadius;
                float2 randomPositionFloat2 = random.NextFloat2Direction() * radius;

                float3 randomPosition = new float3(randomPositionFloat2.x, 0, randomPositionFloat2.y);

                LocalTransform localTransform = LocalTransform.FromPosition(randomPosition);
                localTransform.Scale = 1;
                
                // !!!!!
                state.EntityManager.SetComponentData(bullet, localTransform);
                state.EntityManager.SetComponentData(bullet, new Movement()
                {
                    Velocity = math.normalizesafe(aimPoint - randomPosition) * random.NextFloat(bulletSpawner.bulletSpeedMin, bulletSpawner.bulletSpeedMax),
                });
                
            }

            bulletSpawner.nextSpawnTime = (float)SystemAPI.Time.ElapsedTime +
                                          random.NextFloat(bulletSpawner.timeBetweenSpawnMin,
                                              bulletSpawner.timeBetweenSpawnMax);

            SystemAPI.SetSingleton(bulletSpawner);
        }
        
        
    }
}
