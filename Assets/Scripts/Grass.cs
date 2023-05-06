using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : Terrain
{
    //  Deklarasi agar Prefabs Tree dapat digunakan di script Grass ini
    [SerializeField] List<GameObject> treePreFabslist;
    [SerializeField, Range(0, 1)] float treeProbability;


    public void SetTreePercentage(float newProbability)
    {
        this.treeProbability = Mathf.Clamp01(newProbability);
    }

    //  Override fungsi dari Generate untuk menambahkan pohon pada grass tiles
    public override void Generate(int tileSize)
    {
        //  Base code dari fungsi Generate() akan dijalankan terlebih dahulu
        base.Generate(tileSize);

        //  Buat script untuk menampilkan pohon di grass tiles ini
        // Pohon akan di tampilkan dari 1/3 jumlah total tileSize.
        var treeCount = Mathf.FloorToInt((float)tileSize * treeProbability);

        //  Code untuk membuat posisi dari pohon yang akan di tampilkan
        //  dengan posisi yang random
        var limit = Mathf.FloorToInt((float)tileSize / 2);

        List<int> emptyPosition = new List<int>();

        for (int i = -limit; i <= limit; i++)
        {
            emptyPosition.Add(i);
        }

        for (int i = 0; i < treeCount; i++)
        {

            //  Code untuk memilih posisi kosong pada bagian tile rumput
            //  untuk memunculkan pohon secara random
            var randomIndex = Random.Range(0, emptyPosition.Count);
            var pos = emptyPosition[randomIndex];

            //  Posisi yang sudah ada pohonnya akan dihapus dalam daftar list posisi
            //  Agar tidak terjadi munculnya 2 pohon dalam 1 tile
            emptyPosition.RemoveAt(randomIndex);

            SpawnRandomTree(pos);
        }


        //  Melakukan generate tree di bagian deadzone kanan dan kiri game
        SpawnRandomTree(-limit - 1);
        SpawnRandomTree(limit + 1);
    }


    private void SpawnRandomTree (int pos)
    {
        //  Pilih prefabs pohon secara random
            var randomIndex = Random.Range(0, treePreFabslist.Count);
            var treePrefabs = treePreFabslist[randomIndex];


            //  Set pohon ke posisi yang terpilih
            var tree = Instantiate(treePrefabs, new Vector3(pos, 0, this.transform.position.z), Quaternion.identity, transform);
    }
}
