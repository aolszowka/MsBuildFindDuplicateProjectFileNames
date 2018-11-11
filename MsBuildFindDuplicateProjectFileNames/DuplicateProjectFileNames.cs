// -----------------------------------------------------------------------
// <copyright file="DuplicateProjectFileNames.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MsBuildFindDuplicateProjectFileNames
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    static class DuplicateProjectFileNames
    {
        internal static IEnumerable<KeyValuePair<string, IDictionary<string, string>>> Execute(string targetDirectory)
        {
            IDictionary<string, IDictionary<string, string>> pfnDictionary = LoadProjectFileNameDictionary(targetDirectory);

            // Now filter the above dictionary just to places where we have duplicates
            IEnumerable<KeyValuePair<string, IDictionary<string, string>>> duplicatedProjectNamesEntries = pfnDictionary.Where(kvp => kvp.Value.Count > 1);

            return duplicatedProjectNamesEntries;
        }

        private static IDictionary<string, IDictionary<string, string>> LoadProjectFileNameDictionary(string targetDirectory)
        {
            IEnumerable<string> projectsInDirectory = GetProjectsInDirectory(targetDirectory);

            IDictionary<string, IDictionary<string, string>> distinctProjects = new Dictionary<string, IDictionary<string, string>>(StringComparer.InvariantCultureIgnoreCase);

            foreach (string projectPath in projectsInDirectory)
            {
                // Get the GUID of this project
                string projectGuid = MSBuildUtilities.GetMSBuildProjectGuid(projectPath);

                // Now get the name of the project
                string projectName = Path.GetFileNameWithoutExtension(projectPath);

                IDictionary<string, string> duplicatedProjects;

                if (distinctProjects.ContainsKey(projectName))
                {
                    duplicatedProjects = distinctProjects[projectName];
                }
                else
                {
                    duplicatedProjects = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                    distinctProjects.Add(projectName, duplicatedProjects);
                }

                duplicatedProjects.Add(projectPath, projectGuid);
            }

            return distinctProjects;
        }

        /// <summary>
        /// Gets all Project Files that are understood by this
        /// tool from the given directory and all subdirectories.
        /// </summary>
        /// <param name="targetDirectory">The directory to scan for projects.</param>
        /// <returns>All projects that this tool supports.</returns>
        static IEnumerable<string> GetProjectsInDirectory(string targetDirectory)
        {
            HashSet<string> supportedFileExtensions = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) { ".csproj", ".vbproj", ".synproj" };

            return
                Directory
                .EnumerateFiles(targetDirectory, "*proj", SearchOption.AllDirectories)
                .Where(currentFile => supportedFileExtensions.Contains(Path.GetExtension(currentFile)));
        }
    }
}
