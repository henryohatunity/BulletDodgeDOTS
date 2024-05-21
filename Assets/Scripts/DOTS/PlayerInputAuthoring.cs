using Unity.Entities;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerInputAuthoring : MonoBehaviour
{
    private class PlayerInputBaker : Baker<PlayerInputAuthoring>
    {
        public override void Bake(PlayerInputAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent<PlayerInput>(entity);
        }
    }
}
