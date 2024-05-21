using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class DummyCubeAuthoring : MonoBehaviour
{
    private class DummyCubeBaker : Baker<DummyCubeAuthoring>
    {
        public override void Bake(DummyCubeAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<DummyCube>(entity);
        }
    }
    

}
