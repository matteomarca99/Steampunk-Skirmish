using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "UI/Color Data", order = 1)]
public class ColorData : ScriptableObject
{
    public Color eligibleSlotColor;
    public Color defaultTargetBorderColor;
    public Color eligibleTargetBorderColor;

    public Color playerZonePanelColor;
    public Color playerZoneTextColor;

    public Color opponentZonePanelColor;
    public Color opponentZoneTextColor;

    public Color contestedZonePanelColor;
    public Color contestedZoneTextColor;

    public Color neutralZonePanelColor;
    public Color neutralZoneTextColor;

    public Color playerScoreAddColor;
    public Color opponentScoreAddColor;
}