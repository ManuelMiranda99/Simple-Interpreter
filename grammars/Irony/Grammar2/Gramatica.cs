using System;
using System.Collections.Generic;
using System.Text;
// Librerias agregadas de IRONY
using Irony.Ast;
using Irony.Parsing;

namespace Ejemplos.analizador.Gramatica2
{
    // Gramatica con ambiguedad utilizando precedencia de operadores
    class Gramatica2 : Grammar
    {
        public Gramatica2() : base(caseSensitive: false)
        {

            #region ER
            var ENTERO = new NumberLiteral("entero");
            #endregion

            #region Terminales
            var MAS = ToTerm("+");
            var MENOS = ToTerm("-");
            var POR = ToTerm("*");
            var DIV = ToTerm("/");

            RegisterOperators(1, MAS, MENOS);
            RegisterOperators(2, POR, DIV);

            #endregion

            #region NoTerminales
            NonTerminal init = new NonTerminal("init");
            NonTerminal exp = new NonTerminal("exp");
            #endregion

            #region Gramatica
            init.Rule = exp
                            ;
            exp.Rule = exp + MAS + exp
                |      exp + MENOS + exp
                |      exp + POR + exp
                |      exp + DIV + exp
                |      ENTERO
                            ;
            #endregion

            #region Preferencias
            this.Root = init;
            #endregion
        }
    }
}

