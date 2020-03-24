namespace ParallelSearchTestTool
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using log4net;
    using log4net.Config;
    using ParallelSearchLibrary.List;
    using ParallelSearchLibrary.Trie;

    internal class Program
    {
        private static readonly string[] AutomaticBenchmarkArguments = { "auto", "-a", "--auto", "benchmark", "-b", "--benchmark" };
        private static readonly string[] HelpArguments = { "help", "-h", "--help", "?", "/?", "/help", "man" };
        private static readonly ILog Logger = LogManager.GetLogger(typeof(Program));

        public static void Main(string[] args)
        {
            AddLogger();
            string firstArgument = string.Empty;
            if (args.Any())
            {
                firstArgument = args.FirstOrDefault() ?? string.Empty;
                if (HelpArguments.Contains(firstArgument, StringComparer.OrdinalIgnoreCase))
                {
                    PrintHelp();
                    return;
                }
            }

            PrintGreeting();
            Logger.Debug("Starting Trie Test Tool...");

            var trieManager = new TrieManager();
            var listCreator = new ListCreator();
            var mainMenu = new MainMenu(listCreator, trieManager);

            if (AutomaticBenchmarkArguments.Contains(firstArgument, StringComparer.OrdinalIgnoreCase))
            {
                mainMenu.RunAutomaticBenchmark();
                return;
            }
            else
            {
                mainMenu.StartManualMode();
            }
        }

        private static void AddLogger()
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4netConfig.xml"));
        }

        private static void PrintGreeting()
        {
            Logger.Info("-----------------------------------------------------------------------------------------------------------------------");
            Logger.Info("                                                                                                                       ");
            Logger.Info(@"               __                           __                   __        __                   ___                    ");
            Logger.Info(@"              /\ \__         __            /\ \__               /\ \__    /\ \__               /\_ \                   ");
            Logger.Info(@"              \ \ ,_\  _ __ /\_\     __    \ \ ,_\    __    ____\ \ ,_\   \ \ ,_\   ___     ___\//\ \                  ");
            Logger.Info(@"               \ \ \/ /\`'__\/\ \  /'__`\   \ \ \/  /'__`\ /',__\\ \ \/    \ \ \/  / __`\  / __`\\ \ \                 ");
            Logger.Info(@"                \ \ \_\ \ \/ \ \ \/\  __/    \ \ \_/\  __//\__, `\\ \ \_    \ \ \_/\ \L\ \/\ \L\ \\_\ \_               ");
            Logger.Info(@"                 \ \__\\ \_\  \ \_\ \____\    \ \__\ \____\/\____/ \ \__\    \ \__\ \____/\ \____//\____\              ");
            Logger.Info(@"                  \/__/ \/_/   \/_/\/____/     \/__/\/____/\/___/   \/__/     \/__/\/___/  \/___/ \/____/              ");
            Logger.Info("                                                                                                                       ");
            Logger.Info("-----------------------------------------------------------------------------------------------------------------------");
        }

        private static void PrintHelp()
        {
            Logger.Info("-----------------------------------------------");
            Logger.Info(" Help Table of the Trie Test Tool:");
            Logger.Info("-----------------------------------------------");
            Logger.Info(" * '' ........... no arguments for manual mode");
            Logger.Info(" * 'help' ................. displays this help");
            Logger.Info(" * 'benchmark' ... runs an automatic benchmark");
        }
    }
}