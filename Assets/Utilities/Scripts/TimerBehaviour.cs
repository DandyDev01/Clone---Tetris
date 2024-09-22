using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerBehaviour : MonoBehaviour
{
    [SerializeField] private float countDownTime = 1;
    [SerializeField] private bool endOnDestroy = false;
    [SerializeField] private bool playOnStart = true;
    [SerializeField] private bool loop = false;
    [SerializeField] private UnityEvent OnTimerEnd = new UnityEvent();

    private bool isPlaying;
    private Timer timer;

    // Start is called before the first frame update
    void Start()
    {
        isPlaying = playOnStart;

        timer = new Timer(countDownTime);


        timer.OnTimerEnd += HandleTimerEnd;
    }

    private void Update()
    {
        if(isPlaying)
            timer.Tick(Time.deltaTime);
    }

    private void HandleTimerEnd()
	{

        OnTimerEnd?.Invoke();

        if (!loop)
            Destroy(this.gameObject);
		else
		{
            timer.Reset(countDownTime);
            timer.Play();
        }
            

	}

    public void SetCountDownTime(float time)
	{
        countDownTime = time;
	}

    public void StartCountDown()
	{
        isPlaying = true;
        timer.Play();
	}

    public void StopCoutDown()
	{
        isPlaying = false;
        timer.Reset(countDownTime);
	}

	private void OnDestroy()
    {
        if (endOnDestroy)
        {
            OnTimerEnd?.Invoke();
            StopAllCoroutines();
        }
        
        OnTimerEnd.RemoveAllListeners();
    }

	public void EndEarly()
	{
        OnTimerEnd?.Invoke();
	}

}
