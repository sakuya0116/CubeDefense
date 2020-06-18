using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour {

    public TurretData laserTurretData;
    public TurretData missileTurretData;
    public TurretData standardTurretData;

    private TurretData selectedTurretData; // for game UI

    private MapCube selectedMapCube; //for upgradeTurret UI

    public Text moneyText;
    public Animator moneyAnimator;
    public int money = 100;

    public GameObject upgradeCanvas;
    public Button buttonUpgrade;
    private Animator upgradeCanvasAnimator;

    void Start()
    {
        upgradeCanvasAnimator = upgradeCanvas.GetComponent<Animator>();
    }

    public void ChangeMoney(int change = 0)
    {
        money += change;        
        moneyText.text = money + "$";
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(EventSystem.current.IsPointerOverGameObject()==false)
           {
                //Start to build turret
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                bool isCollider=Physics.Raycast(ray,out hit, 100, LayerMask.GetMask("MapCube"));                
                if(isCollider)
                {
                    MapCube mapCube = hit.collider.GetComponent<MapCube>();                   
                    if (selectedTurretData != null && mapCube.turretGo == null)
                    {
                        //Create turret
                        if (money >= selectedTurretData.cost)
                        {                            
                            ChangeMoney(-selectedTurretData.cost);
                            mapCube.BuildTurret(selectedTurretData);                           
                        }
                        else
                        {
                            //Hint   Money isn't enough                            
                            moneyAnimator.SetTrigger("Flicker");
                        }
                    }
                    else if(mapCube.turretGo != null)
                    {
                        //Update turret                       
                        if (mapCube == selectedMapCube && upgradeCanvas.activeInHierarchy)
                        {
                            StartCoroutine(HideUpgradeUI());
                        }
                        else
                        {                           
                            ShowUpgradeUI(mapCube.UIPos.transform.position, mapCube.isUpgraded);
                        }
                        selectedMapCube = mapCube;
                    }
                }
            }
        }
    }

    public void OnLaserSelected(bool isOn)
    {
        if(isOn)
        {
            selectedTurretData = laserTurretData;
        }
    }
    public void OnMissileSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = missileTurretData;
        }
    }
    public void OnStandardSelected(bool isOn)
    {
        if (isOn)
        {
            selectedTurretData = standardTurretData;
        }
    }

    void ShowUpgradeUI(Vector3 pos, bool isDisableUpgrade = false)
    {
        StopCoroutine("ideUpgradeUI");
        upgradeCanvas.SetActive(false);
        upgradeCanvas.SetActive(true);
        upgradeCanvas.transform.position = pos;
        buttonUpgrade.interactable = !isDisableUpgrade;
    }
    IEnumerator HideUpgradeUI()
    {
        upgradeCanvasAnimator.SetTrigger("Hide");
        yield return new WaitForSeconds(0.2f);
        upgradeCanvas.SetActive(false);
    }

    public void OnUpgradeButtonDown()
    {
        if (money >= selectedMapCube.turretData.costUpgraded)
        {
            ChangeMoney(-selectedMapCube.turretData.costUpgraded);
            selectedMapCube.UpgradeTurret();
        }    
        else
        {
            moneyAnimator.SetTrigger("Flicker");
        }    
        StartCoroutine(HideUpgradeUI());
    }

    public void OnRemoveButtonDown()
    {
        ChangeMoney(selectedMapCube.turretData.cost / 2);
        selectedMapCube.DestroyTurret();
        StartCoroutine(HideUpgradeUI());
    }
}
