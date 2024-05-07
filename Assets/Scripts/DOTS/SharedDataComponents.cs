using Unity.Entities;
using Unity.Mathematics;

// Tags
public struct Bullet : IComponentData
{
}

public struct Player : IComponentData
{
}


public struct Movement : IComponentData
{
    public float3 Velocity;
}


