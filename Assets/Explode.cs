using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Exp());

    }

    IEnumerator Exp()
    {
        yield return new WaitForSeconds(0.6f);
        Destroy(gameObject);

    }
}
