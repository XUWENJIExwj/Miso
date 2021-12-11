using TMPro;
using UnityEngine;

public class SubEventAchievementFrame : MonoBehaviour
{
    [SerializeField] private TMP_Text progress = null;
    [SerializeField] private SubEventAchievementSubFrame prefab = null;
    [SerializeField] private SubEventAchievementSubFrame[] frames = null;

    public void Init()
    {
        int line = Mathf.CeilToInt((float)GlobalInfo.instance.subEventList.Count / prefab.AchievementCount());
        frames = new SubEventAchievementSubFrame[line];
        for (int i = 0; i < line; ++i)
        {
            frames[i] = Instantiate(prefab, transform);
            frames[i].Init(i);
        }
    }

    public void SetAchivementsInfo()
    {
        progress.text = Player.instance.GetPlayerData().achievements.sub.Progress().ToString("P");

        foreach (SubEventAchievementSubFrame frame in frames)
        {
            frame.SetAchivementsInfo();
        }
    }
}
