﻿// -----------------------------------------------------------------------
// <copyright file="MSBuildUtilities.cs" company="Ace Olszowka">
//  Copyright (c) Ace Olszowka 2018-2020. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace MsBuildFindDuplicateProjectFileNames
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public static class MSBuildUtilities
    {
        internal static XNamespace msbuildNS = @"http://schemas.microsoft.com/developer/msbuild/2003";


        /// <summary>
        /// Extracts the Project GUID from the specified proj File.
        /// </summary>
        /// <param name="pathToProjFile">The proj File to extract the Project GUID from.</param>
        /// <returns>The specified proj File's Project GUID.</returns>
        public static string GetMSBuildProjectGuid(string pathToProjFile)
        {
            XDocument projFile = XDocument.Load(pathToProjFile);
            XElement projectGuid = projFile.Descendants(msbuildNS + "ProjectGuid").FirstOrDefault();

            if (projectGuid == null)
            {
                string exception = $"Project {pathToProjFile} did not contain a ProjectGuid.";
                throw new InvalidOperationException(exception);
            }

            return projectGuid.Value;
        }
    }
}
