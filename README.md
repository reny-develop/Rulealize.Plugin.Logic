# Rulealize.Plugin.Logic

Boolean operations for [Rulealize](https://github.com/reny-develop/Rulealize) rule sets.

| | |
| --- | --- |
| Plugin id | `Rulealize.Plugin.Logic` |
| Namespace | `logic` |
| Reserved prefix | none |
| Depends on | `Rulealize.Abstraction` |
| Specification | [doc/specification.md](doc/specification.md) |

`logic.and`, `logic.or`, `logic.not`, `logic.xor` — all expressions, all `Bool` in and
`Bool` out. Everything here builds a condition. Choosing a branch is a different plugin's
job (`Rulealize.Plugin.Branch`), which is why a rule set that only states constraints can
load this one and not that one.

Two things the specification settles that are worth knowing before you read it. `and` and
`or` evaluate their operands strictly in array order and stop as soon as the answer is
decided — that guarantee is the only handle a rule set has on its own cost. And there is no
truthiness: an operand that is not a boolean is an evaluation error, so null and zero are
not false.

## Building

`Rulealize.Abstraction` is not on nuget.org yet, so `NuGet.config` points at a folder
feed. Produce it from the abstraction repository first:

```
dotnet pack path\to\Rulealize.Abstraction\src\Rulealize.Abstraction -c Release -o path\to\LocalNuGet
```

with `LocalNuGet` a sibling of this repository. Then `dotnet build`.

## License

Apache-2.0.
