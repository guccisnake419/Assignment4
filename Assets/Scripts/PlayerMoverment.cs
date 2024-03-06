using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
// adding namespaces
using Unity.Netcode;
// because we are using the NetworkBehaviour class
// NewtorkBehaviour class is a part of the Unity.Netcode namespace
// extension of MonoBehaviour that has functions related to multiplayer
public class PlayerMovement : NetworkBehaviour
{
    public float speed = 2f;
    public Score scoreManager;

    // create a list of colors
    public List<Color> colors = new List<Color>();

    // getting the reference to the prefab
    [SerializeField]
    private GameObject spawnedPrefab;
    // save the instantiated prefab
    private GameObject instantiatedPrefab;
    public float rotationSpeed= 90;
    Rigidbody rb;
    Transform t;
    public GameObject cannon;
    public GameObject bullet;

    public const int interval = 2000;

    public DateTime prevcol= DateTime.Now;

    // reference to the camera audio listener
    [SerializeField] private AudioListener audioListener;
    // reference to the camera
    [SerializeField] private Camera playerCamera;


    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody>();
        t= GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        // check if the player is the owner of the object
        // makes sure the script is only executed on the owners 
        // not on the other prefabs 
        if (!IsOwner) return;

        Vector3 moveDirection = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)){
            

            rb.velocity+=this.transform.forward * speed * Time.deltaTime;
        }
            
        else if (Input.GetKey(KeyCode.S))
            rb.velocity -= this.transform.forward * speed * Time.deltaTime;

        if (Input.GetKey(KeyCode.D))
        {
            
            t.rotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
            // this.transform.forward=new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));

        }
        else if (Input.GetKey(KeyCode.A)){
            t.rotation *= Quaternion.Euler(0, -rotationSpeed* Time.deltaTime, 0);
            // this.transform.forward=new Vector3(Input.GetAxis("Horizontal"),0,Input.GetAxis("Vertical"));

        }


        // if I is pressed spawn the object 
        // if J is pressed destroy the object
        // if (Input.GetKeyDown(KeyCode.I))
        // {
        //     //instantiate the object
        //     instantiatedPrefab = Instantiate(spawnedPrefab);
        //     // spawn it on the scene
        //     instantiatedPrefab.GetComponent<NetworkObject>().Spawn(true);
        // }

        // if (Input.GetKeyDown(KeyCode.J))
        // {
        //     //despawn the object
        //     instantiatedPrefab.GetComponent<NetworkObject>().Despawn(true);
        //     // destroy the object
        //     Destroy(instantiatedPrefab);
        // }

        // if (Input.GetButtonDown("Fire1"))
        // {
        //     // call the BulletSpawningServerRpc method
        //     // as client can not spawn objects
        //     BulletSpawningServerRpc(cannon.transform.position, cannon.transform.rotation);
        // }
    }

    // this method is called when the object is spawned
    // we will change the color of the objects
    public override void OnNetworkSpawn()
    {
        GetComponent<MeshRenderer>().material.color = colors[(int)OwnerClientId];

        // check if the player is the owner of the object
        if (!IsOwner) return;
        // if the player is the owner of the object
        // enable the camera and the audio listener
        audioListener.enabled = true;
        playerCamera.enabled = true;
    }

    // need to add the [ServerRPC] attribute
    [ServerRpc]
    // method name must end with ServerRPC
    private void BulletSpawningServerRpc(Vector3 position, Quaternion rotation)
    {
        // call the BulletSpawningClientRpc method to locally create the bullet on all clients
        BulletSpawningClientRpc(position, rotation);
    }

    [ClientRpc]
    private void BulletSpawningClientRpc(Vector3 position, Quaternion rotation)
    {
        GameObject newBullet = Instantiate(bullet, position, rotation);
        newBullet.GetComponent<Rigidbody>().velocity += Vector3.up * 2;
        newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * 1500);
        // newBullet.GetComponent<NetworkObject>().Spawn(true);
    }
    void OnCollisionEnter(Collision col){
    
        DateTime current_col= DateTime.Now;
        if (col.gameObject.CompareTag("Player")){
            Debug.Log("collided with player 2");
            // Debug.Log(current_col, " ", prevcol);
            Debug.Log((current_col - prevcol).TotalMilliseconds);
            if ((current_col - prevcol).TotalMilliseconds> interval){
                Debug.Log("In here");
                scoreManager.DecementPoint();
                
            }
            
            
            prevcol= DateTime.Now;
            
            

        }
        // else if (col.gameObject.name=="bb8"){
            
        // }
    }
}