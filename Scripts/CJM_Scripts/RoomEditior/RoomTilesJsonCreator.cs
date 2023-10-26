using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Tilemaps;
using System;
using UnityEngine.SceneManagement;

[Serializable]
public class TileInfo
{
    public int TileX;
    public int TileY;
    public int TileType;
    public TileInfo(int tileX, int tileY, int tileType)
    {
        TileX = tileX;
        TileY = tileY;
        TileType = tileType;
    }
}


public class RoomTilesJsonCreator : MonoBehaviour
{
    /*
     * 00 portal right
     * 01 portal up
     * 02 portal left
     * 03 portal down
     * 04 solid tile
     * 05 platform tile
     * 06 small ground enemy
     * 07 small fly enemy
     */
    private Color[] colors = new Color[8];
    private List<TileInfo> tileList = new List<TileInfo>();
    [SerializeField] private TileBase[] myTiles;
    [SerializeField] private Tilemap targetTileMap;
    [SerializeField] private Sprite roomImage;

    private int fileNum = 0;

    private enum tileStyle
    {
        SolidTile = 0,
    }

    private void Awake()
    {
        var portalColorValue = 192.0f / 255.0f;
        targetTileMap = GetComponent<Tilemap>();

        colors[0] = new Color(portalColorValue, 0.0f, 0.0f);
        colors[1] = new Color(portalColorValue, portalColorValue, 0.0f);
        colors[2] = new Color(0.0f, portalColorValue, 0.0f);
        colors[3] = new Color(0.0f, portalColorValue, portalColorValue);
        colors[4] = new Color(0.0f, 0.0f, 0.0f);
        colors[5] = new Color(1.0f, 0.0f, 1.0f);
        colors[6] = new Color(1.0f, 0, 0);
        colors[7] = new Color(1.0f, 1.0f, 0);
    }
    private void Update()
    {
        var imageFileLocation = $"{Application.dataPath}/Resources/CJM/Sprites/RoomImage/4dir{fileNum}.png";
        Debug.Log(imageFileLocation);
        FileInfo imageFileInfo = new FileInfo(imageFileLocation);
        tileList.Clear();
        if (imageFileInfo.Exists)
        {
            roomImage = Resources.Load<Sprite>($"CJM/Sprites/RoomImage/4dir{fileNum}");
            targetTileMap.ClearAllTiles();
            if (roomImage != null)
            {
                for (int iy = 0; iy <= roomImage.texture.height; iy++)
                {
                    for (int ix = 0; ix <= roomImage.texture.width; ix++)
                    {
                        Color pixelcolor = roomImage.texture.GetPixel(ix, iy);
                        for(int i = 0; i < myTiles.Length; i++)
                        {
                            if (pixelcolor == colors[i])
                            {
                                targetTileMap.SetTile(new Vector3Int(ix - 24, iy - 24, 0), myTiles[i]);
                            }
                        }
                    }
                }
            }

            var enableTilePos = targetTileMap.cellBounds.allPositionsWithin;
            foreach (var tilepos in enableTilePos)
            {
                var tile = targetTileMap.GetTile(tilepos);
                if (tile != null)
                {
                    tileList.Add(new TileInfo(tilepos.x, tilepos.y, GetTileType(tile.name)));
                }
            }
            SaveTilesInfo();
            fileNum++;
        }
        else
        {
            SceneManager.LoadScene(1);
        }
    }

    private int GetTileType(string tileName)
    {
        for(int i = 0; i < myTiles.Length; i++)
        {
            if(tileName == myTiles[i].name)
            {
                return i;
            }
        }
        return -1;
    }
    private void SaveTilesInfo()
    {
        Debug.Log("StartWrite");
        while (true)
        {
            Directory.CreateDirectory($"{Application.persistentDataPath}/MapSave");
            var fileLoc = $"{Application.persistentDataPath}/MapSave/MapData4dir{fileNum}.Json";
            FileInfo fileInfo = new FileInfo(fileLoc);
            if (!fileInfo.Exists)
            {
                File.WriteAllText(fileLoc, GetJsonFromArray(tileList));
                break;
            }
            else
            {
                break;
            }
        }
        Debug.Log($"[{fileNum}] WriteDone");
    }


    private string GetJsonFromArray(List<TileInfo> array)
    {
        RoomTiles tiles = new RoomTiles();
        tiles.TileInfo = array;
        return JsonUtility.ToJson(tiles, true);
    }
}
public class RoomTiles
{
    public List<TileInfo> TileInfo;
}