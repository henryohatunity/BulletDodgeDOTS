using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(BulletSpawnSystem))]
public partial struct BulletMoveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Bullet>();
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
        
        
        
        
        
        
        // var query = SystemAPI.QueryBuilder().WithAll<Bullet, LocalTransform>().Build();
        



        var job = new BulletMoveJob
        {
            // allBulletTransforms =
            deltaTime = SystemAPI.Time.DeltaTime
        };
        job.ScheduleParallel();
    }


    [WithAll(typeof(Bullet))]
    [BurstCompile]
    public partial struct BulletMoveJob : IJobEntity
    {
        private const float reflectDistance = 0.1f;
        private const float reflectDistancesqr = reflectDistance * reflectDistance;

        // public NativeArray<LocalTransform> allBulletTransforms;
        // public NativeArray<Entity> allBullets;
        public float deltaTime;

        public void Execute(ref LocalTransform bulletTransform, ref Movement bulletMovement)
        {
            var newPosition = bulletTransform.Position + bulletMovement.Velocity * deltaTime;
            bulletTransform.Position = newPosition;
        }
    }
}
