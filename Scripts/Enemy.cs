using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour {

    public float speed = 1;
    public float hp = 150;
    public int value = 50;
    private float totalHp;
    public GameObject explosionEffect;
    private Slider HpSlider;
    private Transform[] positions;
    private int index = 0;
   

	// Use this for initialization
	void Start () {
        positions = WayPoints.positions;
        totalHp = hp;
        HpSlider = GetComponentInChildren<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
        Move();       
	}

     void Move()    
    {
        if (index > positions.Length - 1) return;
        transform.Translate((positions[index].position - transform.position).normalized * Time.deltaTime * speed);
        if (Vector3.Distance(positions[index].position, transform.position) < 0.2f)
        {
            index++;
        }
        if(index > positions.Length - 1)
        {
            ReachDestination();
        }
    }

    void ReachDestination()
    {
        GameManager.Instance.Defeat();
        GameObject.Destroy(this.gameObject);
    }

     void OnDestroy()
    {
        EnemySpawner.CountEnemyAlive--;
    }

    public void TakeDamage(float damage)
    {
        if (hp <= 0) return;
        hp -= damage;
        HpSlider.value = (float)hp / totalHp;
        if(hp <= 0)
        {
            DestroySelf();
        }
    }
    void DestroySelf()
    {
        GameObject effect = GameObject.Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(effect, 1);
        Destroy(this.gameObject);
        GameObject.Find("GameManager").GetComponent<BuildManager>().ChangeMoney(value);
    }
}
