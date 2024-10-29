using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    private int size = 25;

    private GameObject[,] floorTile;

    // Start is called before the first frame update
    void Start()
    {
        floorTile = new GameObject[size, size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float row = (2.50f * (float)y + 1.25f) - 31.25f;
                float col = (2.50f * (float)x + 1.25f) - 31.25f;

                GameObject tile = Instantiate(gameData.basicFloorTile, new Vector3(col, 0.0f, row), Quaternion.identity);
                floorTile[x, y] = tile;

                tile.GetComponent<FloorTileCntrl>().X = x;
                tile.GetComponent<FloorTileCntrl>().Y = y;
            }
        }


        for (int x = 0, y1=0, y2=24; x < size; x++)
        {
            float row1 = (2.50f * (float)y1 + 1.25f) - 31.25f;
            float row2 = (2.50f * (float)y2 + 1.25f) - 31.25f;
            float col = (2.50f * (float)x + 1.25f) - 31.25f;

            Instantiate(gameData.rails[PickaRail()], new Vector3(col, 0.0f, row1 - 1.25f), Quaternion.identity);
            Instantiate(gameData.rails[PickaRail()], new Vector3(col, 0.0f, row2 + 1.25f), Quaternion.identity);
        }

        for (int y = 0, x1 = 0, x2 = 24; y < size; y++)
        {
            float col1 = (2.50f * (float)x1 + 1.25f) - 31.25f;
            float col2 = (2.50f * (float)x2 + 1.25f) - 31.25f;
            float row = (2.50f * (float)y + 1.25f) - 31.25f;

            GameObject rail1 = Instantiate(gameData.rails[PickaRail()], new Vector3(col1 - 1.25f, 0.0f, row), Quaternion.identity);
            rail1.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f), Space.Self);
            GameObject rail2 = Instantiate(gameData.rails[PickaRail()], new Vector3(col2 + 1.25f, 0.0f, row), Quaternion.identity);
            rail2.transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f), Space.Self);
        }

        int runeCount = 0;

        while (runeCount < 5)
        {
            int x = Random.Range(0, 25);
            int y = Random.Range(0, 25);

            FloorTileCntrl tile = floorTile[x, y].GetComponent<FloorTileCntrl>();

            if (!tile.IsRuneTile)
            {
                Vector3 position = tile.MakeRuneTile();
                Instantiate(gameData.runeTiles[runeCount], position, Quaternion.identity);
                runeCount++;
                floorTile[x, y].SetActive(false);
            } 
        }

        for (int i = 0; i < gameData.nGrassTiles; i++)
        {
            int n = Random.Range(0, gameData.grassTiles.Length);
            float x = Random.Range(-31.25f, 31.25f);
            float y = Random.Range(-31.25f, 31.25f);

            Instantiate(gameData.grassTiles[n], new Vector3(x, 0.0f, y), Quaternion.identity);
        }

        for (int i = 0; i < gameData.nMushRooms; i++) 
        {
            int n = Random.Range(0, gameData.mushRoomTiles.Length);
            float x = Random.Range(-31.25f, 31.25f);
            float y = Random.Range(-31.25f, 31.25f);

            Vector3 position = new Vector3(x, 0.0f, y);

            Instantiate(gameData.mushRoomTiles[n], position, Quaternion.identity);

            for (int g = 0; g < 10; g++)
            {
                int m = Random.Range(0, gameData.grassTiles.Length);

                Vector2 pos = Random.insideUnitCircle * 2.0f;
                Debug.Log($"Pos: {pos}");
                Vector3 posOffset = new Vector3(position.x + pos.x, 0.0f, position.z + pos.y);

                Instantiate(gameData.grassTiles[m], posOffset, Quaternion.identity);
            }
        }

        for (int i = 0; i < gameData.nStatueTiles; i++)
        {
            int n = Random.Range(0, gameData.statueTiles.Length);
            float x = Random.Range(-31.25f, 31.25f);
            float y = Random.Range(-31.25f, 31.25f);

            GameObject statue = Instantiate(gameData.statueTiles[n], new Vector3(x, 0.0f, y), Quaternion.identity);
            statue.transform.Rotate(new Vector3(0.0f, Random.Range(0f, 180.0f), 0.0f));
        }
    }

    private int PickaRail()
    {
        return (Random.Range(0, gameData.rails.Length));
    }
}
