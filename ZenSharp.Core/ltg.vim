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

syn match bnfNonTerminal "\a\w*"
syn region bnfProduction start="^\s*\a" end="::="me=e-3
syn region bnfComment start="^\s*//" end="$"
syn region ltgSub start="<\s*" end="\>" contains=bnfString
syn match bnfOr "|\|{\|}\|="
syn match bnfSeperator "::="
syn match bnfComment "#.*$"
syn region bnfString  start=+"+ skip=+""+ end=+"+

syn match bnfTerminal "\\w"
syn keyword bnfScopeKw scope contained
syn match bnfScope #scope[^{]*{# contains=bnfScopeKw,bnfOr,bnfString

hi link bnfNonTerminal	Type
hi link bnfProduction 	Identifier
hi link bnfOr Operator
hi link bnfSeperator 	PreProc
hi link bnfTerminal 	Constant
hi link bnfComment 		Comment
hi link bnfScopeKw Keyword
hi link bnfTerminalRange bnfTerminal
hi link bnfString String
hi link bnfLiteral 	 	String
hi link ltgSub Conditional

