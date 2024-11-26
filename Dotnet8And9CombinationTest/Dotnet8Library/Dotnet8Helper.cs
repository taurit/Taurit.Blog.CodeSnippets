using System.Runtime.Serialization.Formatters.Binary;

namespace Dotnet8Library;

public static class Dotnet8Helper
{
    public static int Add(int a, int b)
    {
        return a + b;
    }

    public static void UseBinaryFormatter()
    {
#pragma warning disable SYSLIB0011
        var serializer = new BinaryFormatter();
#pragma warning restore SYSLIB0011
        var stream = new MemoryStream();
        var obj = new object();
        serializer.Serialize(stream, obj);
    }
}