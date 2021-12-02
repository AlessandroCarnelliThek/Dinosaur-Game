using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGroundGenerator : MonoBehaviour
{
    private Vector3 spawnPoint;
    private Vector3 endPoint;

    private GameObject tmp;
    private GameObject tmpGroundToDeactivate;

    private bool isTimeToWorkWithList = false;

    private int initialGroundsNumber = 14;

    // dovra essere sostituita con una variabile speed globale che si incrementa con il passare del tempo
    [SerializeField] float speed = 4;

    void Start()
    {
        spawnPoint = GameManager.instance.GetSpawnPoint();
        endPoint = GameManager.instance.GetEndPoint();

        InitializeGround();
    }

    #region INITIALIZE GROUND
    // INIZIALIZZA LA TERRA INSTANZIANDONE I PRIMI 3 MODULI
    private void InitializeGround() 
    {
        //for (int i = 0; i < initialGroundsNumber; i++)
        for (int i = 0; i < 3; i++)
        {
            tmp = ObjectPool.instance.GetRandomGround();

            if (tmp)
            {
                tmp.transform.position = spawnPoint;
                tmp.SetActive(true);
                spawnPoint += Vector3.right;
            }
        }
    }

    // FINISCE DI INSTANZIARE I MODULI DI TERRA RIMANENTE, UNO AD UNO
    public void StartGroundAnimation(Action callback)
    {
        StartCoroutine(FinishInitializingGround(callback));
    }
    IEnumerator FinishInitializingGround(Action callback)
    {
        yield return new WaitForSeconds(0.5f);

        int index = 3;
        while (index < initialGroundsNumber)
        {

            tmp = ObjectPool.instance.GetRandomGround();

            if (tmp)
            {
                tmp.transform.position = spawnPoint;
                tmp.SetActive(true);
                spawnPoint += Vector3.right;
            }

            index++;
            yield return new WaitForSeconds(0.02f);
        }
        GameManager.instance.StartGround();

        callback();
    }
    #endregion

    public void UpdateGround()
    {
        // per ogni figlio di 'Ground'
        for (int i = 0; i < transform.childCount; i++)
        {
            tmp = transform.GetChild(i).gameObject;
            // se il figlio è attivo
            if (tmp.activeInHierarchy)
            {
                // e se non è ancora arrivato all' 'endPoint'
                if (tmp.transform.position.x > endPoint.x)
                {
                    // sposta il figlio
                    tmp.transform.Translate(Vector3.left * speed * Time.deltaTime);
                }
                else
                {
                    // altrimenti il figlio è arrivato all' 'endPoint'
                    // quindi salvo il figlio in una variabile temporanea 'tmpGroudToDeactivate'
                    // e attivo 'isTimeToWorkWithList' che servirà una volta finito il ciclo
                    tmpGroundToDeactivate = tmp;
                    isTimeToWorkWithList = true;
                }
            }
        }

        //se ' isTimetoWorkWithList' è attivo
        if (isTimeToWorkWithList)
        {
            // RESETTO l'oggetto che è arrivato all' 'endPoint'
            ObjectPool.instance.DeactivateGround(tmpGroundToDeactivate);

            // INSTANZIO un nuovo oggetto casuale
            ObjectPool.instance.ActivateNewGround(ref tmp);

            // disattivo 'isTimeToWorkWithList'
            isTimeToWorkWithList = false;
        }
    }
}
