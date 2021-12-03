using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    private void MakeSingleton()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    #region TERRAIN
    [SerializeField] GameObject[] TerrainPrefabs;
    public List<GameObject> TerrainInstanceList;
    public List<GameObject> ActiveTerrainInstanceList;

    private int terrainModulesToCreate;

    private int randomTerrainModuleIndex;
    private int tmpTerrainModuleToDeactivateIndex;
    private Vector3 accurateTerrainSpawnPointPosition;
    #endregion

    #region CACTUS
    [SerializeField] GameObject[] CactusPrefabs;
    public List<GameObject> CactusInstanceList;
    private int cactusToCreate;

    #endregion

    #region ENEMY
    //[SerializeField] GameObject EnemyPrefab;
    //public List<GameObject> EnemyInstanceList;
    //private int enemiesToCreate;

    #endregion

    #region CLOUDS
    //[SerializeField] GameObject CloudPrefab;
    //public List<GameObject> CloudsInstanceList;
    //private int cloudToCreate;

    #endregion


    void Awake()
    {
        // singleton
        MakeSingleton();

        // inizializzazione numero di oggetti da creare
        terrainModulesToCreate = TerrainPrefabs.Length * 2;
        cactusToCreate = CactusPrefabs.Length * 2;
        
        // inizializzazione liste:
        TerrainInstanceList = new List<GameObject>();
        ActiveTerrainInstanceList = new List<GameObject>();

        // instanziamento oggetti:
        GameObject tmp;
        int index = 0;

        // Terrain
        for (int i = 0; i < terrainModulesToCreate; i++)
        {
            InstantiateTerrainModule(out tmp, ref index);
        }

        // Cactus
        index = 0;
        for (int i = 0; i < cactusToCreate; i++)
        {
            InstantiateCactus(out tmp, ref index);
        }

        // Enemy

        // Clouds
    }


    #region TERRAIN METHOD
    private void InstantiateTerrainModule(out GameObject tmp, ref int index)
    {
        // Creazione
        tmp = Instantiate(TerrainPrefabs[index]);
        // Assegnazione al parent 'Ground'
        tmp.transform.parent = GameObject.Find("RandomTerrainGenerator").transform;
        // Inizializzazione
        //----------------------------------


        //----------------------------------
        // Disattivazione
        tmp.SetActive(false);
        // Inserimento nella lista
        TerrainInstanceList.Add(tmp);


        index++;
        if (index == TerrainPrefabs.Length)
        {
            index = 0;
        }
    }

    public GameObject GetRandomTerrainModule()
    {
        // genero un index casuale 'randomIndex'
        randomTerrainModuleIndex = (int)Mathf.Round(Random.Range(0, (TerrainInstanceList.Count)));

        // aggiungo l'oggetto con index 'randomIndex' di GroundInstamceList
        // alla lista GroundInSceneInstanceList
        ActiveTerrainInstanceList.Add(TerrainInstanceList[randomTerrainModuleIndex]);

        // rimuovo l'oggetto appena copiato da GroundInstanceList
        TerrainInstanceList.RemoveAt(randomTerrainModuleIndex);

        return ActiveTerrainInstanceList[ActiveTerrainInstanceList.Count - 1];
    }
    public Vector3 GetAccurateTerrainSpawnPointPosition()
    {
        return new Vector3(ActiveTerrainInstanceList[ActiveTerrainInstanceList.Count - 1].transform.position.x + 1, 0, 0);
    }

    public void DeactivateTerrainModule(GameObject tmpTerrainModuleToDeactivate)
    {
        // aggiungo 'tmpGroundToDeactivate' alla lista delle 'instanze-inattive' 
        TerrainInstanceList.Add(tmpTerrainModuleToDeactivate);

        // rimuovo 'tmpGroundToDeactivate' dalla lista delle 'instanze-attive'
        // so che per forza di cose l'index sarà 0, quindi posso evitare di cercare ogni volta l'index associato al
        // oggetto 'tmpGroundToDeactivate'
        tmpTerrainModuleToDeactivateIndex = 0;
        //tmpGroundToDeactivateIndex = GroundInSceneInstanceList.Find(x => x.name.Contains(tmp.name));
        //tmpGroundToDeactivateIndex = GroundInSceneInstanceList.FindIndex(x => x.name.Contains(tmpGroundToDeactivate.name));
        ActiveTerrainInstanceList.RemoveAt(tmpTerrainModuleToDeactivateIndex);
        // disattivo 'tmpGroundToDeactivate'
        tmpTerrainModuleToDeactivate.SetActive(false);
    }
    public void ActivateNewRandomTerrainModule(ref GameObject tmpCurrentTerrainModule)
    {
        // salvo in 'accurateSpawnPointPosition' la posizione dell'ultimo oggetto della lista
        // delle 'instanze-attive' correggendolo di + 1 sull'asse delle 'x'
        // NOTA[DEVO SALVARE QUESTA VARIABILE PRIMA DI INSTANZIARE UN NUOVO OGGETTO]
        accurateTerrainSpawnPointPosition = GetAccurateTerrainSpawnPointPosition();

        // INSTANZIO un nuovo oggetto casuale
        tmpCurrentTerrainModule = GetRandomTerrainModule();
        // lo posiziono nella posizione salvata in precedenza in 'accurateGroundSpawnPointPosition'
        tmpCurrentTerrainModule.transform.position = accurateTerrainSpawnPointPosition;
        // e lo attivo
        tmpCurrentTerrainModule.SetActive(true);
    }
    #endregion

    #region CACTUS METHOD
    private void InstantiateCactus(out GameObject tmp, ref int index)
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
    // CACTUS METHOD
    public GameObject GetRandomCactus()
    {
        bool cactusFound = false;

        while (!cactusFound)
        {
            randomTerrainModuleIndex = (int)Mathf.Round(Random.Range(0, (CactusInstanceList.Count)));
            if (CactusInstanceList[randomTerrainModuleIndex].activeInHierarchy) { continue; }
            cactusFound = true;
        }
        return CactusInstanceList[randomTerrainModuleIndex];
    }
    #endregion

    #region ENEMIES METHOD
    
    #endregion

    #region CLOUDS METHOD
    
    #endregion
}

