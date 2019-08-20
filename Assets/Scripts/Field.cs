using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    public GameObject ObjectPrefab;
    public int NumStars;


	BoundingBox BoundingBox;

	void Start()
    {
		BoundingBox = GetComponent<BoundingBox>();

		for (int i = 0; i < NumStars; i++)
        {
            int x = Random.Range(BoundingBox.Left, BoundingBox.Right);
			int y = Random.Range(BoundingBox.Bottom, BoundingBox.Top);

            GameObject obj = Instantiate(ObjectPrefab, transform);
            obj.transform.localPosition = new Vector3(x, y, 0);
        }
    }
}
