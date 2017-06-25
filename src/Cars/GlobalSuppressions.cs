// NOTE (Cameron): This file can be removed once StyleCop is updated to cope with the new tuple language features in C# 7.
using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
    "StyleCop.CSharp.SpacingRules",
    "SA1008:Opening parenthesis must be spaced correctly",
    Justification = "This StyleCop rule does not yet work with System.ValueTuple in C# 7.")]

[assembly: SuppressMessage(
    "StyleCop.CSharp.SpacingRules",
    "SA1009:Closing parenthesis must be spaced correctly",
    Justification = "This StyleCop rule does not yet work with System.ValueTuple in C# 7.")]