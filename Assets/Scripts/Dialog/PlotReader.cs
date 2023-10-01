using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public enum LineType
{
    Narration,
    CharacterA,
    CharacterB
}

public struct Line
{
    public LineType Type;
    public string Text;
    public string Id;
}

public class PlotReader
{
    public int Id;
    public List<Line> BeforeGameLines;
    public List<Line> InGameLines;
    public List<Line> AfterGameLines;

    public List<Line> CurrentLines;

    public PlotReader(int chapter)
    {
        Id = chapter;
        BeforeGameLines = new List<Line>();
        InGameLines = new List<Line>();
        AfterGameLines = new List<Line>();

        CurrentLines = new List<Line>();

        Read();
    }

    public void Read()
    {
        string lang = UserPreference.Read<string>("Language") ?? "en";
        string path = Application.streamingAssetsPath + $"/I18n/{lang}/{Id}.xml";

        if (File.Exists(path))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);

            if (xmlDoc.SelectSingleNode("Localization").Attributes["lang"].Value != lang)
            {
                throw new Exception("Localization File Error.");
            }

            XmlNodeList nodeList = xmlDoc.SelectSingleNode("Localization")?.ChildNodes;

            if (nodeList != null)
            {
                foreach (XmlElement node in nodeList)
                {
                    if (node.Name == "BeforeGame")
                    {
                        foreach (XmlElement line in node.ChildNodes)
                        {
                            BeforeGameLines.Add(new Line
                            {
                                Type = (LineType) Enum.Parse(typeof(LineType), line.Name),
                                Text = line.InnerText.Replace("&#10;", Environment.NewLine),
                                Id = line.Attributes["id"]?.Value
                            });
                        }
                    }
                    else if (node.Name == "InGame")
                    {
                        foreach (XmlElement line in node.ChildNodes)
                        {
                            InGameLines.Add(new Line
                            {
                                Type = (LineType) Enum.Parse(typeof(LineType), line.Name),
                                Text = line.InnerText.Replace("&#10;", Environment.NewLine),
                                Id = line.Attributes["id"]?.Value
                            });
                        }
                    }
                    else if (node.Name == "AfterGame")
                    {
                        foreach (XmlElement line in node.ChildNodes)
                        {
                            AfterGameLines.Add(new Line
                            {
                                Type = (LineType) Enum.Parse(typeof(LineType), line.Name),
                                Text = line.InnerText.Replace("&#10;", Environment.NewLine),
                                Id = line.Attributes["id"]?.Value
                            });
                        }
                    }
                }
            }

            return;
        }

        throw new Exception("Localization File not found.");
    }

    public void ReadBeforeLines()
    {
        CurrentLines.AddRange(BeforeGameLines);
    }

    public void ReadGameLines()
    {
        CurrentLines.AddRange(InGameLines);
    }

    public void ReadAfterLines()
    {
        CurrentLines.AddRange(AfterGameLines);
    }
}