using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

public class CountUIText : MonoBehaviour
{
    public static CountUIText Instance;
    public TextMeshProUGUI bulletCountText;

    private BulletCount bulletCount;

    void Awake()
    {
        Instance = this;
        UpdateBulletCount(0);
    }

    IEnumerator Start()
    {
        enabled = false;

        while (!enabled)
        {
            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            var query = new EntityQueryBuilder(Allocator.Temp).WithAll<BulletCount>().Build(entityManager);
            if (query.HasSingleton<BulletCount>())
            {
                bulletCount = query.GetSingleton<BulletCount>();
                enabled = true;
            }

            yield return null;
        }
    }

    void Update()
    {
        var query = new EntityQueryBuilder(Allocator.Temp).WithAll<BulletCount>().Build(World.DefaultGameObjectInjectionWorld.EntityManager);
        if (query.HasSingleton<BulletCount>())
        {
            bulletCountText.text = query.GetSingleton<BulletCount>().cnt.ToString();
        }
        else
        {
            Debug.LogWarning("No BulletCount Singleton");
        }

        /*var query2 = new EntityQueryBuilder(Allocator.Temp).WithAll<BulletCount>()
            .Build(World.DefaultGameObjectInjectionWorld.EntityManager);
        var entities = query2.ToEntityArray(Allocator.Temp);
        foreach (var thisEntity in entities)
        {
            Debug.Log($"thisEntity:{thisEntity.Index},{thisEntity.Version}");
        }*/
    }

    public void UpdateBulletCount(int cnt)
    {
        bulletCountText.text = cnt.ToString();
    }
}
/*
// If not using ECS, no need to do anything here
if (!useECS) return;
		
// Get a reference to an EntityManager which is how we will create and access entities
manager = World.DefaultGameObjectInjectionWorld.EntityManager;

// Create a query that will find the Directory entity. The Directory is created automatically
// by the baking process of the Directory GameObject which you can find in the "Baker Sub Scene"
// in the ECS Shooter scene
EntityQuery query = new EntityQueryBuilder(Allocator.Temp).WithAll<Directory>().Build(manager);

// If this query finds one and only one Directory, then grab the bullet entity and store it
if (query.HasSingleton<Directory>())
    bulletEntityPrefab = query.GetSingleton<Directory>().bulletPrefab;
*/
