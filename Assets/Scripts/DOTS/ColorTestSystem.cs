using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

[UpdateAfter(typeof(BulletSpawnSystem))]

public partial struct ColorTestSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerInput>();
    }
    
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        if (SystemAPI.GetSingleton<PlayerInput>().spaceKey)
        {
            foreach (var baseColorMaterialProperty in SystemAPI.Query<RefRW<URPMaterialPropertyBaseColor>>()
                         .WithAll<Bullet>())
            {
                float4 color = baseColorMaterialProperty.ValueRO.Value;
                Debug.Log($"current color before:{color.x},{color.y},{color.z},{color.w}");

                float red = math.max(color.x - 0.25f, 0);
                baseColorMaterialProperty.ValueRW.Value = new float4(red, color.y, color.z, color.w);
                Debug.Log($"current color after:{color.x},{color.y},{color.z},{color.w}");
            }
        }
    }
}
