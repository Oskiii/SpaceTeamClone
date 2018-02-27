using System;
using System.Collections;
using System.Collections.Generic;

public class Scenario {

	private float _goalProgress;
	private float _progressIncrement;
    public float Progress {get; private set;}

	public event Action OnComplete;
	public event Action<float> OnProgressChanged;

	public Scenario(float goal = 1f, float increment = 0.1f){
		_goalProgress = goal;
		_progressIncrement = increment;
	}

	public void DecreaseProgress(){
		Progress -= _progressIncrement;
		OnProgressChanged(Progress);
	}

	public void IncreaseProgress(){
		Progress += _progressIncrement;
		OnProgressChanged(Progress);
	}

	private void CheckComplete(){
		if(Progress > _goalProgress){
			OnComplete();
		}
	}
}
