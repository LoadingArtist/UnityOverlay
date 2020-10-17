using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStreamlabs;
using UnityEngine.Playables;

public class teststreamlabs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        Streamlabs.Connect();
        Streamlabs.OnDonation += HandleDonation;
        Streamlabs.OnNewFollower += HandleNewFollower;
    }

    private void OnDisable()
    {
        Streamlabs.Disconnect();
        Streamlabs.OnDonation -= HandleDonation;
        Streamlabs.OnNewFollower -= HandleNewFollower;
    }

    private void HandleDonation(Donation donation)
    {
    }

    [ContextMenu("Send Donation")]
    private void SendDonation()
    {

    }

    private void HandleNewFollower(NewFollow newFollow)
    {
        Debug.Log($"{ newFollow.message[0].name} IS NOW FOLLOWING!");
        FindObjectOfType<animctrl>().followName.text = newFollow.message[0].name;
        GetComponent<PlayableDirector>().Play();
    }
}
