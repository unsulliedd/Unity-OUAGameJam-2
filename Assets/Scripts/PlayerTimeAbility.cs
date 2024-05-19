using System.Collections;
using System.Linq;
using UnityEngine;

public class PlayerTimeAbility : MonoBehaviour
{
    private Player player;
    [SerializeField] private float abilityDuration = 10f;
    [SerializeField] private bool isTimeStopped = false;
    [SerializeField] private bool isTimeSlowed = false;
    [SerializeField] private float slowTimeScale = 0.5f; // 50% speed
    [SerializeField] private GameObject stopTimeFx;
    [SerializeField] private GameObject slowTimeFx;

    void Start()
    {
        player = GetComponent<Player>();
        player.controls.Player.StopTimeAbility.performed += ctx => UseTimeStopAbility();
        player.controls.Player.SlowTimeAbility.performed += ctx => UseTimeSlowAbility();
    }

    private void UseTimeStopAbility()
    {
        CreateStopTimeFx();

        if (isTimeStopped)
            ResumeTime();
        else
            StartCoroutine(StopTime());
    }

    private IEnumerator StopTime()
    {
        isTimeStopped = true;

        ITimeAffectable[] timeAffectables = FindObjectsOfType<MonoBehaviour>().OfType<ITimeAffectable>().ToArray();
        foreach (ITimeAffectable timeAffectable in timeAffectables)
            timeAffectable.StopTime();

        yield return new WaitForSeconds(abilityDuration);

        foreach (ITimeAffectable timeAffectable in timeAffectables)
            timeAffectable.ResumeTime();

        isTimeStopped = false;
    }

    private void UseTimeSlowAbility()
    {
        CreateSlowTimeFx();

        if (isTimeSlowed)
            ResumeTime();
        else
            StartCoroutine(SlowTime());
    }

    private IEnumerator SlowTime()
    {
        isTimeSlowed = true;
        Time.timeScale = slowTimeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;

        yield return new WaitForSecondsRealtime(abilityDuration);

        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;

        isTimeSlowed = false;
    }

    private void ResumeTime()
    {
        if (isTimeStopped)
        {
            isTimeStopped = false;
            ITimeAffectable[] timeAffectables = FindObjectsOfType<MonoBehaviour>().OfType<ITimeAffectable>().ToArray();
            foreach (ITimeAffectable timeAffectable in timeAffectables)
                timeAffectable.ResumeTime();
        }

        if (isTimeSlowed)
        {
            isTimeSlowed = false;
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
    }

    private void CreateStopTimeFx()
    {
        GameObject newStopTimeFx = Instantiate(stopTimeFx, transform.position, Quaternion.identity);
        newStopTimeFx.transform.parent = transform;
        Destroy(newStopTimeFx, 10f);
    }

    private void CreateSlowTimeFx()
    {
        GameObject newSlowTimeFx = Instantiate(slowTimeFx, transform.position, Quaternion.identity);
        newSlowTimeFx.transform.parent = transform; 
        Destroy(newSlowTimeFx, 10f);
    }
}
