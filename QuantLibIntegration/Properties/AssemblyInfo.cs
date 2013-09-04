using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Mono.Addins;

[assembly: Addin("QuantLib Integration", "0.1.0", Category = "Core Extension")]
[assembly: AddinDependency("Fairmat", "1.0")]
[assembly: AddinAuthor("Fairmat SRL")]
[assembly: AddinDescription("Provides a bridge between Fairmat and Qauntlib making some of the models implemented in Quantlib available in Fairmat.")]


// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("QuantLibIntegration")]
[assembly: AssemblyDescription("Provides a bridge between Fairmat and Qauntlib making some of the models implemented in Quantlib available in Fairmat.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Fairmat SRL")]
[assembly: AssemblyProduct("QuantLibIntegration")]
[assembly: AssemblyCopyright("Copyright © Fairmat SRL 2013")]
[assembly: AssemblyTrademark("Fairmat")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("2aba50f8-3302-465f-b1ef-6bc44e93fd85")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
