using TMPro;
using UnityEngine;

public class SubEventAchievementFrame : MonoBehaviour
{
    [SerializeField] protected TMP_Text eventType = null;
    [SerializeField] protected TMP_Text progress = null;
    [SerializeField] protected SubEventAchievementSubFrame prefab = null;
    [SerializeField] protected SubEventAchievementSubFrame[] frames = null;

    public virtual void Init(AMAs AMA = AMAs.Max)
    {
        int line = Mathf.CeilToInt((float)GlobalInfo.instance.subEventList.Count / prefab.AchievementCount());
        frames = new SubEventAchievementSubFrame[line];
        for (int i = 0; i < line; ++i)
        {
            frames[i] = Instantiate(prefab, transform);
            frames[i].Init(i);
        }
    }

    public virtual void SetAchivementsInfo()
    {
        progress.text = Player.instance.GetPlayerData().achievements.sub.Progress().ToString("P");

        foreach (SubEventAchievementSubFrame frame in frames)
        {
            frame.SetAchivementsInfo();
        }
    }
}
