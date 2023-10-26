using System.Collections;
using UnityEngine;

public class UIMinimapPlayerIconCreator : MonoBehaviour
{
    [SerializeField] private GameObject iconObj;
    public LayerMask UnitLayerMask;

    private void Start()
    {
        StartCoroutine(CreatingMinimapObj());
    }
    
    private IEnumerator CreatingMinimapObj()
    {
        yield return new WaitForEndOfFrame();
        var playerobj = FindObjectOfType<PlayerControl>();
        if (playerobj != null && playerobj.gameObject.GetComponentInChildren<UIMinimapObj>() == null)
        {
            if ((UnitLayerMask & (1 << playerobj.gameObject.layer)) != 0)
            {
                var temp = Instantiate(iconObj, playerobj.transform);
                temp.GetComponent<UIMinimapObj>().Initialize(playerobj.transform, GlobalEnums.IconObjType.Player);
            }
        }
        Destroy(gameObject);
    }
}
