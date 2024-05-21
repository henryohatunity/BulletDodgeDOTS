using Unity.Entities;
using Unity.Mathematics;

// Tags
public struct Bullet : IComponentData
{
}

public struct Player : IComponentData
{
}


// Components
public struct BulletCount : IComponentData
{
    public int cnt;
}

public struct BulletLife : IComponentData
{
    public int maxLift;
    public int prevLife;
    public int life;
}

public struct Movement : IComponentData
{
    public float3 Velocity;
}

public struct PlayerInput : IComponentData
{
    public bool spaceKey;
}

// test
public struct DummyCube : IComponentData
{
}






