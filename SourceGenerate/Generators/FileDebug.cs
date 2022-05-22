namespace SourceGenerate.Generators;

public static class FileDebug
{
    public static void Debug<T>(T s)
    {
        var stream =
            new StreamWriter(@"E:\MeProgram\C#\SourceGenerate\SourceGenerate.RealTimeTests\NewFile1.txt", true);
        stream.WriteLine(s);
        
        stream.Dispose();
    }
}