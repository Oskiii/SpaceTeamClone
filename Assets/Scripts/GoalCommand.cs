using TMPro;
using UnityEngine;

public class GoalCommand : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goalText;
    public int GoalIndex { get; private set; }

    public void SetGoal(int commandIndex, string commandText)
    {
        GoalIndex = commandIndex;
        _goalText.text = commandText;
    }
}