using UnityEngine;
using System;

public class GameEvents : MonoBehaviour{
    
    public static GameEvents current;

    void Awake(){
        if(current == null){
            current = this;
        } else{
            Destroy(gameObject);    
        }
    }


    public event Action onCubeDestoryed;
    public void CubeDestroyed() => onCubeDestoryed?.Invoke();

}
