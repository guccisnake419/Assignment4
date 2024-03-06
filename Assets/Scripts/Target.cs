using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// adding namespaces
using Unity.Netcode;

public class Target : NetworkBehaviour
{
    public Score scoreManager;
   
    
    //this method is called whenever a collision is detected
    private void OnCollisionEnter(Collision collision)
    {

        // printing if collision is detected on the console
        Debug.Log("Collision Detected");
        
        // if the collision is detected destroy the object
        DestroyTargetServerRpc();
    }

    // client can not spawn or destroy objects
    // so we need to use ServerRpc
    // we also need to add RequireOwnership = false
    // because we want to destroy the object even if the client is not the owner
    [ServerRpc(RequireOwnership = false)]
    public void DestroyTargetServerRpc()
    {
        //on collision adding point to the score
        scoreManager.AddPoint();
        // scoreManager.AddMessage(this.gameObject.GetComponent<TMP_Text>().text);
        //despawn
        GetComponent<NetworkObject>().Despawn(true);
        //after collision is detected destroy the gameobject
        Destroy(gameObject);
    }
}