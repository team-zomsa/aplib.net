# Contributing

Thank you for your interest in contributing to this project. In order to contribute to this project, we ask you to read this document to get acquainted with our style. 

## Formats

**Class names**

- Class name same as file name

# Modifiers, naming conventions & member order

## Access modifiers.

Access modifiers in order of priority when combining them ([Microsoft C# Docs: Code-style rule options](https://learn.microsoft.com/en-us/dotnet/fundamentals/code-analysis/code-style-rule-options?view=vs-2017)):

```csharp
public
private
protected
internal
file
static
extern
new
virtual
abstract
sealed
override
readonly
unsafe
required
volatile
async
```

**Examples:**

```csharp
private static readonly bool s_foo;
protected new virtual int bar;
public abstract class Baz { ... };
```

## Identifier names.

Naming conventions for identifiers ([Microsoft C# Docs: Identifier names](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/identifier-names), [Microsoft C# Docs: protected](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/protected)):

```csharp
namespace PascalCase { ... } // -------------- // **Namespaces**.
<TPascalCase> // ----------------------------- // **Generic types**. Prefix T.
interface IPascalCase { ... } // ------------- // **Interfaces**. Prefix I.
class PascalCase { ... } // ------------------ // **Classes**.
class PascalCaseAttribute : Attribute { ... }  // **Attributes**. Suffix Attribute.
... PascalCase( ... camelCase ) { ... } // --- // **Methods** and **method arguments**.
... PascalCase { get; set; } // -------------- // **Properties**.
const PascalCase; // ------------------------- // **Constants**.
... camelCase; // ---------------------------- // **Local variables**.
public static PascalCase; // --------------- // Public static **fields**. Prefix s_.
internal static _camelCase; // -------------- // Internal static **fields**. Prefix s_.
protected static _camelCase; // ------------- // Protected static **fields**. Prefix s_.
private static _camelCase; // --------------- // Private static **fields**. Prefix s_.
public PascalCase; // ------------------------ // Public nonstatic **fields**.
internal _camelCase: // ---------------------- // Internal nonstatic **fields**. Prefix _.
protected _camelCase; // --------------------- // Protected nonstatic **fields**. Prefix _.
private _camelCase; // ----------------------- // Private nonstatic **fields**. Prefix _.
```

**Additional naming conventions:**

- Use meaningful and descriptive names for variables, methods, and classes.
- Prefer clarity over brevity.
- Enum types use a singular noun for nonflags, and a plural noun for flags.
- Identifiers shouldn't contain two consecutive underscore (_) characters. Those names are reserved for compiler-generated identifiers.
- Avoid using abbreviations or acronyms in names, except for widely known and accepted abbreviations.
- Use meaningful and descriptive namespaces that follow the reverse domain name notation.
- Choose assembly names that represent the primary purpose of the assembly.
- Avoid using single-letter names, except for simple loop counters.
- Treat an acronym or abbreviation of three letters or more as a single word for capitalization. For example, `IterateBdiCycle` instead of `IterateBDICycle`.

## Order of members.

The ordering of class members is as follows (combination of [C# at Google Style Guide](https://google.github.io/styleguide/csharp-style.html) and [StyleCop](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/documentation/SA1201.md)).

**Within a class, struct, or interface:**

- Nested classes, enums, delegates and events.
- Static, const and readonly fields.
- Fields and properties.
- Constructors and finalizers.
- Methods.

**Within each group, elements should be in the following order:**

- Public.
- Internal.
- Protected internal.
- Protected.
- Private.
- Where possible, group interface implementations together.

**Within each of the access groups, order by static, then nonstatic:**

- Static
- Nonstatic

**Within each of the static/nonstatic groups of fields, order by readonly, then nonreadonly:**

- Readonly
- Nonreadonly

Within each group: Order on Name.
