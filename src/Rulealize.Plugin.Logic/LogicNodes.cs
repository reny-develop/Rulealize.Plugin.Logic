// Copyright (c) 2026 Reny
// Licensed under the Apache License, Version 2.0.

using System.Collections.Immutable;
using Rulealize.Abstraction.Building;
using Rulealize.Abstraction.Evaluation;
using Rulealize.Abstraction.Nodes;
using Rulealize.Abstraction.Values;

namespace Rulealize.Plugin.Logic
{
    /// <summary>Conjunction over <c>all</c>, short-circuiting on the first false operand.</summary>
    /// <remarks>
    /// Operands are evaluated strictly in array order and that order is part of the
    /// contract, not an implementation detail. It is the only way a rule author can put a
    /// cheap test in front of an expensive one — Othello's placement guard checks that a
    /// square is empty before it walks eight rays out of it, and on a crowded board that
    /// ordering is what keeps the ray walk off most of the squares.
    /// </remarks>
    internal sealed class AndNode(ImmutableArray<ExpressionNode> operands) : ExpressionNode
    {
        public static ExpressionNode Build(INodeBuildContext context) =>
            new AndNode(context.RequireExpressionArray("all"));

        public override RuleValue Evaluate(IEvaluationContext context)
        {
            for (int i = 0; i < operands.Length; i++)
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                if (!operands[i].Evaluate(context).AsBoolean($"logic.and.all[{i}]"))
                {
                    return RuleValue.False;
                }
            }

            return RuleValue.True;
        }
    }

    /// <summary>Disjunction over <c>any</c>, short-circuiting on the first true operand.</summary>
    /// <remarks>Empty is false, the identity of disjunction.</remarks>
    internal sealed class OrNode(ImmutableArray<ExpressionNode> operands) : ExpressionNode
    {
        public static ExpressionNode Build(INodeBuildContext context) =>
            new OrNode(context.RequireExpressionArray("any"));

        public override RuleValue Evaluate(IEvaluationContext context)
        {
            for (int i = 0; i < operands.Length; i++)
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                if (operands[i].Evaluate(context).AsBoolean($"logic.or.any[{i}]"))
                {
                    return RuleValue.True;
                }
            }

            return RuleValue.False;
        }
    }

    /// <summary>Negation of <c>value</c>.</summary>
    internal sealed class NotNode(ExpressionNode operand) : ExpressionNode
    {
        public static ExpressionNode Build(INodeBuildContext context) =>
            new NotNode(context.RequireExpression("value"));

        public override RuleValue Evaluate(IEvaluationContext context) =>
            RuleValue.Boolean(!operand.Evaluate(context).AsBoolean("logic.not.value"));
    }

    /// <summary>True when an odd number of the operands in <c>of</c> are true.</summary>
    /// <remarks>
    /// The one operation here that does not short-circuit: parity is not decided until
    /// every operand has been seen. Keep expensive expressions out of it.
    /// </remarks>
    internal sealed class XorNode(ImmutableArray<ExpressionNode> operands) : ExpressionNode
    {
        public static ExpressionNode Build(INodeBuildContext context) =>
            new XorNode(context.RequireExpressionArray("of"));

        public override RuleValue Evaluate(IEvaluationContext context)
        {
            bool parity = false;
            for (int i = 0; i < operands.Length; i++)
            {
                context.CancellationToken.ThrowIfCancellationRequested();
                parity ^= operands[i].Evaluate(context).AsBoolean($"logic.xor.of[{i}]");
            }

            return RuleValue.Boolean(parity);
        }
    }
}
