# Rulealize.Plugin.Logic

Boolean operations for [Rulealize](https://github.com/reny-develop/Rulealize) rule sets.

| | |
| --- | --- |
| Plugin id | `Rulealize.Plugin.Logic` |
| Namespace | `logic` |
| Reserved prefix | none |
| Depends on | `Rulealize.Abstraction` |

Everything here is an expression that builds a condition. Choosing a branch is a
different plugin's job (`Rulealize.Plugin.Branch`), which is why a rule set that only
states constraints can load this one and not that one.

## Operations

| Operation | Shape | Result |
| --- | --- | --- |
| `logic.and` | `{ "op": "logic.and", "all": [ … ] }` | true when every operand is true; empty is true |
| `logic.or` | `{ "op": "logic.or", "any": [ … ] }` | true when any operand is true; empty is false |
| `logic.not` | `{ "op": "logic.not", "value": … }` | the negation |
| `logic.xor` | `{ "op": "logic.xor", "of": [ … ] }` | true when an odd number of operands are true; empty is false |

The key name changes with the operation — `all`, `any`, `of` — so that a node says what
it means twice over.

## Evaluation order is part of the contract

`logic.and` and `logic.or` evaluate their operands strictly in array order and stop as
soon as the answer is decided. Reordering them, or evaluating them in parallel, is not a
permitted optimisation.

This is not about speed in the abstract. It is the only handle a rule author has on cost:

```jsonc
{
  "op": "logic.and",
  "all": [
    { "op": "cmp.isNull", "value": { "op": "grid.at", "grid": "$board", "coord": "@at" } },
    { "op": "seq.any", "source": { "op": "def.call", "def": "flips", "args": { "at": "@at" } } }
  ]
}
```

The first operand reads one square. The second walks eight rays. `GetValidInputs`
evaluates this for all sixty-four squares of an Othello board, and as the board fills up
the first operand rejects more and more of them before the ray walk is ever reached.

`logic.xor` is the exception: parity is not decided until every operand has been seen, so
it never short-circuits.

## No truthiness

An operand that is not a boolean is an evaluation error. Null and zero are not false.

The reason is `grid.at`, which returns null for an empty square, for a square off the
board, and for a null coordinate alike. If null were falsy, a guard that meant to ask "is
this square black" would quietly answer "no" for reasons its author never considered.
Emptiness is written as `cmp.isNull`, explicitly.

## Building

`Rulealize.Abstraction` is not on nuget.org yet, so `NuGet.config` points at a folder
feed. Produce it from the abstraction repository first:

```
dotnet pack path\to\Rulealize.Abstraction\src\Rulealize.Abstraction -c Release -o path\to\LocalNuGet
```

with `LocalNuGet` a sibling of this repository. Then `dotnet build`.

## License

Apache-2.0.
