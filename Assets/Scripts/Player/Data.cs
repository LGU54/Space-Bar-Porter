using System.Collections.Generic;

public static class Data
{
    private static Dictionary<string, int> dataList = new Dictionary<string, int>() { };

    private static List<string> Scene = new List<string> { "no0", "no1", "no2", "no3", "no4", "no5" };
    public static void addData(string str, int num)
    {
        dataList.Add(str, num);
    }

    public static void SearchListAndReplace(string str, int num)
    {
        if (dataList.ContainsKey(str))
        {
            dataList[str] = num;
        }
        else
        {
            dataList.Add(str, num);
        }
    }

    public static int SearchListAndOutput(string str)
    {
        if (dataList.ContainsKey(str))
        {
            return dataList[str];
        }
        else
        {
            return -1;
        }
    }
    public static void DataClear()
    {
        dataList.Clear();
    }

    public static string SearchList(int num)
    {
        return Scene[num];
    }
    public static int GetIndexOfList(string name)
    {
        return Scene.IndexOf(name);
    }
}