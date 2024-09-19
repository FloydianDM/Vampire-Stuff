using UnityEngine;

public class GameResources : MonoBehaviour
{
    private static GameResources _instance;

    public static GameResources Instance 
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<GameResources>("GameResources");
            }

            return _instance;
        }
    }

    [Header("Player")]
    public PlayerDetailsSO CurrentPlayerDetails;

}
