using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialView : Monosingleton<TutorialView>
{
    [SerializeField] private Sprite[] sprites = null;
    [SerializeField] private TutorialPage[] prefabs = null;
    [SerializeField] private List<TutorialPage> pages = null;
    [SerializeField] private int currentPage = 0;
    [SerializeField] private Button[] turnButtons = null;

    public void Init()
    {
        CreatePages();

        gameObject.SetActive(Player.instance.GetPlayerData().tutorial);
    }

    private void CreatePages()
    {
        currentPage = 0;

        int i = 0;
        CreatePage(0, i);
        for (i = 1; i < sprites.Length - 1; ++i)
        {
            CreatePage(1, i);
        }
        CreatePage(prefabs.Length - 1, i);

        pages[0].gameObject.SetActive(true);
    }

    private void CreatePage(int PrefabIndex, int PageIndex)
    {
        TutorialPage page = Instantiate(prefabs[PrefabIndex], transform);
        page.Init(sprites[PageIndex]);
        page.name = "Page" + PageIndex.ToString("00");
        pages.Add(page);
        page.transform.SetSiblingIndex(PageIndex);
    }

    public void TurnPage(int Offset)
    {
        SoundManager.instance.SE_Tap();

        int nextPage = FixPage(Offset);
        pages[currentPage].TurnPage(pages[nextPage]);
        currentPage = nextPage;
    }

    public int FixPage(int Offset)
    {
        int nextPage = currentPage + Offset;

        if (nextPage < 0)
        {
            nextPage = 0;
        }
        else if (nextPage >= pages.Count)
        {
            nextPage = pages.Count - 1;
        }

        return nextPage;
    }

    public Button[] GetTurnButtons()
    {
        return turnButtons;
    }

    public void EndTutorialView()
    {
        SoundManager.instance.SE_Tap();

        Player.instance.SetTutorialFlag();
        Player.instance.Save();

        // ‰¼
        MainGameLogic logic = LogicManager.instance.GetSceneLogic<MainGameLogic>();
        logic.SetNextSate(MainGameState.BaseSelect);

        gameObject.SetActive(false);
    }
}