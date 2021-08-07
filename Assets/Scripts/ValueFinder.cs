using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueFinder : MonoBehaviour
{
   public GameObject target;

    private int cubeValue;

   public int FindValue(GameObject cube) {

        Vector3 test = cube.transform.up;
        if( Mathf.RoundToInt(test.y) > 0 ){
            cubeValue = 6;
        }else{
            if( Mathf.RoundToInt(test.y) < 0 ){
                cubeValue = 1;
            }else{
                test = cube.transform.right;
                if( Mathf.RoundToInt(test.y) > 0 ){
                    cubeValue = 5;
                }else{
                    if( Mathf.RoundToInt(test.y) < 0){
                        cubeValue = 2;
                    }else{
                        test = cube.transform.forward;
                        if( Mathf.RoundToInt(test.y) > 0 ){
                            cubeValue = 3;
                        }else {
                            cubeValue = 4;
                            }
                    }
                }
            }
        }

        print("UP :" + cube.transform.up);
        print("RIGHT :" + cube.transform.right);
        print("FORWORD :" + cube.transform.forward);

       Debug.Log("Cube value: "+cubeValue.ToString());

       return cubeValue;
   }


   private void Update() {
    //    print(cube.transform.up);
   }
}
