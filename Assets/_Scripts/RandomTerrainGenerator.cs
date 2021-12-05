using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTerrainGenerator : MonoBehaviour
{
    private Vector3 terrainSpawnPoint;
    private Vector3 endPoint;

    private GameObject tmpCurrentTerrainModule;
    private GameObject tmpTerrainModuleToDeactivate;

    private bool isTimeToWorkWithList = false;

    private int initialTerrainModulesNumber = 14;

    // dovra essere sostituita con una variabile speed globale che si incrementa con il passare del tempo
    [SerializeField] Transform TerrainAncorPoint;


    void Start()
    {

        //terrainSpawnPoint = GameManager.instance.GetTerrainSpawnPoint();

        terrainSpawnPoint = new Vector3(TerrainAncorPoint.position.x - 2, 0, 0);

        endPoint = terrainSpawnPoint;

        InitializeTerrain();
    }

    #region INITIALIZE GROUND
    // INIZIALIZZA LA TERRA INSTANZIANDONE I PRIMI 3 MODULI
    private void InitializeTerrain() 
    {
        //for (int i = 0; i < initialGroundsNumber; i++)
        for (int i = 0; i < 3; i++)
        {
            tmpCurrentTerrainModule = ObjectPool.instance.GetRandomTerrainModule();

            if (tmpCurrentTerrainModule)
            {
                tmpCurrentTerrainModule.transform.position = terrainSpawnPoint;
                tmpCurrentTerrainModule.SetActive(true);
                terrainSpawnPoint += Vector3.right;
            }
        }
    }

    // FINISCE DI INSTANZIARE I MODULI DI TERRA RIMANENTE, UNO AD UNO
    public void StartTerrainAnimation(Action callback)
    {
        StartCoroutine(FinishInitializeTerrain(callback));
    }
    IEnumerator FinishInitializeTerrain(Action callback)
    {
       // yield return new WaitForSeconds(0.8f);

        int index = 3;
        while (index < initialTerrainModulesNumber)
        {

            tmpCurrentTerrainModule = ObjectPool.instance.GetRandomTerrainModule();

            if (tmpCurrentTerrainModule)
            {
                tmpCurrentTerrainModule.transform.position = terrainSpawnPoint;
                tmpCurrentTerrainModule.SetActive(true);
                terrainSpawnPoint += Vector3.right;
            }

            index++;
            yield return new WaitForSeconds(0.02f);
        }
        GameManager.instance.StartGround();

        callback();
    }
    #endregion

    public void UpdateTerrain()
    {
        // per ogni figlio di 'Ground'
        for (int i = 0; i < transform.childCount; i++)
        {
            tmpCurrentTerrainModule = transform.GetChild(i).gameObject;
            // se il figlio è attivo
            if (tmpCurrentTerrainModule.activeInHierarchy)
            {
                // e se non è ancora arrivato all' 'endPoint'
                if (tmpCurrentTerrainModule.transform.position.x > endPoint.x)
                {
                    // sposta il figlio
                    tmpCurrentTerrainModule.transform.Translate(Vector3.left * GameManager.instance.speed * Time.deltaTime);
                }
                else
                {
                    // altrimenti il figlio è arrivato all' 'endPoint'
                    // quindi salvo il figlio in una variabile temporanea 'tmpGroudToDeactivate'
                    // e attivo 'isTimeToWorkWithList' che servirà una volta finito il ciclo
                    tmpTerrainModuleToDeactivate = tmpCurrentTerrainModule;
                    isTimeToWorkWithList = true;
                }
            }
        }

        //se ' isTimetoWorkWithList' è attivo
        if (isTimeToWorkWithList)
        {
            // RESETTO l'oggetto che è arrivato all' 'endPoint'
            ObjectPool.instance.DeactivateTerrainModule(tmpTerrainModuleToDeactivate);

            // INSTANZIO un nuovo oggetto casuale
            ObjectPool.instance.ActivateNewRandomTerrainModule(ref tmpCurrentTerrainModule);

            // disattivo 'isTimeToWorkWithList'
            isTimeToWorkWithList = false;
        }
    }
}
