using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomDesignLoader : MonoBehaviour
{
   
    [SerializeField] private RuleTile solidRuleTile;
    [SerializeField] private RuleTile platformRuleTile;
    [SerializeField] private Tile portalRight;
    [SerializeField] private Tile portalUp;
    [SerializeField] private Tile portalLeft;
    [SerializeField] private Tile portalDown;

    [SerializeField] private GameObject chest;

    private Tilemap tilemap;
    private TileList tileList = new TileList();
    private bool[] roomDirInfo = new bool[4];
    private RoomPortal[] roomPortalObjs = new RoomPortal[4];


    public void Initialize(bool[] dirs, RoomPortal[] roomPortals , TileList tilelist)
    {
        tileList = tilelist;
        roomDirInfo = dirs;
        tilemap = GetComponent<Tilemap>();
        roomPortalObjs = roomPortals;
    }

    private void Start()
    {
        PlaceTiles();
        PlaceChest();
    }

    public void PlaceTiles()
    {
        foreach (TileInfo tileinfo in tileList.TileInfo)
        {
            RuleTile targetRuleTile = null;
            Tile targetPortalTile = null;
            switch(tileinfo.TileType)
            {
                case 0: targetPortalTile = portalRight; break;
                case 1: targetPortalTile = portalUp; break;
                case 2: targetPortalTile = portalLeft; break;
                case 3: targetPortalTile = portalDown; break;
                case 4: targetRuleTile = solidRuleTile; break;
                case 5: targetRuleTile = platformRuleTile; break;
            }
            Vector3Int tilePos = new Vector3Int(tileinfo.TileX,tileinfo.TileY, 0);

            if (targetRuleTile != null && tilemap.GetTile(tilePos) == null)
            {
                tilemap.SetTile(tilePos, targetRuleTile);
            }
            
            else if (targetPortalTile != null)
            {
                if (!roomDirInfo[tileinfo.TileType])
                {
                    for (int iy = -1; iy < 2; iy++)
                    {
                        for (int ix = -1; ix < 2; ix++)
                        {
                            var pos = tilePos + new Vector3Int(ix, iy);
                            tilemap.SetTile(pos, solidRuleTile);
                        }
                    }
                }
                else
                {
                    roomPortalObjs[tileinfo.TileType].transform.localPosition = (Vector3)tilePos + new Vector3(0.5f, 0.5f);
                    roomPortalObjs[tileinfo.TileType].transform.localScale += Vector3.right * 2.0f;
                }
            }
            
        }
    }

    public void PlaceChest()
    {
        List<TileInfo> tilelist = new List<TileInfo>();
        if (Random.Range(0, 2) == 0)
        {
            foreach(var tileinfo in tileList.TileInfo)
            {
                var breakable = false;
                if (tileinfo.TileType == 4 && tileinfo.TileY < 19)
                {
                    for (int iy = 0; iy <= 3; iy++)
                    {
                        for (int ix = -2; ix <= 2; ix++)
                        {
                            var tilepos = new Vector3Int(tileinfo.TileX + ix, tileinfo.TileY + iy);
                            if(iy == 0)
                            {
                                if (tilemap.GetTile(tilepos) == null)
                                {
                                    breakable = true;
                                    break;
                                }
                            }
                            else
                            {
                                if (tilemap.GetTile(tilepos) != null)
                                {
                                    breakable = true;
                                    break;
                                }
                            }
                        }
                        if (breakable) { break; }
                    }
                }
                else
                {
                    
                    breakable = true;
                }

                if(!breakable)
                {
                    tilelist.Add(tileinfo);
                }
            }
            var targetTile = tilelist[Random.Range(0, tilelist.Count - 1)];
            var chestobj = Instantiate(chest, transform);
            chestobj.transform.localPosition = new Vector3(targetTile.TileX + 0.5f, targetTile.TileY + 1.5f);
        }
    }
}
