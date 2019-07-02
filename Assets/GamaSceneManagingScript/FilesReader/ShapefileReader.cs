using System.Collections;
using System.Collections.Generic;
using ummisco.gama.unity.files.ShapefileImporter;
using UnityEngine;


public class ShapefileReader : MonoBehaviour
{
	public ShapeFile shapeFile;
	public string fileName;
	// Start is called before the first frame update
	void Start()
    {
		Debug.Log("Let's try to read a shapefile!");
		fileName = "/Users/sklab/Desktop/TODELETE/zone_etude/zones241115.shp";

		shapeFile = new ShapeFile();

        shapeFile.ReadShapes(fileName, 200000, 1, 200000, 1);
        
        //shapeFile.Read(fileName);

        Debug.Log("Good, file was read.");

        Debug.Log(shapeFile.ToString());

        Debug.Log(shapeFile.FileHeader.ToString());

        

        foreach (ShapeFileRecord rec in shapeFile.MyRecords)
        {

            Debug.Log("---> The record number is : " + rec.RecordNumber);
            Debug.Log("---> The record Content length is : " + rec.ContentLength);
            Debug.Log("---> The record Shape type is : " + rec.ShapeType);
            Debug.Log("---> The record number of parts is : " + rec.NumberOfParts);
            Debug.Log("---> The record number of points is : " + rec.NumberOfPoints);

            Debug.Log("---> the record attributes are: "+rec.Attributes);
            
            foreach (Vector2 v in rec.Points)
            {
                //Debug.Log("---> the point is: " + v);
                
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
		
	}
}

