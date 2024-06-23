using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGrounds : MonoBehaviour
{
    public Transform groundsBorder;
    public Transform centerGround;
    public bool isChanging = false;

    
    public void CenterTheGround(Transform ground)
    {
        StartCoroutine(stopOtherCollision());
        Vector3 newCenter = ground.position;
        Vector3 newCenterLocal = ground.localPosition;
        Vector3 ancientCenter = centerGround.localPosition;

        /*groundsBorder.localPosition = newCenter;
        centerGround.position = ancientCenter;*/


        centerGround.parent = groundsBorder;
        centerGround.position = ground.position;

        ground.parent = transform;





        groundsBorder.position = ground.position;



        centerGround = ground;
    }


    IEnumerator stopOtherCollision()
    {
        isChanging = true;
        yield return new WaitForSeconds(1);
        isChanging = false;
    }

}
