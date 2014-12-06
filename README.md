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
ReSharper menu.

Rules
---
Simplest rule for expand text `cw` into `System.Console.WriteLine($END$)`(this
rule is ordinary ReSharper live template) will looks like `start ::=
"cw"="System.Console.WriteLine($END$)"`

More complex rule, a sort of predifined live template for define class in scope,
where class declaration is allowed. In ordinary live templates, if you want to
specify class acccess before executing live template, you must define
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
