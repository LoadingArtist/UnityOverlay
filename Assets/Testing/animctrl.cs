using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class animctrl : MonoBehaviour
{
    public PlayableDirector pd;
    public Text followName;
    // Start is called before the first frame update
    void Start()
    {
        Application.runInBackground = true;
        InvokeRepeating("WaveAnim", 5f, 7f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GetComponent<Animator>().SetTrigger("wavetrigger");
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            pd.Play();
        }
    }

    void WaveAnim()
    {
        GetComponent<Animator>().SetTrigger("wavetrigger");
    }
}
