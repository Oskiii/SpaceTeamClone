using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GoalCommand : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goalText;
    [SerializeField] private Slider _timerSlider;
    private const float _baseGoalTime = 5f;
    private float _goalTimeLeft = _baseGoalTime;
    private bool _timerActive = false;
    public int GoalIndex { get; private set; }

    public event Action TimerDoneEvent;

    public void SetGoal(int commandIndex, string commandText)
    {
        GoalIndex = commandIndex;
        _goalText.text = commandText;
        ResetGoalTime();
    }

    private void ResetGoalTime(){
        _goalTimeLeft = _baseGoalTime;
        _timerActive = true;
    }

    private void Update(){
        if(_timerActive){
            TimerTick();
        }
    }

    private void TimerTick(){
        _goalTimeLeft -= Time.deltaTime;
        _timerSlider.value = _goalTimeLeft.Scale(0f, _baseGoalTime, 0f, 1f);

            if(_goalTimeLeft < 0f){
            _timerActive = false;
            TimerDoneEvent();
        }
    }
}