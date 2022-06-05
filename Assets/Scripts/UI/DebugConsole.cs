using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugConsole : MonoBehaviour
{
    public TMP_InputField input;
    public TMP_Text output;

    private void Start()
    { 
        input.onEndEdit.AddListener(OnCommand);
    }

    void Print(string line)
    {
        output.text += System.Environment.NewLine + ">" + line;
    }
    void PrintUnformatted(string line)
    {
        output.text += line;
    }

    void OnCommand(string command)
    {
        string[] args = command.Split(' ');
        string cmd = args[0];

        input.text = "";

        switch (cmd)
        {
            case "help":
                //I dont want to T_T
                Print("No T_T");
                break;
            case "tp":
                //Teleports currently consciuous robot to specified coordinates
                foreach(RobotController c in FindObjectsOfType<RobotController>())
                {
                    if (c.isConscious)
                    {
                        try
                        {
                            c.transform.position = new Vector3(float.Parse(args[1], System.Globalization.NumberStyles.Number),
                                                                float.Parse(args[2], System.Globalization.NumberStyles.Number),
                                                                float.Parse(args[3], System.Globalization.NumberStyles.Number));
                            Print("Teleported succesfully.");
                        }
                        catch (System.FormatException)
                        {
                            Print("Unable to parse input coordinates.");
                        }
                    }
                }
                break;
            case "effect":
                foreach (RobotController c in FindObjectsOfType<RobotController>())
                {
                    if (c.isConscious)
                    {
                        try
                        {
                            c.GetComponent<RobotConstructor>().ApplyStatusChange(int.Parse(args[1]));
                            Print("Applied status succesfully.");
                        }
                        catch (System.FormatException)
                        {
                            Print("Unable to parse input status.");
                        }
                    }
                }
                break;
        }
    }
}
