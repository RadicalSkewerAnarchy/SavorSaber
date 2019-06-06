using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGSTextSpawner : MonoBehaviour
{
    public static MGSTextSpawner instance;
    public GameObject MGSTextPrefab;
    private Queue<CallData> q = new Queue<CallData>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnText(IngredientData data, Vector2 position)
    {
        q.Enqueue(new CallData { data = data, position = position });
        if (q.Count == 1)
        {
            StartCoroutine(SpawnAndWait());
        }
    }

    private IEnumerator SpawnAndWait()
    {
        var callData = q.Peek();
        var mgsText = Instantiate(MGSTextPrefab, transform.position + new Vector3(0,1,0), Quaternion.identity).GetComponent<MGSText>();
        mgsText.Configure(callData.data);
        yield return new WaitForSeconds(0.66f);
        q.Dequeue();
        if(q.Count > 0)
            StartCoroutine(SpawnAndWait());
    }

    private struct CallData
    {
        public IngredientData data;
        public Vector2 position;
    }

}
