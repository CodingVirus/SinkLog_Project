using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UIMinimap : MonoBehaviour
{
    private enum TypeofSetPixel
    {
        none,
        Solid,
        Platform,
    }
    private int nowRoomID;

    private Texture2D myTexture2D;
    private RawImage myImage;

    private int sizeMutiply = 2;

    private void Awake()
    {
        myTexture2D = new Texture2D(48 * sizeMutiply, 48 * sizeMutiply);
        myImage = GetComponent<RawImage>();
    }

    private void Update()
    {
        if(RoomManager.Instance.GetActiveRoom() != null)
        {
            if (myImage.texture == null || nowRoomID != RoomManager.Instance.GetActiveRoom().MyRoomData.RoomID)
            {
                DrawRoomTiles();
            }
        }
    }

    private void DrawRoomTiles()
    {
        var tiles = RoomManager.Instance.GetActiveRoom();
        nowRoomID = tiles.MyRoomData.RoomID;
        int size = 24;
        for (int ix = -size; ix <= size; ix++)   
        {
            for (int iy = size; iy >= -size; iy--)
            {
                var solidTile = tiles.MyTilemapObjects[0].GetComponent<Tilemap>();
                var platformTile = tiles.MyTilemapObjects[1].GetComponent<Tilemap>();
                if (solidTile.GetTile(new Vector3Int(ix, iy)))
                {
                    TextureSetPixel2x2((ix + size) * sizeMutiply, (iy + size) * sizeMutiply, TypeofSetPixel.Solid);
                }
                else if (platformTile.GetTile(new Vector3Int(ix, iy)))
                {
                    TextureSetPixel2x2((ix + size) * sizeMutiply, (iy + size) * sizeMutiply, TypeofSetPixel.Platform);
                }
                else if (platformTile.GetTile(new Vector3Int(ix, iy)) == null)
                {
                    TextureSetPixel2x2((ix + size) * sizeMutiply, (iy + size) * sizeMutiply, TypeofSetPixel.none);
                }
            }
        }
        myTexture2D.Apply();
        myImage.texture = myTexture2D;
        myImage.texture.filterMode = FilterMode.Point;
        myImage.color = new Color(1.0f, 1.0f, 1.0f, 0.55f);
    }


    private void TextureSetPixel2x2(int xpos, int ypox, TypeofSetPixel type)
    {
        Color color = (type == TypeofSetPixel.Platform || type == TypeofSetPixel.Solid) ? Color.gray : Color.white;
        myTexture2D.SetPixel(xpos, ypox, color);
        myTexture2D.SetPixel(xpos + (sizeMutiply / 2), ypox, color);

        color = (type == TypeofSetPixel.Platform || type == TypeofSetPixel.none) ? Color.white : Color.gray;
        myTexture2D.SetPixel(xpos, ypox - (sizeMutiply / 2), color);
        myTexture2D.SetPixel(xpos + (sizeMutiply / 2), ypox - (sizeMutiply / 2), color);
    }
}
