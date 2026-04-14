using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    [SerializeField] private ProgressionState currentState = ProgressionState.PreOldMan;
    [SerializeField] private Windmill[] windmills = new Windmill[3];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case ProgressionState.PreOldMan:
                break;
            case ProgressionState.PostOldMan:
                bool allWindmillsSpinning = true;
                foreach (Windmill w in windmills)
                {
                    allWindmillsSpinning &= w.isSpinning;
                }
                if (allWindmillsSpinning)
                    //OpenGate();
                    ChangeState(ProgressionState.WindmillsActive);
                break;

            case ProgressionState.WindmillsActive:
                // if (cauldron.isActive()){
                // RaiseBridge();
                // ChangeState(ProgressionState.PlatformsSpawned;
                //}
                break;
            case ProgressionState.PlatformsSpawned:
                break;
        }
    }

    private void ChangeState(ProgressionState state)
    {
        currentState = state;
    }
    private enum ProgressionState
    {
        PreOldMan,
        PostOldMan,
        WindmillsActive,
        PlatformsSpawned
    }
}
