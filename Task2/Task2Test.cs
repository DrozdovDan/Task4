using System.Text;
using NUnit.Framework;
using static NUnit.Framework.Assert;
using static Task2.Task2;
using static System.Text.Encoding;

namespace Task2;

public class Tests
{
    [Test]
    public void Main1Test()
    {
        var tmpFileName = Path.GetTempFileName();
        try
        {
            File.Copy("C:/Users/Admin/RiderProjects/Class4/Task2/text-utf-8.txt", tmpFileName, true);
            Main(new[] { tmpFileName, "utf-8", "UTF-16LE" });
            That(File.ReadAllBytes(tmpFileName), Is.EqualTo(File.ReadAllBytes("C:/Users/Admin/RiderProjects/Class4/Task2/text-utf-16.txt")));
        }
        finally
        {
            File.Delete(tmpFileName);            
        }
    }

    [Test]
    public void Main2Test()
    {
        var tmpFileName = Path.GetTempFileName();
        try
        {
            File.Copy("C:/Users/Admin/RiderProjects/Class4/Task2/text-utf-16.txt", tmpFileName, true);
            Main(new[] { tmpFileName, "UTF-16LE", "utf-8"});
            That(File.ReadAllText(tmpFileName), Is.EqualTo(File.ReadAllText("C:/Users/Admin/RiderProjects/Class4/Task2/text-utf-8.txt")));
        }
        finally
        {
            File.Delete(tmpFileName);            
        }
    }
}
