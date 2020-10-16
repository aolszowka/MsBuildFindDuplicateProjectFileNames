# MsBuildFindDuplicateProjectFileNames
Utility to find MSBuild Project System Files which have duplicate file names.

## Background
In the MSBuild Project System the file name of the Project File is inserted into any project that has a ProjectReference tag in the Name Property (See [Microsoft Docs: Common MSBuild Project Items - ProjectReference](https://docs.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items?view=vs-2017#projectreference))

This only becomes a problem when you attempt to add these projects into a single Solution File as Visual Studio will alert you to the duplicated project names.

## When To Use This Tool
You can use this tool to find duplicate MSBuild Project System file names in a source tree.

Note that if you rename a file you will need to update all MsBuild Project System files that had a ProjectReference to the renamed file, see the sister tool https://github.com/aolszowka/MsBuildProjectReferenceFixer for a utility to fix this. In addition you will also need to fix any Visual Studio Solution that was generated, see the sister tool https://github.com/aolszowka/VisualStudioSolutionFixer to fix this.

## Usage
```
Usage: MsBuildFindDuplicateProjectFileNames C:\DirectoryWithProjects [-xml]

Scans given directory for MsBuild Projects; reporting any duplicate Project
Names it finds.

Arguments:

              <>            The directory to scan for MSBuild Projects
      --xml                  Produce Output in XML Format
  -?, -h, --help             Show this message and exit
```

## Hacking
The most likely change you will want to make is changing the supported project files. Because this tooling is operating on file names any extension is supported.

See DuplicateProjectFileNames.GetProjectsInDirectory(string) for the place to modify this.

## Contributing
Pull requests and bug reports are welcomed so long as they are MIT Licensed.

## License
This tool is MIT Licensed.

## Third Party Licenses
This project uses other open source contributions see [LICENSES.md](LICENSES.md) for a comprehensive listing.
