using System;
using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyTitle("Cars")]
[assembly: AssemblyDescription("A trivial car distance logging application.")]

[assembly: CLSCompliant(true)]
[assembly: InternalsVisibleTo("Cars.Tests")]

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]