using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    [SerializeField] public GameObject spawnPoint;

    [SerializeField] GameObject[] GroundPrefabs;
    [SerializeField] GameObject[] CactusPrefabs;
    //[SerializeField] GameObject EnemyPrefab;
    //[SerializeField] GameObject CloudPrefab;

    public List<GameObject> GroundInstanceList;
    public List<GameObject> GroundInSceneInstanceList;
    public List<GameObject> CactusInstanceList;
    //public List<GameObject> EnemyInstanceList;
    //public List<GameObject> CloudsInstanceList;
    private int groundsToCreate;
    private int cactusToCreate;
    //private int enemiesToCreate;
    //private int cloudToCreate;
    //private bool objectFound = false;
    //private bool cactusFound = false;

    int index = 0;

    void Awake()
    {
        // singleton
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        groundsToCreate = GroundPrefabs.Length * 2;
        cactusToCreate = CactusPrefabs.Length * 2;

        GroundInstanceList = new List<GameObject>();
        GroundInSceneInstanceList = new List<GameObject>();

        GameObject tmp;

        // instanziamento oggetti:
        // Ground
        index = 0;
        for (int i = 0; i < groundsToCreate; i++)
        {
            InstantiateGround(out tmp);
        }

        // Cactus
        index = 0;
        for (int i = 0; i < cactusToCreate; i++)
        {
            InstantiateCactus(out tmp);
        }

    }
    private void InstantiateGround(out GameObject tmp)
    {
        // Creazione
        tmp = Instantiate(GroundPrefabs[index]);
        // Assegnazione al parent 'Ground'
        tmp.transform.parent = GameObject.Find("Ground").transform;
        // Inizializzazione
        //----------------------------------


        //----------------------------------
        // Disattivazione
        tmp.SetActive(false);
        // Inserimento nella lista
        GroundInstanceList.Add(tmp);


        index++;
        if (index == GroundPrefabs.Length)
        {
            index = 0;
        }
    }

    private void InstantiateCactus(out GameObject tmp)
    {
        // Creazione
        tmp = Instantiate(CactusPrefabs[index]);
        // Assegnazione al parent 'Ground'
        tmp.transform.parent = GameObject.Find("Cactus").transform;
        // Inizializzazione
        //----------------------------------



        //----------------------------------
        // Disattivazione
        tmp.SetActive(false);
        // Inserimento nella lista
        CactusInstanceList.Add(tmp);


        index++;
        if (index == CactusPrefabs.Length)
        {
            index = 0;
        }
    }


    int randomIndex;

    // GROUND METHOD
    public void AddObjectToGroundInstanceList(GameObject tmp)
    {
        GroundInstanceList.Add(tmp);
    }
    public void RemoveObjectFromGroundInSceneInstanceList(GameObject tmp)
    {
        int tmpIndex;
        //GroundInSceneInstanceList.Find(x => x.name.Contains(tmp.name));
        tmpIndex = GroundInSceneInstanceList.FindIndex(x => x.name.Contains(tmp.name));
        GroundInSceneInstanceList.RemoveAt(tmpIndex);

    }

    public GameObject GetRandomGround()
    {
        // genero un index casuale 'randomIndex'
        randomIndex = (int)Mathf.Round(Random.Range(0, (GroundInstanceList.Count)));

        // aggiungo l'oggetto con index 'randomIndex' di GroundInstamceList
        // alla lista GroundInSceneInstanceList
        GroundInSceneInstanceList.Add(GroundInstanceList[randomIndex]);

        // rimuovo l'oggetto appena copiato da GroundInstanceList
        GroundInstanceList.RemoveAt(randomIndex);
        return GroundInSceneInstanceList[GroundInSceneInstanceList.Count - 1];
    }
    public Vector3 GetAccurateGroundSpawnPointPosition()
    {
        return new Vector3(GroundInSceneInstanceList[GroundInSceneInstanceList.Count - 1].transform.position.x + 1, 0, 0);
    }


    // CACTUS METHOD
    public GameObject GetRandomCactus()
    {
        bool cactusFound = false;

        while (!cactusFound)
        {
            randomIndex = (int)Mathf.Round(Random.Range(0, (CactusInstanceList.Count)));
            if (CactusInstanceList[randomIndex].activeInHierarchy) { continue; }
            cactusFound = true;
        }
        return CactusInstanceList[randomIndex];
    }





    /*
        public GameObject GetGround()
    {
        for (int i = 0; i < groundsToCreate; i++)
        {
            if (GroundInstanceList[i].activeInHierarchy) { continue; }
            return GroundInstanceList[i];
        }
        return null;
    }
    */

    /*
    public GameObject GetRandomGround()
    {
        objectFound = false;

        while (!objectFound)
        {
            int randomIndex = (int)Mathf.Round(Random.Range(0, (groundsToCreate)));
            if (GroundInstanceList[randomIndex].activeInHierarchy) { continue; }
            objectFound = true;
            return GroundInstanceList[randomIndex];
        }
        return null;
    }
    */

}

