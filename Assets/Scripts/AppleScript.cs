using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleScript : MonoBehaviour
{
    public BoxCollider2D GridArea;
    public GameObject Snake;

    private void Start()
    {
        RandomizePosition();
    }

    private void RandomizePosition()
    {
        Bounds bounds = this.GridArea.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);

        this.transform.position = new Vector3(Mathf.Round(x),Mathf.Round(y),0.0f);
        List<Transform> segments = Snake.GetComponent<PlayerScript>()._segments;
        for (int i = 0; i < segments.Count; i++){
            if (this.transform.position == segments[i].position){
                print ("Spawned in the Same Space as Snake");
                RandomizePosition();
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player"){
            RandomizePosition();
        }
    }
}
