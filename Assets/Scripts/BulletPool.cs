using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private int poolSize = 10;

    [SerializeField] private List<GameObject> bulletList;

    public static BulletPool Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        addBulletsToPool(poolSize);
    }

    private void addBulletsToPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject bullet = Instantiate(BulletPrefab);
            bullet.SetActive(false);
            bulletList.Add(bullet);
            //bullet.transform.SetParent(transform); // Set the parent to the BulletPool for organization
            bullet.transform.parent = transform;
        }
    }

    public GameObject requestBullet()
    { 
        for(int i = 0; i < bulletList.Count; i++)
        {
            if (!bulletList[i].activeInHierarchy)
            {
                bulletList[i].SetActive(true);
                return bulletList[i];
            }
        }
        addBulletsToPool(1); // If no inactive bullets are available, add one more to the pool
        bulletList[bulletList.Count - 1].SetActive(true);
        return bulletList[bulletList.Count - 1];
    }
}
