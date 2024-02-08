namespace APLib.Core;

/// <summary>
/// Describes the APLib package.
/// </summary>
public class Aplib
{
    public string PackageName = "APLib";
    public int package_version = 1_0_0;

    public void LaadPakket() => Console.WriteLine("APLib is running...");
}
