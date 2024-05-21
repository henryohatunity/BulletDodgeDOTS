
using Unity.Entities;
using UnityEngine;

[UpdateAfter(typeof(BulletSpawnSystem))]
public partial struct PlayerInputSystem : ISystem//, ISystemStartStop
{
    private bool spacePressed;
    
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerInput>();
        // Debug.Log($"OnCreate SystemAPI.HasSingleton<PlayerInput>():{SystemAPI.HasSingleton<PlayerInput>()}");
    }

    // public void OnStartRunning(ref SystemState state)
    // {
    //     Debug.Log($"OnStartRunning SystemAPI.HasSingleton<PlayerInput>():{SystemAPI.HasSingleton<PlayerInput>()}");
    // }
    //
    // public void OnStopRunning(ref SystemState state)
    // {
    //     
    // }

    public void OnUpdate(ref SystemState state)
    {
        spacePressed = Input.GetKeyDown(KeyCode.Space);
        // Update
        SystemAPI.SetSingleton(new PlayerInput(){spaceKey =  spacePressed});
    }
}
