using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple item generator. Instantiates items on the floor
/// </summary>
public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> items = new List<GameObject>();
    
    [SerializeField][Range(1,20)] private int generateNumber = 3;
    
    [SerializeField][Range(1,10)] private int fieldSize = 5;
    [SerializeField][Range(0,2)] private float generationHeight = 0.5f;
    
    // Start is called before the first frame update
    IEnumerator Start()
    {
        for (int i = 0; i < generateNumber; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(-fieldSize, fieldSize),
                generationHeight,
                Random.Range(-fieldSize, fieldSize)
                );

            var item = Instantiate(items[Random.Range(0, items.Count)]);

            item.transform.position = position;
            
            yield return 0;
        }
    }
}
