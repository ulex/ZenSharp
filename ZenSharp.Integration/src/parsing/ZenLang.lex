using System.Collections;
using JetBrains.Util;
using JetBrains.Text;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.RegExp.ClrRegex;
using JetBrains.ReSharper.Psi.RegExp.ClrRegex.Parsing;


%%

%unicode

%init{
   myCurrentTokenType = null;
%init}

%namespace JetBrains.ReSharper.Psi.RegExp.ClrRegex.Parsing
%class ClrRegexLexerGenerated
%public
%implements IIncrementalLexer
%function _locateToken
%virtual
%type TokenNodeType
%eofval{
  myCurrentTokenType = null; return myCurrentTokenType;
%eofval}

%include ..\..\..\..\..\..\Tasks\CsLex\Unicode.lex

CARRIAGE_RETURN_CHAR=\u000D	// \r
LINE_FEED_CHAR=\u000A				// \n
NEW_LINE_PAIR={CARRIAGE_RETURN_CHAR}{LINE_FEED_CHAR}
NEW_LINE_CHAR=({CARRIAGE_RETURN_CHAR}|{LINE_FEED_CHAR}|(\u0085)|(\u2028)|(\u2029))

DIGIT=[0-9]
HEX_DIGIT=({DIGIT}|[A-Fa-f])
LETTER=({UNICODE_LL}|{UNICODE_LM}|{UNICODE_LO}|{UNICODE_LT}|{UNICODE_LU}|{UNICODE_NL})
WHITESPACE=({NEW_LINE_PAIR}|{NEW_LINE_CHAR}|{UNICODE_ZS}|(\u0009)|(\u000B)|(\u000C)|(\u200B)|(\uFEFF))

ESCAPE_SYMBOL=\\

NAMED_BACKREFERENCE={ESCAPE_SYMBOL}k
UNICODE_NAMED_BLOCK={ESCAPE_SYMBOL}p
NONUNICODE_NAMED_BLOCK={ESCAPE_SYMBOL}P
ESCAPE_B={ESCAPE_SYMBOL}b		// \b in context [] is escape symbol 'backspace', otherwise \b is anchor

ESCAPE={ESCAPE_SYMBOL}([atrvfne]|x{HEX_DIGIT}{HEX_DIGIT}|{DIGIT}{DIGIT}{DIGIT}|u{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}{HEX_DIGIT}|c[A-Za-z])
CLASS={ESCAPE_SYMBOL}[wWsSdD]
ANCHOR={ESCAPE_SYMBOL}[AZzGB]
INVALID={ESCAPE_SYMBOL}({LETTER}|_)
BACKREFERENCE=({ESCAPE_SYMBOL}{DIGIT}+)

LETTER_OPTION=[imnsx]
OTHER_ESCAPE={ESCAPE_SYMBOL}.
OTHER=(.|\n)

BORDER=\u001C
COMMENT=\(\?#[^\)]*\)
COMMENT_LINE=#[^{LINE_FEED_CHAR}{BORDER}]*

%%

<YYINITIAL> {NAMED_BACKREFERENCE}			{ return MakeToken (ClrRegexTokenTypes.NAMED_BACKREFERENCE); }
<YYINITIAL> {UNICODE_NAMED_BLOCK}			{ return MakeToken (ClrRegexTokenTypes.UNICODE_NAMED_BLOCK); }
<YYINITIAL> {NONUNICODE_NAMED_BLOCK}	{ return MakeToken (ClrRegexTokenTypes.NONUNICODE_NAMED_BLOCK); }

<YYINITIAL> {ESCAPE_SYMBOL}"b"	{ return MakeToken (ClrRegexTokenTypes.ESCAPE_LOWER_B); }
<YYINITIAL> {ESCAPE_SYMBOL}"z"	{ return MakeToken (ClrRegexTokenTypes.ESCAPE_LOWER_Z); }
<YYINITIAL> {ESCAPE_SYMBOL}"B"	{ return MakeToken (ClrRegexTokenTypes.ESCAPE_UPPER_B); }
<YYINITIAL> {ESCAPE_SYMBOL}"Z"	{ return MakeToken (ClrRegexTokenTypes.ESCAPE_UPPER_Z); }
<YYINITIAL> {ESCAPE_SYMBOL}"A"	{ return MakeToken (ClrRegexTokenTypes.ESCAPE_UPPER_A); }
<YYINITIAL> {ESCAPE_SYMBOL}"G"	{ return MakeToken (ClrRegexTokenTypes.ESCAPE_UPPER_G); }

<YYINITIAL> {ESCAPE}					{ return MakeToken (ClrRegexTokenTypes.ESCAPE); }
<YYINITIAL> {CLASS}						{ return MakeToken (ClrRegexTokenTypes.CLASS); }
<YYINITIAL> {ANCHOR}					{ return MakeToken (ClrRegexTokenTypes.ANCHOR); }
<YYINITIAL> {INVALID}					{ return MakeToken (ClrRegexTokenTypes.INVALID); }

<YYINITIAL> {BACKREFERENCE}		{ return MakeToken (ClrRegexTokenTypes.BACKREFERENCE); }
<YYINITIAL> {COMMENT}         { return MakeToken (ClrRegexTokenTypes.COMMENT); }
<YYINITIAL> {COMMENT_LINE}		{ return TryMakeCommentLineToken(); }
<YYINITIAL> {BORDER}		      { return MakeToken (ClrRegexTokenTypes.BORDER); }

<YYINITIAL> "^" { return MakeToken (ClrRegexTokenTypes.CAROT); }
<YYINITIAL> "$" { return MakeToken (ClrRegexTokenTypes.DOLLAR); }
<YYINITIAL> "+" { return MakeToken (ClrRegexTokenTypes.PLUS); }
<YYINITIAL> "*" { return MakeToken (ClrRegexTokenTypes.STAR); }
<YYINITIAL> "?" { return MakeToken (ClrRegexTokenTypes.QUESTION); }
<YYINITIAL> "." { return MakeToken (ClrRegexTokenTypes.DOT); }
<YYINITIAL> "|" { return MakeToken (ClrRegexTokenTypes.PIPE); }

<YYINITIAL> "," { return MakeToken (ClrRegexTokenTypes.COMMA); }
<YYINITIAL> "-" { return MakeToken (ClrRegexTokenTypes.DASH); }
<YYINITIAL> "!" { return MakeToken (ClrRegexTokenTypes.EXCLAMATION); }
<YYINITIAL> "=" { return MakeToken (ClrRegexTokenTypes.EQUAL); }
<YYINITIAL> "&" { return MakeToken (ClrRegexTokenTypes.AMPER); }
<YYINITIAL> "'" { return MakeToken (ClrRegexTokenTypes.APOSTROPHE); }
<YYINITIAL> "`" { return MakeToken (ClrRegexTokenTypes.GRAVEACCENT); }
<YYINITIAL> "_" { return MakeToken (ClrRegexTokenTypes.UNDERLINE); }
<YYINITIAL> ":" { return MakeToken (ClrRegexTokenTypes.COLON); }

<YYINITIAL> "[" { ProcessLeftBracket();  return MakeToken (ClrRegexTokenTypes.LBRACKET); }
<YYINITIAL> "]" { ProcessRightBracket(); return MakeToken (ClrRegexTokenTypes.RBRACKET); }

<YYINITIAL> "(" { return MakeToken (ClrRegexTokenTypes.LPARENTH); }
<YYINITIAL> ")" { return MakeToken (ClrRegexTokenTypes.RPARENTH); }
<YYINITIAL> "{" { return MakeToken (ClrRegexTokenTypes.LBRACE); }
<YYINITIAL> "}" { return MakeToken (ClrRegexTokenTypes.RBRACE); }

<YYINITIAL> ">" { return MakeToken (ClrRegexTokenTypes.GT); }
<YYINITIAL> "<" { return MakeToken (ClrRegexTokenTypes.LT); }

<YYINITIAL> {DIGIT}					{ return MakeToken (ClrRegexTokenTypes.DIGIT); }
<YYINITIAL> {LETTER_OPTION}	{ return MakeToken (ClrRegexTokenTypes.LETTER_OPTION); }
<YYINITIAL> {LETTER}				{ return MakeToken (ClrRegexTokenTypes.LETTER); }
<YYINITIAL> {WHITESPACE}		{ return MakeToken (ClrRegexTokenTypes.WHITESPACE); }
<YYINITIAL> {OTHER_ESCAPE}	{ return MakeToken (ClrRegexTokenTypes.ESCAPE); }
<YYINITIAL> {OTHER}					{ return MakeToken (ClrRegexTokenTypes.OTHER); }
<YYINITIAL> {ESCAPE_SYMBOL}	{ return MakeToken (ClrRegexTokenTypes.INVALID); }
