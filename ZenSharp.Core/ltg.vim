" Vim syntax file
" Language:         BNF
" Maintainer:       Michael Brailsford, Alexander Ulitin
" Last Change:      Arpril 03, 2014

" Quit when a syntax file was already loaded	{{{
if version < 600
  syntax clear
elseif exists("b:current_syntax")
  finish
endif
"}}}

syn match bnfNonTerminal "<\a\w*>" contained
syn region bnfProduction start="^\s*\a" end="::="me=e-3 contained
syn region bnfComment start="^\s*//" end="$" contained
syn match bnfOr "|\|{\|}\|=" contained
syn match bnfSeperator "::=" contained
syn match bnfComment "#.*$" contained
syn match bnfQuoted #".*"# contains=bnfNonTerminal,bnfProduction,bnfSeperator,bnfLiteral,bnfTerminalRangeDelim,bnfTerminalRange
syn match bnfQuoted #'.*'# contains=bnfNonTerminal,bnfProduction,bnfSeperator,bnfLiteral,bnfTerminalRangeDelim,bnfTerminalRange
syn match bnfLiteral #"[ \w]*"# contained
syn match bnfTerminal "^.*$" contains=bnfNonTerminal,bnfProduction,bnfSeperator,bnfOr,bnfComment,bnfLiteral,bnfTerminalRangeDelim,bnfTerminalRange,bnfQuoted
syn keyword bnfScopeKw scope contained
syn match bnfScope #scope[^{]*{# contains=bnfScopeKw,bnfOr

hi link bnfNonTerminal	Type
hi link bnfProduction 	Identifier
hi link bnfOr Operator
hi link bnfSeperator 	PreProc
hi link bnfTerminal 	Constant
hi link bnfComment 		Comment
hi link bnfScopeKw Keyword
hi link bnfTerminalRange bnfTerminal
hi link bnfQuoted bnfTerminal
hi link bnfLiteral 	 	String

