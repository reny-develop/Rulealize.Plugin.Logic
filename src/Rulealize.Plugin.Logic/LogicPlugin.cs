// Copyright (c) 2026 Reny
// Licensed under the Apache License, Version 2.0.

using Rulealize.Abstraction.Plugin;

namespace Rulealize.Plugin.Logic
{
    /// <summary>
    /// Boolean operations over the <c>logic</c> namespace.
    /// </summary>
    /// <remarks>
    /// <para>
    /// These are expressions that build a condition, not control structures. Choosing a
    /// branch is <c>Rulealize.Plugin.Branch</c>'s job, and a rule set that only states
    /// constraints can load this plugin without loading that one.
    /// </para>
    /// <para>
    /// No value is coerced to boolean. Null and zero are rejected rather than treated as
    /// false, so that the null an empty board square reads as cannot slip through a guard.
    /// </para>
    /// </remarks>
    public sealed class LogicPlugin : IRulealizePlugin
    {
        /// <inheritdoc />
        public PluginManifest Manifest { get; } =
            new("Rulealize.Plugin.Logic", new Version(1, 0, 0), "logic");

        /// <inheritdoc />
        public void Register(IPluginRegistry registry)
        {
            ArgumentNullException.ThrowIfNull(registry);

            registry.AddExpression("and", AndNode.Build);
            registry.AddExpression("or", OrNode.Build);
            registry.AddExpression("not", NotNode.Build);
            registry.AddExpression("xor", XorNode.Build);
        }
    }
}
