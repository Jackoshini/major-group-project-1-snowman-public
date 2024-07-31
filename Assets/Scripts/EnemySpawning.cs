using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawning : MonoBehaviour
{

    private float lastSpawnTime;
    private float increaseHealthTime;
    private float increaseGroupSizeTime;
    [SerializeField] private float healthTime;
    [SerializeField] private float groupSizeTime;
    [SerializeField] GameObject[] spawners;
    [SerializeField] private float spawnTime = 10f;
    [SerializeField] public int skeletonGroup = 5;
    [SerializeField] public int necromancerGroup = 2;
    [SerializeField] GameObject skeleton;
    [SerializeField] GameObject necromancer;
    public float skeltonHealth;
    public float necromancerHealth;
    // Start is called before the first frame update
    void Start()
    {
        increaseHealthTime = healthTime;
        lastSpawnTime = spawnTime;
        increaseGroupSizeTime = groupSizeTime;
        skeltonHealth = skeleton.GetComponent<Skeleton>().health;
        necromancerHealth = necromancer.GetComponent<Necromancer>().health;

        if (File.Exists(Application.persistentDataPath + "/data.json"))
        {
            using StreamReader reader = new(Application.persistentDataPath + "/data.json");
            LevelData data = LevelData.Load(reader.ReadToEnd());
            skeltonHealth = data.SkeletonMaxHp;
            necromancerHealth = data.NecromancerMaxHp;
            skeletonGroup = data.SkeletonGroupSize;
            necromancerGroup = data.NecromancerGroupSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        lastSpawnTime -= Time.deltaTime;
        increaseHealthTime -= Time.deltaTime;
        increaseGroupSizeTime -= Time.deltaTime;

        if (lastSpawnTime <= 0)
        {
            spawnEnemies();
            lastSpawnTime = spawnTime;
        }
        if (increaseHealthTime <= 0)
        {
            increaseHealth();
            increaseHealthTime = healthTime;
        }
        if (increaseGroupSizeTime <= 0)
        {
            skeletonGroup += 5;
            necromancerGroup += 1;
            increaseGroupSizeTime = groupSizeTime;
        }

    }

    private void increaseHealth()
    {
        skeltonHealth += 10.0f;
        necromancerHealth += 10.0f;
    }

    private void spawnEnemies()
    {
        int enemyType = UnityEngine.Random.Range(0, 100);
        if (enemyType < 70) 
        {
            spawnSkeletons();
        }
        else
        {
            spawnNecromancers();
        }
    }

    private void spawnNecromancers()
    {
        for (int i = 0; i < necromancerGroup; i++)
        {
            int randomSpawner = UnityEngine.Random.Range(0, spawners.Length-1);
            Vector3 spawnLocation = spawners[randomSpawner].gameObject.transform.position;
            GameObject necromancerSpawn = Instantiate(necromancer, spawnLocation,Quaternion.identity);
            necromancerSpawn.GetComponent<Necromancer>().health = necromancerHealth;
        }
    }

    private void spawnSkeletons()
    {
        for (int i = 0; i < skeletonGroup; i++)
        {
            int randomSpawner = UnityEngine.Random.Range(0, spawners.Length-1);
            Vector3 spawnLocation = spawners[randomSpawner].gameObject.transform.position;
            GameObject skeletonSpawn = Instantiate(skeleton, spawnLocation,Quaternion.identity);
            skeletonSpawn.GetComponent<Skeleton>().health = skeltonHealth;
        }
    }
}
