using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ObjectManage", order = 1)]
public class OjectScriptable : ScriptableObject
{
    public GameObject[] WallsSprites;
    public GameObject[] PosesSprites;
    public Texture2D[] PosesButtons;
}
