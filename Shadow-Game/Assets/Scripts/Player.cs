using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform segmentPrefab,cornerPrefab,Grabber,spawnArea,currSeg,currCor;
    public SpriteRenderer rend;
    public string dir = "w";
    public float error = 0.0025f;
    public List<Transform> _segments,corners;
    public List<GameObject> keys;
    public float move_cooldown = 0.01f;
    private float wait_to_move = 0f;
    public bool touching_corner = false;
    public Animator anim;
    public int keyCount = 0;
    public bool holding = false;
    public bool hit_ob = true;

    private Vector3 startPos;
    
    void Start()
    {
        _segments = new List<Transform>();
        corners = new List<Transform>();
        keys = new List<GameObject>();
        startPos = transform.position;
        currSeg = SpawnSeg();
    }

    // Update is called once per frame
    private void Update()
    {
        if (wait_to_move <= 0f)
        {
            wait_to_move = move_cooldown;
            if(hit_ob){
                Move();
            } 
        }
        wait_to_move -= Time.deltaTime;
        Grab();
        
    }

    public void Grab(){
        if(Input.GetKeyDown(KeyCode.Space)){
            holding = true;
            anim.SetBool("grab",true);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(Grabber.position, 0.5f);
            foreach (Collider2D nearby in colliders){
                if (nearby.tag == "Key"){
                    if (nearby != null){
                        nearby.transform.parent = Grabber.transform;
                        keys.Add(nearby.gameObject);
                        keyCount++;
                    }
                }   
            }
        } else if(Input.GetKeyUp(KeyCode.Space)) {
            anim.SetBool("grab",false);
            holding = false;
            foreach(GameObject i in keys){
                if(i != null){
                    i.transform.parent = null;
                }
            }
            keys.Clear();
            keyCount = 0;
        }
    }

    private void Move()
    {
        Vector3 Move = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)){
            Move = new Vector3(0, 1f, 0);
            if(dir == "w" || dir == "e"){
                SpawnCorners(dir, "s");
                dir = "s";
                transform.rotation = Quaternion.Euler(0f,0f,90f);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),Mathf.Abs(transform.localScale.y),Mathf.Abs(transform.localScale.z));
            } else if(touching_corner && dir == "n"){
                _segments.Remove(currSeg);
                dir = "s";
                Corner c = currCor.GetComponent<Corner>();
                transform.position = c.Setup(c.source,c.sdir,dir).position;
                currSeg = SpawnSeg();
                currSeg.rotation = currCor.rotation;
                transform.rotation = Quaternion.Euler(0f,0f,90f);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),Mathf.Abs(transform.localScale.y),Mathf.Abs(transform.localScale.z));
            }
        }   
        else if (Input.GetKey(KeyCode.S)){
            Move = new Vector3(0, -1f, 0);
            if(dir == "w" || dir == "e"){
                SpawnCorners(dir, "n");
                dir = "n";
                transform.rotation = Quaternion.Euler(0f,0f,90f);
                transform.localScale = new Vector3(-1*Mathf.Abs(transform.localScale.x),-1*Mathf.Abs(transform.localScale.y),Mathf.Abs(transform.localScale.z));
                currSeg.localScale = new Vector3(-0.1f,.97f,1f);
            } else if(touching_corner && dir == "s"){
                _segments.Remove(currSeg);
                dir = "n";
                Corner c = currCor.GetComponent<Corner>();
                transform.position = c.Setup(c.source,c.sdir,dir).position;
                currSeg = SpawnSeg();
                currSeg.rotation = currCor.rotation;
                transform.rotation = Quaternion.Euler(0f,0f,90f);
                transform.localScale = new Vector3(-1*Mathf.Abs(transform.localScale.x),-1*Mathf.Abs(transform.localScale.y),Mathf.Abs(transform.localScale.z));
                currSeg.localScale = new Vector3(-0.1f,.97f,1f);
            }
            
        }
        else if (Input.GetKey(KeyCode.A)){
            Move = new Vector3(-1f, 0, 0);
            if(dir == "n" || dir == "s"){
                SpawnCorners(dir, "e");
                dir = "e";
                transform.rotation = Quaternion.Euler(0f,0f,0f);
                transform.localScale = new Vector3(-1*Mathf.Abs(transform.localScale.x),Mathf.Abs(transform.localScale.y),Mathf.Abs(transform.localScale.z));
                currSeg.localScale = new Vector3(-0.1f,.97f,1f);
            } else if(touching_corner && dir == "w"){
                _segments.Remove(currSeg);
                dir = "e";
                Corner c = currCor.GetComponent<Corner>();
                transform.position = c.Setup(c.source,c.sdir,dir).position;
                currSeg = SpawnSeg();
                currSeg.rotation = currCor.rotation;
                transform.rotation = Quaternion.Euler(0f,0f,0f);
                transform.localScale = new Vector3(-1*Mathf.Abs(transform.localScale.x),Mathf.Abs(transform.localScale.y),Mathf.Abs(transform.localScale.z));
                currSeg.localScale = new Vector3(-0.1f,.97f,1f);
            }
        }
        else if (Input.GetKey(KeyCode.D)){
            Move = new Vector3(1f, 0, 0);
            if(dir == "n" || dir == "s"){
                SpawnCorners(dir, "w");
                dir = "w";
                transform.rotation = Quaternion.Euler(0f,0f,0f);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),Mathf.Abs(transform.localScale.y),Mathf.Abs(transform.localScale.z));
            } else if(touching_corner && dir == "e"){
                _segments.Remove(currSeg);
                dir = "w";
                Corner c = currCor.GetComponent<Corner>();
                transform.position = c.Setup(c.source,c.sdir,dir).position;
                currSeg = SpawnSeg();
                currSeg.rotation = currCor.rotation;
                transform.rotation = Quaternion.Euler(0f,0f,0f);
                transform.rotation = Quaternion.Euler(0f,0f,0f);
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),Mathf.Abs(transform.localScale.y),Mathf.Abs(transform.localScale.z));
            }
        }

        float cornerwidth = cornerPrefab.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;
        transform.position += Move * (cornerwidth);
        currSeg.localScale = new Vector3(Move.x*error + currSeg.localScale.x + Move.x * (cornerwidth)/2f,currSeg.localScale.y,currSeg.localScale.z);
        currSeg.localScale = new Vector3(Move.y*error + currSeg.localScale.x + Move.y * (cornerwidth)/2f,currSeg.localScale.y,currSeg.localScale.z);
    }

    private Transform SpawnSeg(){
        //float width = segmentPrefab.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;
        Vector3 spawnpos = new Vector3(spawnArea.position.x,spawnArea.position.y,spawnArea.position.z);
        Transform newSegment = Instantiate(segmentPrefab,spawnpos,transform.rotation);

        newSegment.localScale = new Vector3(0.1f,.97f,1f);
        
        _segments.Add(newSegment);
        return newSegment;
    }

    private Transform SpawnCorners(string olddir, string newdir){
        float width = cornerPrefab.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.x;
        Vector3 spawnpos = new Vector3(spawnArea.position.x,spawnArea.position.y,spawnArea.position.z);
        Transform newCorner = Instantiate(cornerPrefab,spawnpos,transform.rotation);
        
        transform.position = newCorner.GetComponent<Corner>().Setup(currSeg.gameObject,olddir,newdir).position;
        currSeg = SpawnSeg();
        currSeg.rotation = newCorner.rotation;
        corners.Add(newCorner);
        currCor = newCorner;
        return newCorner;
    }

    // Handle collisions
    private void OnTriggerEnter2D(Collider2D other){
        
        if (other.tag == "Obstacle"){
            hit_ob = false;
            CameraController.instance.Shake(.3f,.2f);
            transform.position = startPos;
            ResetPlayer();
            hit_ob = true;
        }
        if (other.tag == "powerup"){
            DeleteTail();
            currSeg = SpawnSeg();
            Destroy(other.gameObject);
        }
        if (other.tag == "Corner" && Object.ReferenceEquals(other.transform, currCor)){
            touching_corner = true;
        } else {
            touching_corner = false;
        }
        if (other.tag == "Door"){
            if(keyCount > 0){
                keyCount--;
                Destroy(other.gameObject);
                Destroy(keys[0]);
                keys.RemoveAt(0);
            } 
        }

    }

    private void OnTriggerExit2D(Collider2D other){
        if (other.tag == "Corner"){
            touching_corner = false;
        }
    }

    // get rid of player tail
    private void DeleteTail(){
        for (int i = 0; i < _segments.Count; i++){
            Destroy(_segments[i].gameObject);
        }
        _segments.Clear();
        for (int i = 0; i < corners.Count; i++){
            Destroy(corners[i].gameObject);
        }
        corners.Clear();
        
    }

    // reset the player to the starting position
    private void ResetPlayer(){
        DeleteTail();
        dir = "w";
        transform.rotation = Quaternion.Euler(0f,0f,0f);
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x),Mathf.Abs(transform.localScale.y),Mathf.Abs(transform.localScale.z));
        currSeg = SpawnSeg();
    }

    /*
     old snake code
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W)){
            _direction = Vector2.up;
            transform.rotation = Quaternion.Euler(0f,0f,90f);
            gameObject.GetComponent<SpriteRenderer>().flipY = true;
        }   
        else if (Input.GetKeyDown(KeyCode.S)){
            _direction = Vector2.down;
            transform.rotation = Quaternion.Euler(0f,0f,90f);
            gameObject.GetComponent<SpriteRenderer>().flipY = false;
            
        }
        else if (Input.GetKeyDown(KeyCode.A)){
            _direction = Vector2.left;
            transform.rotation = Quaternion.Euler(0f,0f,0f);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            
        }
        else if (Input.GetKeyDown(KeyCode.D)){
            _direction = Vector2.right;
            transform.rotation = Quaternion.Euler(0f,0f,0f);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
    }


    public void Move(){
        // move the segments in reverse order each segment is following the one in front of it
        for (int i = _segments.Count - 1; i > 0; i--){
            _segments[i].position = _segments[i - 1].position;
        }
        // move the head in the direction of the input
        transform.position = new Vector3(
            Mathf.Round(transform.position.x + _direction.x), 
            Mathf.Round(transform.position.y + _direction.y), 
            0.0f
        );

    }

    // create a new segment, place it on "tail end" of the shadow, and add it to the list
    private void Grow(){
        Transform newSegment = Instantiate(segmentPrefab);
        newSegment.position = _segments[_segments.Count - 1].position;
        _segments.Add(newSegment);
    }
    */


}
