using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MyWindow : EditorWindow
{
    DirEntry rootDir;
    int filesCount;
    List<string> guids = new List<string>();

    int readDirSemaphore = 0;
    int countRefsSemaphore = 0;

    Vector2 scrollPos;

    static GUIStyle normalstyle = new GUIStyle();
    static GUIStyle directstyle = new GUIStyle();
    static GUIStyle noRefsStyle = new GUIStyle();

    private const string _cross = " ├─";
    private const string _corner = " └─";
    private const string _vertical = " │ ";
    private const string _space = "   ";

    // Add menu named "My Window" to the Window menu
    [MenuItem("Window/Reference Couner")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        MyWindow window = (MyWindow)EditorWindow.GetWindow(typeof(MyWindow));
        window.Show();
    }

    void OnGUI()
    {
        noRefsStyle.normal.textColor = new Color(0.7f, 0.1f, 0.1f);
        noRefsStyle.richText = true;
        noRefsStyle.fixedWidth = 0.9f;

        normalstyle.normal.textColor = new Color(0.7f, 0.7f, 0.7f);
        normalstyle.richText = true;
        normalstyle.fixedWidth = 0.9f;

        directstyle.normal.textColor = new Color(0.6f, 0.6f, 0.6f);
        directstyle.richText = true;
        directstyle.fixedWidth = 0.9f;

        if (GUILayout.Button("Refresh"))
        {
            if ((readDirSemaphore & countRefsSemaphore) == 0)
            {
                Task.Run(Refresh);
            }
        }

        PrintTree();
        GUILayout.Label($"files: {filesCount}, references: {guids.Count}");
    }

    private async Task Refresh()
    {
        readDirSemaphore = 0;
        countRefsSemaphore = 0;
        filesCount = 0;
        guids = new List<string>();

        rootDir = new DirEntry();
        rootDir.path = Directory.GetCurrentDirectory() + "/Assets";

        await ReadDir(rootDir);
        await CountRefsInDir(rootDir);

    }

    private async Task ReadDir(DirEntry dir)
    {
        readDirSemaphore++;
        foreach (var filePath in Directory.EnumerateFiles(dir.path))
        {
            var start = filePath.Substring(0, filePath.Length - 5);
            var last5 = filePath.Substring(filePath.Length - 5);
            var last6 = filePath.Substring(filePath.Length - 6);
            var last7 = filePath.Substring(filePath.Length - 7);

            if (last5 == ".meta")
            {
                if (!Directory.Exists(start))
                {
                    try
                    {
                        var lines = File.ReadLines(filePath).GetEnumerator();
                        lines.MoveNext();
                        lines.MoveNext();

                        var name = Path.GetFileName(start);
                        var guid = lines.Current.Substring(6, 32);
                        dir.files.Add(new FileEntry(name, guid));
                        filesCount++;
                    }
                    catch (System.Exception x)
                    {
                        Debug.LogError(x);
                    }
                }
            }
            else if (last6 == ".unity" || last7 == ".prefab")
            {
                ReadGuids(filePath);
            }
        }

        foreach (var subdirPath in Directory.EnumerateDirectories(dir.path))
        {
            var subdir = new DirEntry();
            subdir.path = subdirPath;

            dir.dirs.Add(subdir);
            await ReadDir(subdir);
        }
        readDirSemaphore--;
    }

    void ReadGuids(string path)
    {
        foreach (var line in File.ReadLines(path))
        {
            int index = line.IndexOf("guid: ");

            if (index >= 0)
            {
                guids.Add(line.Substring(index + 6, 32));
            }
        }
    }

    async Task CountRefsInDir(DirEntry dir)
    {
        countRefsSemaphore++;
        foreach (var file in dir.files)
        {
            CountRefs(file);
        }

        foreach (var subdir in dir.dirs)
        {
            await CountRefsInDir(subdir);
        }
        countRefsSemaphore--;
    }

    void CountRefs(FileEntry file)
    {
        int refs = 0;

        foreach (var guid in guids)
        {
            if (guid.Equals(file.GUID))
            {
                refs++;
            }
        }

        file.RefsCount = refs;
    }


    void PrintTree()
    {
        if (readDirSemaphore != 0)
        {
            GUILayout.Label("Loading data");
        }
        else if (countRefsSemaphore != 0)
        {
            GUILayout.Label($"Counting references");
        }
        else if (rootDir == null)
        {
            GUILayout.Label($"No data");
        }
        else
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            PrintDir(rootDir, 0);
            EditorGUILayout.EndScrollView();
        }
    }

    void PrintDir(DirEntry dir, int level)
    {
        var name = Path.GetFileName(dir.path);
        var offset = new string('│', level);

        GUILayout.Label($"<color=#b3b3b3ff> {offset}</color> {name}:", directstyle);

        foreach (var file in dir.files)
        {
            var block = true ? '├' : '└';
            var style = file.RefsCount != 0 ? normalstyle : noRefsStyle;
            GUILayout.Label($"<color=#b3b3b3ff> {offset}{block}</color> {file.Name}  {file.RefsCount}", style);
        }

        foreach (var subdir in dir.dirs)
        {
            PrintDir(subdir, level + 1);
        }
    }

    void printNode(string Label, string indent, bool last)
    {

    }
}

class FileEntry
{
    public FileEntry(string name, string guid)
    {
        Name = name;
        GUID = guid;
    }

    public string Name { get; private set; }
    public string GUID { get; private set; }
    public int RefsCount { get; set; }
}



class DirEntry
{
    public string path;
    public List<FileEntry> files = new List<FileEntry>();
    public List<DirEntry> dirs = new List<DirEntry>();
}