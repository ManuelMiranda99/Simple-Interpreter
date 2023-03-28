using System;
using System.Collections.Generic;
using System.Text;
// Librerias agregadas de IRONY
using Irony.Ast;
using Irony.Parsing;

namespace Ejemplos.analizador.Gramatica1
{
    // Gramatica no ambigua, sin precedencia de operadores
    class Gramatica1 : Grammar
    {
        public Gramatica1()
        {

            #region ER
            var ENTERO = new NumberLiteral("Entero");
            #endregion

            #region Terminales
            var MAS = ToTerm("+");
            var MENOS = ToTerm("-");
            var POR = ToTerm("*");
            var DIV = ToTerm("/");
            #endregion

            #region NoTerminales
            NonTerminal init = new NonTerminal("init");
            NonTerminal suma = new NonTerminal("suma");
            NonTerminal mult = new NonTerminal("mult");
            NonTerminal valo = new NonTerminal("valo");
            #endregion

            #region Gramatica
            init.Rule = suma
                            ;
            suma.Rule = suma + MAS + suma
                |       suma + MENOS + suma
                |       mult
                            ;
            mult.Rule = mult + POR + mult
                |       mult + DIV + mult
                |       valo
                            ;
            valo.Rule = ENTERO
                            ;
            #endregion

            #region Preferencias
            this.Root = init;
            #endregion
        }
    }
}