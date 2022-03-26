using System.IO;

public static class PathUtils
{
    public static string Combine(string path1, string path2)
    {
        return Path.Combine(path1, path2);
    }

    public static string Combine(string path1, string path2, string path3)
    {
        return Combine(path1, Path.Combine(path2, path3));
    }

    public static string Combine(string path1, string path2, string path3, string path4)
    {
        return Combine(path1, path2, Path.Combine(path3, path4));
    }

    public static string Combine(params string[] paths)
    {
        if (paths != null && paths.Length != 0)
        {
            string path = paths[0];
            for (int i = 1, max = paths.Length; i < max; ++i)
            {
                path = Combine(path, paths[i]);
            }

            return path;
        }

        return null;
    }
}