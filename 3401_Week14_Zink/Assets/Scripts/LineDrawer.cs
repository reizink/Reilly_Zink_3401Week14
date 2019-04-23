using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject LinePrefab;
    public float MinimumSegmentLength = 0.5f;

    private Vector3 _previousMousePos;
    private LineRenderer _currentLine;
    private List<GameObject> _undoBuffer;
    //public LineRenderer line;
    //public EdgeCollider2D edge;

    private void Start()
    {
        _undoBuffer = new List<GameObject>();
    }


    // Update is called once per frame
    void Update()
    {
        //spawn line prefab on left click down
        if (Input.GetMouseButtonDown(0))
        {
            _currentLine = Instantiate(LinePrefab).GetComponent<LineRenderer> ();
            _previousMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        //if there's a current line, update it
        if(_currentLine != null)
        {
            Vector3 currentMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentMousePos.z = 0;

            if(Vector3.Distance (_previousMousePos, currentMousePos) > MinimumSegmentLength)
            {
                _currentLine.positionCount++;
                _currentLine.SetPosition(_currentLine.positionCount - 1, currentMousePos);

                //copy line points to array
                EdgeCollider2D currentEdge = _currentLine.gameObject.GetComponent<EdgeCollider2D>();
                Vector2[] edgePoints = new Vector2[_currentLine.positionCount];

                for(int i = 0; i < edgePoints.Length; i++)
                {
                    edgePoints[i] = _currentLine.GetPosition(i);
                }
                //assign collider points
                currentEdge.points = edgePoints;

                //set prev mouse pos for next frame
                _previousMousePos = currentMousePos;
            }


        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_currentLine.positionCount < 2)
            {
                Destroy(_currentLine.gameObject);
            }
            else
            {
                //add to history
                _undoBuffer.Add(_currentLine.gameObject);
            }


            _currentLine = null;
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
           //undo last line in list
           Destroy(_undoBuffer[_undoBuffer.Count - 1]);
           _undoBuffer.RemoveAt(_undoBuffer.Count - 1);

        }

        //if (Input.GetMouseButtonDown(0)) //on left click
        //{
        //    //get cursor position, to currentMousePos after new code
        //    Vector3 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //    clickPoint.z = 0;
        //    line.positionCount++; //add new point to line

        //    //set last position of mouse
        //    line.SetPosition(line.positionCount - 1, clickPoint);

        //    //add line points to array for collider
        //    Vector2[] edgePoints = new Vector2[line.positionCount];
        //    for(int i = 0; i < edgePoints.Length; i++)
        //    {
        //        edgePoints[i] = line.GetPosition(i);
        //    }
        //    //set collider points
        //    edge.points = edgePoints;
        //}
    }
}
