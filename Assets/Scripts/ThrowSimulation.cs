using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThrowSimulation : MonoBehaviour{
    

    #region  Public Members

    public Camera ARCamera;
    
    public Transform Target;
    
    public Button rollBtn;
    
    public GameObject cubePrefab;
    
    public TextMeshProUGUI valuesText;
    
    #endregion

    #region  Private Memebers
    
    private ValueFinder valueFinder;
    private Transform Projectile;    
    
    private List<GameObject> cubes;
    
    private bool toPlaceInFront = true;
    private bool isCubeRolled = false;
    
    private float firingAngle = 30.0f;
    private float gravity = 9.8f;
    
    #endregion

 
    void Start(){          
        cubes = new List<GameObject>();
        valueFinder = gameObject.GetComponent<ValueFinder>();
    }
 
    public void OnTargetFound(){
        if(!isCubeRolled) SetRollBtn(true);
    }

    public void OnTargetLost(){
        SetRollBtn(false);
    }

    private void SetRollBtn(bool state){
        rollBtn.interactable = state;
    }

    public void GO(){

        isCubeRolled = true;
        SetRollBtn(false);

        Vector3 pose = ARCamera.gameObject.transform.position;

        Vector3 cubePoseInst = ARCamera.gameObject.transform.position + ARCamera.gameObject.transform.forward*0.15f;
        cubePoseInst.y -= 0.04f;
        GameObject test = Instantiate(cubePrefab, cubePoseInst , transform.rotation);
        
        Projectile = test.transform;

        Target.parent = null;

        foreach(Transform child in Projectile){
            child.rotation = Random.rotation;
        }

        StartCoroutine(SimulateProjectile());
    }

    IEnumerator SimulateProjectile()
    {
        Projectile.parent = ARCamera.transform;

        // Short delay added before Projectile is thrown
        yield return new WaitForSeconds(.5f);
       
        Projectile.parent = null;


        // Move projectile to the position of throwing object + add some offset if needed.
        Projectile.position = Projectile.position + new Vector3(0, 0.0f, 0);
       
        // Calculate distance to target
        float target_Distance = Vector3.Distance(Projectile.position, Target.position);
 
        // Calculate the velocity needed to throw the object to the target at specified angle.
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);
 
        // Extract the X  Y componenent of the velocity
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);
 
        // Calculate flight time.
        float flightDuration = target_Distance / Vx;
   
        // Rotate projectile to face the target.
        Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);
       
        float elapse_time = 0;
        float testSpeed = 1f;
        while (elapse_time < flightDuration)
        {
            Projectile.Translate(0, (Vy - (gravity * elapse_time)) * Time.deltaTime * testSpeed, Vx * Time.deltaTime * testSpeed);
           
            elapse_time += Time.deltaTime * testSpeed;
 
            yield return null;
        }

        foreach(Transform child in Projectile){
            cubes.Add(child.gameObject);
            child.GetComponent<Rigidbody>().useGravity = true;
        }

        // detach both cubes
        Projectile.DetachChildren();

        // detect values of both cube
        StartCoroutine(DetectValues());
    }  

    IEnumerator DetectValues(){
        Destroy(Projectile.gameObject);
        yield return new WaitForSeconds(1f);
        valuesText.text = "[";
        bool tmp = true;
        foreach(GameObject obj in cubes){
            int value = valueFinder.FindValue(obj);
            valuesText.text += " "+ value.ToString();
            if(tmp){valuesText.text += " ,"; tmp = false;}
        }
        valuesText.text += " ]";
        cubes.Clear();
    }

    private void OnEnable() {
        GameEvents.current.onCubeDestoryed += cubeDestroyed;
    }


    private void OnDisable() {
        GameEvents.current.onCubeDestoryed -= cubeDestroyed;
    }

    private void cubeDestroyed(){
        isCubeRolled = false;
        SetRollBtn(true);
    }

}
