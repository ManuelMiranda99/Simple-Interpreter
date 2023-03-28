using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ejemplos.arbol;
// Librerias agregadas de IRONY
using Irony.Ast;
using Irony.Parsing;

namespace Ejemplos.analizador.Gramatica2
{
    class Sintactico2 : Sintactico
    {
        private int temps;
        private int count;
        public Sintactico2()
        {
            this.grammar = "%lex\n%%\n\n\\s+                   /*skip whitespace*/\n[0-9]+('.'[0-9]+)?\\b  return 'NUMBER'\n'*'                   return '*'\n'/'                   return '/'\n'-'                   return '-'\n'+'                   return '+'\n<<EOF>>               return 'EOF'\n.                     return 'INVALID'\n\n/lex\n\n\n%left '+' '-'%left '*' '/'\n\n%start expressions\n\n%%\n\nexpressions\n    : e EOF\n    ;\n\ne\n    : e '+' e\n    | e '-' e\n    | e '*' e\n    | e '/' e\n    | NUMBER\n    ;\n\n";
        }

        private String getTemp()
        {
            this.temps++;
            return "t" + temps;
        }

        public override Nodo analizar(String cadena)
        {
            this.temps = -1;
            this.count = 0;

            Gramatica2 gramatica2 = new Gramatica2();
            LanguageData languageData = new LanguageData(gramatica2);
            Parser parser = new Parser(languageData);

            ParseTree tree = parser.Parse(cadena);
            ParseTreeNode root = tree.Root;

            Nodo retorno = init(root);

            return retorno;
        }

        public Nodo init(ParseTreeNode actual)
        {
            Nodo ini = new Nodo("init_" + count);
            count++;

            Nodo expr = exp(actual.ChildNodes.ElementAt(0));

            ini.setDireccion(expr.getDireccion());
            ini.setValor(expr.getValor());

            ini.hijos.AddLast(expr);

            ini.C3D = expr.C3D;

            return ini;
        }

        public Nodo exp(ParseTreeNode actual)
        {
            Nodo expr = new Nodo("expr_" + count);
            count++;
            if(actual.ChildNodes.Count == 3)
            {
                Nodo primer = exp(actual.ChildNodes.ElementAt(0));
                Nodo segundo = exp(actual.ChildNodes.ElementAt(2));

                expr.setDireccion(this.getTemp());
                expr.hijos.AddLast(primer);

                String operador = actual.ChildNodes.ElementAt(1).ToString().Split(' ')[0];
                
                switch (operador)
                {
                    case "+":
                        expr.C3D = primer.C3D + segundo.C3D + expr.getDireccion() + "=" + primer.getDireccion() + "+" + segundo.getDireccion() + "\n";
                        expr.setValor(primer.getValor() + segundo.getValor());

                        expr.hijos.AddLast(new Nodo("SUMA_" + count));
                        count++;
                        break;
                    case "-":
                        expr.C3D = primer.C3D + segundo.C3D + expr.getDireccion() + "=" + primer.getDireccion() + "-" + segundo.getDireccion() + "\n";
                        expr.setValor(primer.getValor() - segundo.getValor());

                        expr.hijos.AddLast(new Nodo("MENOS_" + count));
                        count++;
                        break;
                    case "*":
                        expr.C3D = primer.C3D + segundo.C3D + expr.getDireccion() + "=" + primer.getDireccion() + "*" + segundo.getDireccion() + "\n";
                        expr.setValor(primer.getValor() * segundo.getValor());

                        expr.hijos.AddLast(new Nodo("POR_" + count));
                        count++;
                        break;
                    case "/":
                        expr.C3D = primer.C3D + segundo.C3D + expr.getDireccion() + "=" + primer.getDireccion() + "/" + segundo.getDireccion() + "\n";
                        expr.setValor(primer.getValor() / segundo.getValor());

                        expr.hijos.AddLast(new Nodo("DIV_" + count));
                        count++;
                        break;
                }
                expr.hijos.AddLast(segundo);
            }
            else
            {
                Nodo val = new Nodo("NUMERO_" + actual.ChildNodes.ElementAt(0).ToString().Split(' ')[0] + "_" + count);
                val.setValor(Double.Parse(actual.ChildNodes.ElementAt(0).ToString().Split(' ')[0]));
                val.setDireccion(actual.ChildNodes.ElementAt(0).ToString().Split(' ')[0]);
                count++;

                expr.setValor(val.getValor());
                expr.setDireccion(val.getDireccion());

                expr.hijos.AddLast(val);
            }
            return expr;
        }
    }
}