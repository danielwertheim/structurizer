using System.Runtime.InteropServices;
using System.Reflection;

#if DEBUG
[assembly: AssemblyProduct("Structurizer (Debug)")]
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyProduct("Structurizer (Release)")]
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyDescription("Structurizer - for key-value object extraction.")]
[assembly: AssemblyCompany("Daniel Wertheim")]
[assembly: AssemblyCopyright("Copyright © 2016 Daniel Wertheim")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]