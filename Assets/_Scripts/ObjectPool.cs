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

    #region GROUND
    [SerializeField] GameObject[] GroundPrefabs;
    public List<GameObject> GroundInstanceList;
    public List<GameObject> GroundInSceneInstanceList;

    private int groundsToCreate;

    private int randomGroundIndex;
    private int tmpGroundToDeactivateIndex;
    private Vector3 accurateGroundSpawnPointPosition;
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
        groundsToCreate = GroundPrefabs.Length * 2;
        cactusToCreate = CactusPrefabs.Length * 2;
        
        // inizializzazione liste:
        GroundInstanceList = new List<GameObject>();
        GroundInSceneInstanceList = new List<GameObject>();

        // instanziamento oggetti:
        GameObject tmp;
        int index = 0;

        // Ground
        for (int i = 0; i < groundsToCreate; i++)
        {
            InstantiateGround(out tmp, ref index);
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


    #region GROUND METHOD
    private void InstantiateGround(out GameObject tmp, ref int index)
    {
        // Creazione
        tmp = Instantiate(GroundPrefabs[index]);
        // Assegnazione al parent 'Ground'
        tmp.transform.parent = GameObject.Find("RandomGroundGenerator").transform;
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

    public GameObject GetRandomGround()
    {
        // genero un index casuale 'randomIndex'
        randomGroundIndex = (int)Mathf.Round(Random.Range(0, (GroundInstanceList.Count)));

        // aggiungo l'oggetto con index 'randomIndex' di GroundInstamceList
        // alla lista GroundInSceneInstanceList
        GroundInSceneInstanceList.Add(GroundInstanceList[randomGroundIndex]);

        // rimuovo l'oggetto appena copiato da GroundInstanceList
        GroundInstanceList.RemoveAt(randomGroundIndex);

        return GroundInSceneInstanceList[GroundInSceneInstanceList.Count - 1];
    }
    public Vector3 GetAccurateGroundSpawnPointPosition()
    {
        return new Vector3(GroundInSceneInstanceList[GroundInSceneInstanceList.Count - 1].transform.position.x + 1, 0, 0);
    }

    public void DeactivateGround(GameObject tmpGroundToDeactivate)
    {
        // aggiungo 'tmpGroundToDeactivate' alla lista delle 'instanze-inattive' 
        GroundInstanceList.Add(tmpGroundToDeactivate);

        // rimuovo 'tmpGroundToDeactivate' dalla lista delle 'instanze-attive'
        // so che per forza di cose l'index sarà 0, quindi posso evitare di cercare ogni volta l'index associato al
        // oggetto 'tmpGroundToDeactivate'
        tmpGroundToDeactivateIndex = 0;
        //tmpGroundToDeactivateIndex = GroundInSceneInstanceList.Find(x => x.name.Contains(tmp.name));
        //tmpGroundToDeactivateIndex = GroundInSceneInstanceList.FindIndex(x => x.name.Contains(tmpGroundToDeactivate.name));
        GroundInSceneInstanceList.RemoveAt(tmpGroundToDeactivateIndex);
        // disattivo 'tmpGroundToDeactivate'
        tmpGroundToDeactivate.SetActive(false);
    }
    public void ActivateNewGround(ref GameObject tmp)
    {
        // salvo in 'accurateSpawnPointPosition' la posizione dell'ultimo oggetto della lista
        // delle 'instanze-attive' correggendolo di + 1 sull'asse delle 'x'
        // NOTA[DEVO SALVARE QUESTA VARIABILE PRIMA DI INSTANZIARE UN NUOVO OGGETTO]
        accurateGroundSpawnPointPosition = GetAccurateGroundSpawnPointPosition();

        // INSTANZIO un nuovo oggetto casuale
        tmp = GetRandomGround();
        // lo posiziono nella posizione salvata in precedenza in 'accurateGroundSpawnPointPosition'
        tmp.transform.position = accurateGroundSpawnPointPosition;
        // e lo attivo
        tmp.SetActive(true);
    }


    #endregion


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
            randomGroundIndex = (int)Mathf.Round(Random.Range(0, (CactusInstanceList.Count)));
            if (CactusInstanceList[randomGroundIndex].activeInHierarchy) { continue; }
            cactusFound = true;
        }
        return CactusInstanceList[randomGroundIndex];
    }
}

