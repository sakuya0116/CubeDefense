using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapCube : MonoBehaviour {
   // [HideInInspector]
    public GameObject turretGo;
   // [HideInInspector]
    public TurretData turretData;

    //[HideInInspector]
    public bool isUpgraded = false;
    public GameObject UIPos;

    public GameObject buildEffect;
    public GameObject buildEffectPos;

    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    public void BuildTurret(TurretData turretData)
    {
        this.turretData = turretData;
        isUpgraded = false;        
        turretGo = GameObject.Instantiate(turretData.turretPrefab, transform.position, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildEffect, buildEffectPos.transform.position, Quaternion.identity);
        Destroy(effect, 1.2f);
    }

    public void UpgradeTurret()
    {
        if (isUpgraded == true) return;
        Destroy(turretGo);
        isUpgraded = true;        
        turretGo = GameObject.Instantiate(turretData.turretUpgradedPrefab, transform.position, Quaternion.identity);
        GameObject effect = GameObject.Instantiate(buildEffect, buildEffectPos.transform.position, Quaternion.identity);       
        Destroy(effect, 1.2f);
    }

    public void DestroyTurret()
    {
        Destroy(turretGo);
        isUpgraded = false;
        turretGo = null;
        turretData = null;
        GameObject effect = GameObject.Instantiate(buildEffect, buildEffectPos.transform.position, Quaternion.identity);
        Destroy(effect, 1.2f);
    }

    void OnMouseEnter()
    {
        if(turretGo == null && EventSystem.current.IsPointerOverGameObject() == false)
        {
            renderer.material.color = Color.gray;
        }
    }

    void OnMouseExit()
    {
        renderer.material.color = Color.white;
    }
}
