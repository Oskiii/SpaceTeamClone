using TMPro;
using UnityEngine;

public class GoalCommand : MonoBehaviour
{
    private int _currentGoalIndex;
    [SerializeField] private TextMeshProUGUI _goalText;

    public int CurrentGoalIndex
    {
        get { return _currentGoalIndex; }
        set
        {
            _currentGoalIndex = value;
            print("Setting goal to: " + value);
            _goalText.text = CommandController.CommandsStrings[_currentGoalIndex];
        }
    }
}