using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour{

    private bool reached = false;

    public List<ParticleSystem> peList;
    
    private void OnCollisionStay(Collision other) {
        if(other.gameObject.tag == "Floor" && !reached){
            Debug.Log("Cube reached floor!");
            reached = true;
            StartCoroutine(DestroyGameObject());
        }
    }

    private void Start() {
        // gameObject.transform.localScale  = new Vector3(.8f,.8f,.8f);
        // LeanTween.scale(gameObject,new Vector3(1.5f,1.5f,1.5f),.3f)
        //     .setDelay(.5f).setEaseInCirc();
    }

    IEnumerator DestroyGameObject(){
        yield return new WaitForSeconds(2.5f);

        // destroy effect

        gameObject.GetComponent<MeshRenderer>().enabled = false;

        foreach(ParticleSystem pe in peList){
            pe.Play();
        }

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        GameEvents.current.CubeDestroyed();
    }



}
