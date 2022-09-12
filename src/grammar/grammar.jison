%{
  const { Type } = require("../interpreter/Abstract/Return")

  const { Print } = require('../interpreter/Instruction/Print')
  const { If } = require('../interpreter/Instruction/If')
  const { While } = require('../interpreter/Instruction/While')
  const { Declaration } = require('../interpreter/Instruction/Declaration')

  const { Literal } = require("../interpreter/Expressions/Literal")
  const { Variable } = require("../interpreter/Expressions/Variable")
  const { Arithmetic } = require("../interpreter/Expressions/Arithmetic")
  const { Relational } = require("../interpreter/Expressions/Relational")
%}

/* LEXICAL ANALYSIS */
%lex
%options case-sensitive

integer [0-9]+
number {integer}("."{integer})?
id ([a-zA-Z_])[a-zA-Z0-9_]*
stringL \"[^"]+\"

%%
\s+ /* ignore whitespace */

/* COMMENTS */
"#".* return

/* LITERALS */
{integer} return "INTEGER"
{number} return "DOUBLE"
{stringL} return "STRING"
"True" return "TRUE"
"False" return "FALSE"

/* OPERATORS */
/* ARITMETIC SYMBOLS */
"*"                         return 'TIMES'
"/"                         return 'DIVISION'
"+"                         return 'PLUS'
"-"                         return 'MINUS'
"%"                         return 'MOD'

/* RELATIONAL SYMBOLS */
"<="                        return 'LE'
">="                        return 'GE'
"=="                        return 'EQ'
"!="                        return 'NE'
"<"                         return 'LT'
">"                         return 'GT'

/* GENERAL SYMBOLS */
"="                         return 'ASSIGN'
":"                         return 'COLON'
"("                         return 'LPAREN'
")"                         return 'RPAREN'
","                         return 'COMMA'

/* KEYWORDS */
"if"                        return 'IF'
"else"                      return 'ELSE'
"while"                     return 'WHILE'
"print"                     return 'PRINT'

/* IDENTIFIERS */
{id}                        return 'ID'
<<EOF>>                     return 'EOF'

. {
  console.log(`Error: ${yytext} at line ${yylloc.first_line}`);
}

/lex

/* PRECEDENCE */
%left 'EQ' 'NE'
%left 'LT' 'GT' 'LE' 'GE'
%left 'PLUS' 'MINUS'
%left 'TIMES' 'DIVISION' 'MOD'

/* GRAMMAR */
%start START

%%

START:  INSTRUCTIONS EOF  { return $1; }
      | EOF               { $$ = null; }
  ;

INSTRUCTIONS: INSTRUCTIONS INSTRUCTION  { $$ = [...$1, $2]; }
            | INSTRUCTION               { $$ = [$1]; }
  ;

INSTRUCTION: PRINT_STMT   { $$ = $1; }
            | ASSIGN_STMT { $$ = $1; }
            | IF_STMT     { $$ = $1; }
            | WHILE_STMT  { $$ = $1; }
            | EXPRESSION  { $$ = $1; }
  ;

PRINT_STMT: PRINT LPAREN EXPRESSION RPAREN      { $$ = new Print($3, @1.first_line, @1.first_column) }
  ;

ASSIGN_STMT: ID ASSIGN EXPRESSION { $$ = new Declaration($1, $3, @1.first_line, @1.first_column) }
  ;

IF_STMT: IF EXPRESSION COLON INSTRUCTIONS                           { $$ = new If($2, $4, [], @1.first_line, @1.first_column) }
        | IF EXPRESSION COLON INSTRUCTIONS ELSE COLON INSTRUCTIONS  { $$ = new If($2, $4, $7, @1.first_line, @1.first_column) }
  ;

WHILE_STMT: WHILE EXPRESSION COLON INSTRUCTIONS   { $$ = new While($2, $4, @1.first_line, @1.first_column) }
  ;

EXPRESSION: EXPRESSION PLUS EXPRESSION            { $$ = new Arithmetic($1, $3, $2, @1.first_line, @1.first_column) }
          | EXPRESSION MINUS EXPRESSION           { $$ = new Arithmetic($1, $3, $2, @1.first_line, @1.first_column) }
          | EXPRESSION TIMES EXPRESSION           { $$ = new Arithmetic($1, $3, $2, @1.first_line, @1.first_column) }
          | EXPRESSION DIVISION EXPRESSION        { $$ = new Arithmetic($1, $3, $2, @1.first_line, @1.first_column) }
          | EXPRESSION MOD EXPRESSION             { $$ = new Arithmetic($1, $3, $2, @1.first_line, @1.first_column) }
          | EXPRESSION EQ EXPRESSION              { $$ = new Relational($1, $3, $2, @1.first_line, @1.first_column) }
          | EXPRESSION NE EXPRESSION              { $$ = new Relational($1, $3, $2, @1.first_line, @1.first_column) }
          | EXPRESSION LT EXPRESSION              { $$ = new Relational($1, $3, $2, @1.first_line, @1.first_column) }
          | EXPRESSION GT EXPRESSION              { $$ = new Relational($1, $3, $2, @1.first_line, @1.first_column) }
          | EXPRESSION LE EXPRESSION              { $$ = new Relational($1, $3, $2, @1.first_line, @1.first_column) }
          | EXPRESSION GE EXPRESSION              { $$ = new Relational($1, $3, $2, @1.first_line, @1.first_column) }
          | LPAREN EXPRESSION RPAREN              { $$ = $2 }
          | ID                                    { $$ = new Variable($1, @1.first_line, @1.first_column) }
          | INTEGER                               { $$ = new Literal($1, @1.first_line, @1.first_column, Type.NUMBER) }
          | DOUBLE                                { $$ = new Literal($1, @1.first_line, @1.first_column, Type.NUMBER) }
          | STRING                                { $$ = new Literal($1, @1.first_line, @1.first_column, Type.STRING) }
          | TRUE                                  { $$ = new Literal($1, @1.first_line, @1.first_column, Type.BOOLEAN) }
          | FALSE                                 { $$ = new Literal($1, @1.first_line, @1.first_column, Type.BOOLEAN) }
  ;