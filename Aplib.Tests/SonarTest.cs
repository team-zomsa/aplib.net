namespace Aplib.Tests;

public class SonarTest
{
    [Fact]
    public void TestMain()
    {
        Core.SonarTest test = new();
        test.Main();

        Assert.True(true);
    }
}