using System;
using System.IO;
using UnityEngine;
 

public class LogFiles : MonoBehaviour{
    static string path = "Assets/LogFiles/";
    static string fileName = "LogFiles.txt";


    public static void WriteLogs(string logs){
        using (StreamWriter w = File.AppendText(path + fileName))
        {
            Log("Test1", w);
            Log("Test2", w);
        }
    }

    public static void ReadLogs()
    {
        using (StreamReader r = File.OpenText(path + fileName))
        {
            DumpLog(r);
        }
    }


    public static void Log(string logMessage, TextWriter w)
    {
        w.Write("\r\nLog Entry : ");
        w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
            DateTime.Now.ToLongDateString());
        w.WriteLine("  :");
        w.WriteLine("  :{0}", logMessage);
        w.WriteLine("-------------------------------");
    }

    public static void DumpLog(StreamReader r)
    {
        string line;
        while ((line = r.ReadLine()) != null)
        {
            Console.WriteLine(line);
        }
    }
}
