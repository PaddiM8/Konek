namespace Konek.Server.Core;

class CommonPaths
{
    public static string DataFolder
        => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "konek");

    public static string AppKey
        => Path.Combine(DataFolder, "appkey.txt");

    public static string Database
        => Path.Combine(DataFolder, "konek.db");
}