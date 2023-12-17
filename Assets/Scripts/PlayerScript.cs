using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    private Vector2 _direction = Vector2.right;
    public List<Transform> _segments;
    public Transform segmentPrefab;
    private int Score = 0;
    public TextMeshProUGUI _text;
    private List<Vector3> AStarList;
    private int CurrentPos = 0;
    private bool PathTime = true;

    private void Start()
    {
        _text.text = Score.ToString();

        _segments = new List<Transform>();
        _segments.Add(this.transform);
    }

    // Update is called once per frame
    private void Update()
    {
        if (PathTime){
            this.GetComponent<AStarAlgorithm>().AStar();
            AStarList = this.GetComponent<AStarAlgorithm>().ResultPath;
            PathTime = false;
            CurrentPos = 0;
        }

        if (Input.GetKeyDown(KeyCode.Return) && Time.timeScale == 0){
            Time.timeScale = 1;
            ResetState();
            this.GetComponent<AStarAlgorithm>().ResultPath.Clear();
        }
        //if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            //_direction = Vector2.left;
        //}
        //else if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
            //_direction = Vector2.right;
        //}
        //else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            //_direction = Vector2.down;
        //}
        //else if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)){
            //_direction = Vector2.up;
        //}
    }

    

    private void FixedUpdate()
    {
        CurrentPos++;

        if (AStarList != null && AStarList.Count > CurrentPos){
            _direction = AStarList[CurrentPos] - this.transform.position; 
        }

        else{
            print("Recalculating");
            this.GetComponent<AStarAlgorithm>().AStar();
            AStarList = this.GetComponent<AStarAlgorithm>().ResultPath;
            PathTime = false;
            CurrentPos = 0;
            _direction = AStarList[CurrentPos] - this.transform.position;
            //Might Need to Set So Walk To Open Space And Then Run Algorithm Again
            //_direction = Vector2.right;
            //PathTime = true;
        }

        for (int i = _segments.Count - 1; i > 0; i--){
            _segments[i].position = _segments[i-1].position;
        }

        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + _direction.x,
            Mathf.Round(this.transform.position.y) + _direction.y,
            0.0f
        );
    }

    private void Grow()
    {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = _segments[_segments.Count - 1].position;

        _segments.Add(segment);
        Score++;
        _text.text = Score.ToString();
    }

    private void ResetState()
    {
        //print(Score);

        for (int i = 1; i < _segments.Count; i++){
            Destroy(_segments[i].gameObject);
        }

        _segments.Clear();
        _segments.Add(this.transform);

        this.transform.position = Vector3.zero;
        Score = 0;
        _text.text = Score.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Food"){
            Grow();
            PathTime = true;
        }
        else if (other.tag == "Player" || other.tag == "Wall"){
            print(Score);
            print("Died by " + other.tag + " at " + this.transform.position);
            Time.timeScale = 0;
        }
    }
}
