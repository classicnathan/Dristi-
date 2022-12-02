using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;
using TMPro;

public class LineManager : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public ARPlacementInteractable placementInteractable;
    public TextMeshPro mText;
    private int pointCount = 0;
    LineRenderer line;
    public bool continuous;
    public TextMeshProUGUI buttonText;



    // Start is called before the first frame update
    void Start()
    {
        placementInteractable.objectPlaced.AddListener(DrawLine);

    }

    public void ToggleBetweenDiscreteAndContinuous()
    {
        continuous = !continuous;
        if (continuous)
        {
            buttonText.text = "Discrete";

        }
        else
        {
            buttonText.text = "Continuous";
        }
    }


    void DrawLine(ARObjectPlacementEventArgs args)
    {
        pointCount++;

        if (pointCount < 2)
        {
            line = Instantiate(lineRenderer);
            line.positionCount = 1;

        }
        else
        {
            line.positionCount = pointCount;
            if(!continuous)
              pointCount = 0;
        }
       
        //2. let the points location in the line renderer 
        line.SetPosition(index:line.positionCount-1,args.placementObject.transform.position);

        //for the allignment of the measurement text and line settings
        if (line.positionCount>1 )
        {
            Vector3 pointA = line.GetPosition(index: line.positionCount - 1);
            Vector3 pointB = line.GetPosition(index: line.positionCount - 2);
            float dist = Vector3.Distance(pointA, pointB);

            TextMeshPro distText = Instantiate(mText);
            distText.text = "" + dist;

            Vector3 directionVector = (pointB - pointA);
            Vector3 normal = args.placementObject.transform.up;

            Vector3 upd = Vector3.Cross(lhs: directionVector, rhs: normal).normalized;
            Quaternion rotation = Quaternion.LookRotation(forward: -normal, upwards: upd);

            distText.transform.rotation = rotation;
            distText.transform.position = (pointA + directionVector * 0.5f) + upd * 0.5f;

        }
    }
}
