using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    private List<GameObject> enemies = new List<GameObject>();
     void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Enemy")
        {
            enemies.Add(col.gameObject);
        }
    }

     void OnTriggerExit(Collider col)
    {
        if (col.tag == "Enemy")
        {
            enemies.Remove(col.gameObject);
        }
    }

    public float attackRateTime = 1;
    private float timer = 0;

    public GameObject bulletPrefab;
    public Transform firePosition1;
    public Transform firePosition2;
    public Transform head;

    public bool useLaser = false;
    public float damageRate = 70;
    public LineRenderer laserRenderer;
    public GameObject laserEffect;

    void Start()
    {
        timer = attackRateTime;
    }

    void Update()
    {
        if (enemies.Count > 0 && enemies[0] != null)
        {
            
            Vector3 targetPosition = enemies[0].transform.position;
            targetPosition.y = head.position.y;
            head.LookAt(targetPosition);
        }
        if(useLaser == false)
        {
            timer += Time.deltaTime;        
            if (enemies.Count > 0 && timer >= attackRateTime)
            {
                timer = 0;
                Attack();
            }
        }
        else if(enemies.Count > 0)
        {
            if (laserRenderer)
            {
                if (laserRenderer.enabled == false)
                    laserRenderer.enabled = true;
            }
            if (laserEffect) laserEffect.SetActive(true);
            if (enemies[0] == null)
            {
                UpdateEnemies();
            }
            if(enemies.Count > 0)
            {
                if (laserRenderer) laserRenderer.SetPositions(new Vector3[] { firePosition1.position, enemies[0].transform.position });
                enemies[0].GetComponent<Enemy>().TakeDamage(damageRate*Time.deltaTime);
                laserEffect.transform.position = enemies[0].transform.position;
                Vector3 pos = transform.position;
                pos.y = enemies[0].transform.position.y;
                laserEffect.transform.LookAt(pos);
            }
        } 
        else
        {
            if(laserEffect) laserEffect.SetActive(false);
            if(laserRenderer) laserRenderer.enabled = false;
        }      
    }

    void Attack()
    {
        if(enemies[0] == null)
        {
            UpdateEnemies();
        }
        if(enemies.Count > 0)
        {
            GameObject bullet1 = GameObject.Instantiate(bulletPrefab, firePosition1.position, firePosition1.rotation);
            GameObject bullet2 = GameObject.Instantiate(bulletPrefab, firePosition2.position, firePosition2.rotation);
            bullet1.GetComponent<Bullet>().SetTarget(enemies[0].transform);
            bullet2.GetComponent<Bullet>().SetTarget(enemies[0].transform);
        }
        else
        {
            timer = attackRateTime;
        }
    }

    void UpdateEnemies()
    {
        //enemies.RemoveAll(null);
        List<int> emptyIndex = new List<int>();
        for (int index = 0; index < enemies.Count; index++)
        {
            if(enemies[index] == null)
            {
                emptyIndex.Add(index);
            }
        }
        for (int i = 0; i<emptyIndex.Count; i++)
        {
            enemies.RemoveAt(emptyIndex[i] + i);
        }
    }
}
