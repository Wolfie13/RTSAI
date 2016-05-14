using UnityEngine;
using System.Collections;

public class SetResourceTileTest : MonoBehaviour
{

    public int resourceAmount = 0;
    public ResourceType type;

    public int SpawnX = 0, SpawnY = 0;


    public float DelayTime = 1f;


    // Use this for initialization
    IEnumerator Start()
    {
        yield return new WaitForSeconds(DelayTime);

        IVec2 MapPos = new IVec2(SpawnX, SpawnY);

        Map.CurrentMap.setResourcetile(type, resourceAmount, MapPos);
    }
}
	
