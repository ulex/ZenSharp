ReSharper ZenSharp plugin
==============

A shortcuts language for defining ReSharper live template items. You can
specify your own live template scheme using flexible language.

How it's work:

![example1](https://raw.githubusercontent.com/ulex/ZenSharp/master/doc/screen1.gif)

![example2](https://raw.githubusercontent.com/ulex/ZenSharp/master/doc/screen2.gif)

How do I get it?
---
You can install directly into ReSharper 8.2 or 9.0 via the Extension Manager in the
ReSharper menu. (8.2 support removed, but binary version still available in R# gallery)

Predifined templates
---
ZenSharp shipped with a lot of standart templates.

`$END$` means cursor position after expand template by pressing Tab.

**Examples** in scope, where method definition is allowed:

<!-- btw: «how to memo» really non readable markdown, sorry :) -->
| shortcut | expand to                                              | how to memo                                                     |
|----------|--------------------------------------------------------|-----------------------------------------------------------------|
| pps      | `public string $name$ { get; private set; } $END$`     | **p**ublic **p**roperty **s**tring                              |
| ppsAge   | `public string Age {get; private set;} $END$`          | **p**ublic **p**roperty **s**tring Name                         |
| pps+     | `public string $name$ { get; set; } $END$`             | **p**ublic **p**roperty **s**tring more access!                 |
| ppsA+p   | `public string A {get; protected set;} $END$`          | **p**ublic **p**roperty **s**tring more access!**p**rotected    |
| \_rs     | `private readonly string $name$; $END$ `               | [**_** is private] **r**eadonly **s**tring                      |
| pvm      | `public virtual void $name$($END$) { } `               | **p**ublic **v**irtual **m**ethod                               |
| pM~s     | `public static IEnumerable<string> $name$($END$) { } ` | public [static **M**ethod] [**~** is IEnumerable] of **s**tring |
| pamb     | `public abstract bool $name$($END$) { } `              | **p**ublic **a**bstract **m**ethod **b**ool                     |


**Examples** where type declaration is allowed:

| shortcut | expand to                                | how to memo                     |
|----------|------------------------------------------|---------------------------------|
| pi       | `public interface $name$ { $END$ }`      | **p**ublic **i**nterface        |
| psc      | `public sealed class $name$ { $END$ }`   | **p**ublic **s**ealed **c**lass |
| pc:t     | `public class $name$ : $type$ { $END$ }` | **p**ublic  **c**lass **:t**ype |
| ie       | `internal enum $name$ { $END$ }`         | **i**nternal **e**num           |

**Hint**: you can always write variable name after shortcut. For example, you can type `ppsPropertyName` and
by pressing tab it magically expand to `public string PropertyName {get; set; }`.


#### Access
| shortcut | expand to |
|----------|-----------|
| p        | public    |
| _        | private   |
| i        | internal  |
| P        | protected |


#### Types
| shortcut | expand to | note                                  |
|----------|-----------|---------------------------------------|
| t        | $type$    | ask user for custom type after expand |

##### Primitive types
| shortcut | expand to |
|----------|-----------|
| s        | string    |
| by       | byte      |
| b        | bool      |
| dt       | DateTime  |
| d        | double    |
| i        | int       |
| ui       | uint      |
| g        | Guid      |
| dc       | decimal   |
| b?       | bool?     |


##### Generic types
| shortcut | expand to       |
|----------|-----------------|
| l        | IList<T1>       |
| ~        | IEnumerable<T1> |
| sl       | SortedList<T1>  |
| di       | Dictionary<T1>  |


Rules
---
Simplest rule for expand text `cw` into `System.Console.WriteLine($END$)`(this
rule is ordinary ReSharper live template) will looks like `start ::=
"cw"="System.Console.WriteLine($END$)"`

More complex rule, a sort of predifined live template for define class in scope,
where class declaration is allowed. In ordinary live templates, if you want to
specify class access before executing live template, you must define
independed live templates one for `"public class"`=`pc`, another for `"internal class"=ic`
But class can be also be sealed. Or static. And same access modifier can be
applied to interface and enum.

In ZenSharp config you can simple define rules like formal grammar.
For class declaration this definition will looks like:

    space  ::= " "
    cursor ::= "$END$"
    identifier ::= <name default="$name$" macros = "complete()">

    access ::= (internal=i | public=p | private=_ | protected=P) space
    class  ::= access ["sealed "=s] ("class"=c | "static class"=C) space body
    body   ::= identifier "{" cursor "}"

    scope "InCSharpTypeAndNamespace"
    {
      start = class | interface
    }

More complex example available in predefined templates file.

How to compile
---
ZenSharp written in C# and Nemerle programming language. You can install it from Nemerle website http://nemerle.org/Downloads .
ZenSharp depend on nuget packages ReSharper.SDK and nunit.framwork. Use nuget package restore for them.

After this, ZenSharp can be built either msbuild or Visual Studio.

If you want to build nupkg you also need ILmerge.

<!--
vim:tw=140:
-->
