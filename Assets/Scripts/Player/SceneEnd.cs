using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneEnd
{
    public static void EnterNext()
    {
        string SceneName = SceneManager.GetActiveScene().name;
        // Debug.Log(SceneName);
        if (SceneName == "no5")
        {
            ControlSceneManager.SwitchSceneWithoutConfirm("StartScene");
            return;
        }

        int index = Data.GetIndexOfList(SceneName);
        ControlSceneManager.SwitchSceneWithoutConfirm(Data.SearchList(index + 1), () => {
            DialogFSM.GetInstance().Context = new PlotReader(index + 1);
            DialogFSM.GetInstance().Context.ReadBeforeLines();
            DialogFSM.NextLine();
        });
        UserPreference.Save("save", Data.SearchList(index + 1));
    }
}
