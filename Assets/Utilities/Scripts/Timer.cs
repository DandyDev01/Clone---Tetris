using System;
public class Timer
{

	public float RemainingTime { get; private set; }
	public bool Finished { get; private set; }
	private bool isPlaying = true;

	public bool IsPlaying { get { return isPlaying; } }	

	public event Action OnTimerEnd;
	public event Action<float> OnTick;

	public Timer(float coutDownTime)
	{
		RemainingTime = coutDownTime;
	}

	public Timer(bool playOnStart)
	{
		isPlaying = playOnStart;
	}

	public Timer(float countDownTimer, bool playOnStart)
	{
		RemainingTime = countDownTimer;
		isPlaying = playOnStart;
	}

	
	// main functionality
	public void Tick(float deltaTime)
	{
		if (!isPlaying)
			return;

		CheckForTimerEnd();

		if (RemainingTime == 0)
			return;

		RemainingTime -= deltaTime;
		OnTick?.Invoke(deltaTime);
	}

	public void Play()
	{
		isPlaying = true;
	}

	public void Stop()
	{
		isPlaying = false;
	}

	/// <summary>
	/// reset the timer to start playing from countDownTime
	/// </summary>
	/// <param name="countDownTime">Time until the timer finishes</param>
	/// <param name="playOnRest">weather or not the timer plays after reset. Default false</param>
	public void Reset(float countDownTime, bool playOnRest = false)
	{
		Finished = false;
		isPlaying = playOnRest;
		RemainingTime = countDownTime;
	}

	// timer has ended, call event
	private void CheckForTimerEnd()
	{
		if (RemainingTime > 0f) return;

		RemainingTime = 0f;

		Finished = true;

		Stop();

		OnTimerEnd?.Invoke();
	}
}
