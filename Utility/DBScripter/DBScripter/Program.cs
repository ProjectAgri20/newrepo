using CommandLine;

namespace DBScripter
{
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments<Parameters>(args).MapResult(parameters => DBScripter.ScriptDatabase(parameters), _ => 1);
        }
    }
}
