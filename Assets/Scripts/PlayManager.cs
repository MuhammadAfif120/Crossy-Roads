using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    [SerializeField] List<Terrain> terrainList;

    [SerializeField] List<Coin> coinList;

    [SerializeField] int initialGrassCount;
    [SerializeField] int horizontalSize;

    [SerializeField] int backViewDistancePos;

    [SerializeField] int forwadViewDistancePos;

    //  List untuk gabungan antara prefabs road dan grass
    // private List<Terrain> terrainList;

    Dictionary<int, Terrain> activeTerrainDict = new Dictionary<int, Terrain>(20);

    [SerializeField] private int travelDistance;

    [SerializeField] private int coin;

    [SerializeField] AudioSource chickenDeadSound;

    public UnityEvent<int, int> OnUpdateTerrainLimit;

    public UnityEvent<int> onScoreUpdate;

    private void Start()
    {

        Screen.SetResolution(1080, 1920, true);

        //  Membuat grass tile kedepan dan kebelakang sebagai permulaan awal game (-4 ---- 4)
        for (int zPos = backViewDistancePos; zPos < initialGrassCount; zPos++)
        {
            var terrain = Instantiate(terrainList[0]);
            terrain.transform.position = new Vector3(0, 0, zPos);

            if (terrain is Grass grass)
            {
                //  Code untuk tidak memunculkan pohon di 5 grass tile awal permainan
                //  Dan code untuk memunculkan pohon di posisi belakang pemain saat dalam fase start game
                grass.SetTreePercentage(zPos < -1 ? 1 : 0);

            }

            terrain.Generate(horizontalSize);

            activeTerrainDict[zPos] = terrain;
        }


        //  Membuat tile random kedepan sejumlah "forwardViewDistance"
        //  Dengan memanggil terrainList untuk random tilesnya.
        for (int zPos = initialGrassCount; zPos < forwadViewDistancePos; zPos++)
        {
            SpawnRandomTerrain(zPos);
        }

        OnUpdateTerrainLimit.Invoke(horizontalSize, travelDistance + backViewDistancePos);
    }


    private Terrain SpawnRandomTerrain(int zPos)
    {
        //  Variabel sementara untuk menyimpan terrain yang akan di check
        //  jumlah pemakaiannya
        Terrain comparatorTerrain = null;

        int randomIndex;

        for (int z = -1; z >= -3; z--)
        {
            var checkPos = zPos + z;

            if (comparatorTerrain == null)
            {
                comparatorTerrain = activeTerrainDict[checkPos];
                continue;
            }
            else if (comparatorTerrain.GetType() != activeTerrainDict[checkPos].GetType())
            {
                //  Membuat variabel temporary dengan memasukkan random range
                //  untuk memunculkan tiles road dan grass secara random
                randomIndex = Random.Range(0, terrainList.Count);

                return spawnTerrain(terrainList[randomIndex], zPos);
            }
            else
            {
                continue;
            }
        }

        //  Membuat duplikasi list terrain dengan nama baru
        //  agar dapat digunakan secara terpisah
        var candidateTerrain = new List<Terrain>(terrainList);

        //  Code untuk menghilangkan prefab tile terrain
        //  ketika sudah dipakai semisal 3x berturut - turut
        for (int i = 0; i < candidateTerrain.Count; i++)
        {

            //  Menghilangkan tile prefab dari list terrain
            if (comparatorTerrain.GetType() == candidateTerrain[i].GetType())
            {
                candidateTerrain.Remove(candidateTerrain[i]);
                break;
            }
        }

        //  Membuat variabel temporary dengan memasukkan random range
        //  untuk memunculkan tiles road dan grass secara random
        randomIndex = Random.Range(0, candidateTerrain.Count);

        return spawnTerrain(candidateTerrain[randomIndex], zPos);
    }


    public Terrain spawnTerrain(Terrain terrain, int zPos)
    {
        //  Memanggil prefabs yang ada pada terrainList menggunakan "Instantiate"
        //  dengan index "randomIndex" yang telah dibuat di line sebelumnya
        terrain = Instantiate(terrain);
        terrain.transform.position = new Vector3(0, 0, zPos);
        terrain.Generate(horizontalSize);
        activeTerrainDict[zPos] = terrain;

        SpawnCoin(horizontalSize, zPos);

        return terrain;
    }



    public Coin SpawnCoin(int horizontalSize, int zPos, float probabilityCoin = 0.2f)
    {

        if (probabilityCoin == 0)
        {
            return null;
        }


        List<Vector3> spawnPosCandidateList = new List<Vector3>();
        for (int x = -horizontalSize / 2; x <= horizontalSize / 2; x++)
        {
            var spawnPos = new Vector3(x, 0, zPos);

            if (Tree.AllPositions.Contains(spawnPos) == false)
            {
                spawnPosCandidateList.Add(spawnPos);
            }
        }


        if (probabilityCoin >= Random.value)
        {
            var index = Random.Range(0, coinList.Count);
            var spawnPosIndex = Random.Range(0, spawnPosCandidateList.Count);

            return Instantiate(coinList[index], spawnPosCandidateList[spawnPosIndex], Quaternion.identity);
        }

        return null;
    }



    //  Code untuk mengecek seberapa jauh karakter bergerak
    //  serta code untuk perhitungan score seberapa jauh target bergerak
    public void UpdateTravelDistance(Vector3 targetPosition)
    {
        //  nilai akan bertambah sesuai nilai travel distance sekarang
        //  jika karakter bergerak mundur, nilai tidak akan berkurang
        if (targetPosition.z > travelDistance)
        {
            travelDistance = Mathf.CeilToInt(targetPosition.z);
            UpdateTerrain();

            onScoreUpdate.Invoke(GetScore());
        }
    }

    public void AddCoin(int value = 1)
    {
        this.coin += value;
        onScoreUpdate.Invoke(GetScore());
    }


    private int GetScore()
    {
        return travelDistance + coin;
    }


    public void UpdateTerrain()
    {
        var destroyPos = travelDistance - 1 + backViewDistancePos;
        Destroy(activeTerrainDict[destroyPos].gameObject);
        activeTerrainDict.Remove(destroyPos);

        var spawnPosition = travelDistance - 1 + forwadViewDistancePos;
        SpawnRandomTerrain(spawnPosition);

        OnUpdateTerrainLimit.Invoke(horizontalSize, travelDistance + backViewDistancePos);
    }


    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

}