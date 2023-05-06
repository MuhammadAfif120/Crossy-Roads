using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Terrain : MonoBehaviour
{
    //  Deklarasi prefabs dari road tiles agar dapat di generate pada Road.cs
    [SerializeField] GameObject tilePrefabs;

    protected int horizontalSize;

    //  Fungsi Generate Tile 
    //  Ditambahkan kata virtual untuk
    //  Dapat di Override pada turunan dari Terrain ini.
    public virtual void Generate(int tileSize)
    {
        horizontalSize = tileSize;

        if (tileSize == 0)
        {
            return;
        }


        //  Membuat generate tilesize ke ganjil jika fungsi Gerenate()
        //  Diisikan genap
        if ((float)tileSize % 2 == 0)
        {
            tileSize -= 1;
        }


        int limit = Mathf.FloorToInt((float) tileSize / 2);

        for (int i = -limit; i <= limit; i++)
        {
            spawnTile(i);
        }

        //  Membuat variabel temporary untuk bagian mana saja
        //  yang akan dijadikan DeadZone
        var leftBoundaryTile = spawnTile(-limit -1);
        var rightBoundaryTile = spawnTile(limit + 1);

        //  Memasukkan variabel temporary kedalam DeadZone
        DeadZone(leftBoundaryTile);
        DeadZone(rightBoundaryTile);

    }


    private GameObject spawnTile(int xPos)
    {
        /*
                Cara kerja dibawah ini adalah tileprefabs akan dijalankan dulu
                baru disimpan kedalam var go
                Instantiate digunakan untuk clone dari prefabs
        */
        var go = Instantiate(tilePrefabs, transform);

        go.transform.localPosition = new Vector3(xPos, 0, 0);

        return go;
    }


    //  Fungsi untuk membuat Deadzone(area yang tidak dapat dilalui player) 
    //  pada setiap bagian paling kanan dan paling kiri Road tiles
    private void DeadZone(GameObject go)
    {
        var renderers = go.GetComponentsInChildren<MeshRenderer>(includeInactive: true);

        foreach (var rend in renderers)
        {
            //  Dikalikan agar warna dasar masih terlihat (sama kyk tint)
            rend.material.color *= Color.grey;
        }
    }
}
