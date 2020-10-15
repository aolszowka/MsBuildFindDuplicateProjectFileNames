// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018-2020. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MsBuildFindDuplicateProjectFileNames
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using MsBuildFindDuplicateProjectFileNames.Properties;

    using NDesk.Options;

    class Program
    {
        static void Main(string[] args)
        {

            string targetDirectory = string.Empty;
            bool showHelp = false;

            OptionSet p = new OptionSet()
            {
                { "<>", Strings.TargetDirectoryArgument, v => targetDirectory = v },
                { "?|h|help", Strings.HelpDescription, v => showHelp = v != null },
            };

            try
            {
                p.Parse(args);
            }
            catch (OptionException)
            {
                Console.WriteLine(Strings.ShortUsageMessage);
                Console.WriteLine($"Try `{Strings.ProgramName} --help` for more information.");
                Environment.Exit(21);
            }

            if (showHelp || string.IsNullOrEmpty(targetDirectory))
            {
                int exitCode = ShowUsage(p);
                Environment.Exit(exitCode);
            }
            else
            {
                if (Directory.Exists(targetDirectory))
                {
                    Environment.ExitCode = PrintToConsole(targetDirectory);
                }
                else
                {
                    string error = string.Format(Strings.InvalidDirectoryArgument, targetDirectory);
                    Console.WriteLine(error);
                    Environment.ExitCode = 9009;
                }
            }
        }

        private static int ShowUsage(OptionSet p)
        {
            Console.WriteLine(Strings.ShortUsageMessage);
            Console.WriteLine();
            Console.WriteLine(Strings.LongDescription);
            Console.WriteLine();
            Console.WriteLine($"              <>            {Strings.TargetDirectoryArgument}");
            p.WriteOptionDescriptions(Console.Out);
            return 21;
        }

        static int PrintToConsole(string targetDirectory)
        {
            int duplicatedProjectCount = 0;
            IEnumerable<KeyValuePair<string, IDictionary<string, string>>> duplicateProjectFileNames = DuplicateProjectFileNames.Execute(targetDirectory);

            foreach (KeyValuePair<string, IDictionary<string, string>> duplicateProjectFileNameEntry in duplicateProjectFileNames)
            {
                duplicatedProjectCount++;
                Console.WriteLine($"{duplicateProjectFileNameEntry.Key}:");

                foreach (KeyValuePair<string, string> duplicatedProjectEntry in duplicateProjectFileNameEntry.Value)
                {
                    Console.WriteLine($"{duplicatedProjectEntry.Key}\t{duplicatedProjectEntry.Value}");
                }

                Console.WriteLine();
            }

            return duplicatedProjectCount;
        }
    }
}
