// -----------------------------------------------------------------------
// <copyright file="Program.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MsBuildFindDuplicateProjectFileNames
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    class Program
    {
        static void Main(string[] args)
        {
            int errorCode = 0;

            if (args.Any())
            {
                string command = args.First().ToLowerInvariant();

                if (command.Equals("-?") || command.Equals("/?") || command.Equals("-help") || command.Equals("/help"))
                {
                    errorCode = ShowUsage();
                }
                else
                {
                    if (Directory.Exists(command))
                    {
                        string targetPath = command;
                        errorCode = PrintToConsole(command);
                    }
                    else
                    {
                        string error = string.Format("The specified path `{0}` is not valid.", command);
                        Console.WriteLine(error);
                        errorCode = 1;
                    }
                }
            }
            else
            {
                // This was a bad command
                errorCode = ShowUsage();
            }

            Environment.Exit(errorCode);
        }

        private static int ShowUsage()
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine("Scans given directory for MsBuild Projects; reporting any duplicates it finds.");
            message.AppendLine("Invalid Command/Arguments. Valid commands are:");
            message.AppendLine();
            message.AppendLine("[directory]    - [READS] Spins through the specified directory and all\n" +
                               "                 subdirectories for Project files; prints any duplicate\n" +
                               "                 names it finds. Returns the number of duplicates.");
            Console.WriteLine(message);
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
