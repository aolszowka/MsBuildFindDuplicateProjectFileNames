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
    using System.Linq;
    using System.Xml.Linq;

    using MsBuildFindDuplicateProjectFileNames.Properties;

    using NDesk.Options;

    class Program
    {
        static void Main(string[] args)
        {

            string targetDirectory = string.Empty;
            bool showHelp = false;
            bool xmlOutput = false;

            OptionSet p = new OptionSet()
            {
                { "<>", Strings.TargetDirectoryArgument, v => targetDirectory = v },
                { "xml", Strings.XmlOutputFlag, v => xmlOutput = v != null },
                { "?|h|help", Strings.HelpDescription, v => showHelp = v != null },
            };

            try
            {
                p.Parse(args);
            }
            catch (OptionException)
            {
                Console.WriteLine(Strings.ShortUsageMessage);
                Console.WriteLine($"Try `--help` for more information.");
                Environment.Exit(160);
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
                    KeyValuePair<string, IDictionary<string, string>>[] duplicateProjectFiles =
                        DuplicateProjectFileNames
                        .Execute(targetDirectory)
                        .ToArray();

                    if (xmlOutput)
                    {
                        PrintXmlToConsole(duplicateProjectFiles);
                    }
                    else
                    {
                        PrintToConsole(duplicateProjectFiles);
                    }

                    Environment.ExitCode = duplicateProjectFiles.Length;
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
            return 160;
        }

        static void PrintXmlToConsole(IEnumerable<KeyValuePair<string, IDictionary<string, string>>> duplicateProjects)
        {
            XDocument outputDocument = new XDocument(new XDeclaration("1.0", null, null));
            outputDocument.Add(new XElement("MsBuildFindDuplicateProjectFileNames"));

            foreach (KeyValuePair<string, IDictionary<string, string>> duplicateProject in duplicateProjects)
            {
                XElement projectElement = new XElement("Project", new XAttribute("Name", duplicateProject.Key));
                foreach (KeyValuePair<string, string> duplicatedProjectEntry in duplicateProject.Value)
                {
                    XElement duplicatedProjectElement = new XElement("Duplicate", new XAttribute("Path", duplicatedProjectEntry.Key), new XAttribute("GUID", duplicatedProjectEntry.Value));

                    projectElement.Add(duplicatedProjectElement);
                }
                outputDocument.Root.Add(projectElement);
            }

            Console.WriteLine(outputDocument.ToString());
        }

        static void PrintToConsole(IEnumerable<KeyValuePair<string, IDictionary<string, string>>> duplicateProjects)
        {
            foreach (KeyValuePair<string, IDictionary<string, string>> duplicateProject in duplicateProjects)
            {
                Console.WriteLine($"{duplicateProject.Key}:");

                foreach (KeyValuePair<string, string> duplicatedProjectEntry in duplicateProject.Value)
                {
                    Console.WriteLine($"{duplicatedProjectEntry.Key}\t{duplicatedProjectEntry.Value}");
                }

                Console.WriteLine();
            }
        }
    }
}
