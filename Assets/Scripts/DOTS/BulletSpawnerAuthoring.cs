using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class BulletSpawnerAuthoring : MonoBehaviour
{
    public GameObject bulletPrefab;
    public int maxConcurrentSpawnCnt = 100;
    public float timeBetweenSpawnMin = 0.1f;
    public float timeBetweenSpawnMax = 3.0f;
    public float spawnRadius = 20.0f;
    public float bulletSpeedMin = 1.0f;
    public float bulletSpeedMax = 10.0f;

    private class BulletSpawnerBaker : Baker<BulletSpawnerAuthoring>
    {
        public override void Bake(BulletSpawnerAuthoring authoring)
        {
            Debug.Log("Bake");
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new BulletSpawner()
            {
                bulletPrefab = GetEntity(authoring.bulletPrefab, TransformUsageFlags.Dynamic),
                maxConcurrentSpawnCnt = authoring.maxConcurrentSpawnCnt,
                timeBetweenSpawnMin = authoring.timeBetweenSpawnMin,
                timeBetweenSpawnMax =  authoring.timeBetweenSpawnMax,
                spawnRadius = authoring.spawnRadius,
                bulletSpeedMin = authoring.bulletSpeedMin,
                bulletSpeedMax = authoring.bulletSpeedMax,
                nextSpawnTime = 0
            });
        }
    }
    
}

public struct BulletSpawner : IComponentData
{
    public Entity bulletPrefab;
    public int maxConcurrentSpawnCnt;
    public float spawnRadius;
    public float timeBetweenSpawnMin;
    public float timeBetweenSpawnMax;
    public float nextSpawnTime;
    public float bulletSpeedMin;
    public float bulletSpeedMax;
}
