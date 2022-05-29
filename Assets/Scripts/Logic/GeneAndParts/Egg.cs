using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Egg : MonoBehaviour
{
    public GenePool pool;
    public int currentSeconds;
    public int maxSeconds;

    private void Start()
    {
        currentSeconds = maxSeconds;
        StartCoroutine("Countdown");
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSecondsRealtime(1);
        currentSeconds--;
        if(currentSeconds <= 0)
        {
            GameObject newRobot = Instantiate(Resources.Load<GameObject>("Empty_Robot"));
            newRobot.transform.tag = "Robot";
            newRobot.transform.position = transform.position;
            newRobot.transform.rotation = Quaternion.identity;
            RobotConstructor constructor = newRobot.GetComponent<RobotConstructor>();
            constructor.genePool = pool;
            constructor.ConstructRobot(pool, Vector3.zero, Quaternion.identity, newRobot.transform);
            this.enabled = false;
            Destroy(gameObject);
        }
        StartCoroutine("Countdown");
    }
}
