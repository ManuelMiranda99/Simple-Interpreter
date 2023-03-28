using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ejemplos.arbol;
// Librerias agregadas de IRONY
using Irony.Ast;
using Irony.Parsing;

namespace Ejemplos.analizador.Gramatica1
{
    class Sintactico1 : Sintactico
    {
        private int temps;
        private int count;
        public Sintactico1()
        {
            this.grammar = "%lex\n%%\n\n\\s+                   /*skip whitespace*/\n[0-9]+('.'[0-9]+)?\\b  return 'NUMBER'\n'*'                   return '*'\n'/'                   return '/'\n'-'                   return '-'\n'+'                   return '+'\n<<EOF>>               return 'EOF'\n.                     return 'INVALID'\n\n/lex\n\n%start init\n\n%%\n\ninit : suma EOF\n;\n\nsuma: suma '+' suma\n| suma '-' suma\n| mult\n;\n\nmult: mult '*' mult\n| mult '/' mult\n| valo\n;\n\nvalo: NUMBER;\n";
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

            Gramatica1 gramatica1 = new Gramatica1();
            LanguageData languageData = new LanguageData(gramatica1);
            Parser parser = new Parser(languageData);

            ParseTree tree = parser.Parse(cadena);
            ParseTreeNode root = tree.Root;

            // root -> init

            Nodo retorno = init(root);

            return retorno;
        }

        public Nodo init(ParseTreeNode actual)
        {
            Nodo ini = new Nodo("init_" + count);
            count++;

            Nodo sum = suma(actual.ChildNodes.ElementAt(0));

            ini.setDireccion(sum.getDireccion());
            ini.setValor(sum.getValor());

            ini.hijos.AddLast(sum);

            ini.C3D = sum.C3D;

            return ini;
        }

        public Nodo suma(ParseTreeNode actual)
        {
            Nodo sum = new Nodo("suma_" + count);
            count++;
            if(actual.ChildNodes.Count == 3)
            {
                String operador = actual.ChildNodes.ElementAt(1).ToString().Split(' ')[0];
                switch (operador)
                {
                    case "+":
                        Nodo sum1 = suma(actual.ChildNodes.ElementAt(0));

                        Nodo sum2 = suma(actual.ChildNodes.ElementAt(2));

                        sum.setDireccion(this.getTemp());
                        sum.setValor(sum1.getValor() + sum2.getValor());

                        sum.hijos.AddLast(sum1);

                        sum.hijos.AddLast(new Nodo("SUMA_" + count));
                        count++;

                        sum.hijos.AddLast(sum2);

                        sum.C3D = sum1.C3D + sum2.C3D + sum.getDireccion() + "=" + sum1.getDireccion() + "+" + sum2.getDireccion() + "\n";

                        return sum;
                    case "-":
                        Nodo sum3 = suma(actual.ChildNodes.ElementAt(0));

                        Nodo sum4 = suma(actual.ChildNodes.ElementAt(2));

                        sum.setDireccion(this.getTemp());
                        sum.setValor(sum3.getValor() - sum4.getValor());

                        sum.hijos.AddLast(sum3);

                        sum.hijos.AddLast(new Nodo("RESTA_" + count));
                        count++;

                        sum.hijos.AddLast(sum4);

                        sum.C3D = sum3.C3D + sum4.C3D + sum.getDireccion() + "=" + sum3.getDireccion() + "-" + sum4.getDireccion() + "\n";

                        return sum;
                    default:
                        Console.WriteLine("Toca ver esto");
                        return null;
                }
            }
            else
            {
                Nodo multi = mult(actual.ChildNodes.ElementAt(0));

                sum.setValor(multi.getValor());
                sum.setDireccion(multi.getDireccion());

                sum.hijos.AddLast(multi);

                sum.C3D = multi.C3D;

                return sum;
            }
        }

        public Nodo mult(ParseTreeNode actual)
        {
            Nodo multi = new Nodo("mult_" + count);
            count++;
            if (actual.ChildNodes.Count == 3)
            {
                String operador = actual.ChildNodes.ElementAt(1).ToString().Split(' ')[0];

                switch (operador)
                {
                    case "*":
                        Nodo mult1 = mult(actual.ChildNodes.ElementAt(0));

                        Nodo mult2 = mult(actual.ChildNodes.ElementAt(2));

                        multi.setDireccion(this.getTemp());
                        multi.setValor(mult1.getValor() * mult2.getValor());

                        multi.hijos.AddLast(mult1);

                        multi.hijos.AddLast(new Nodo("MULT_" + count));
                        count++;

                        multi.hijos.AddLast(mult2);

                        multi.C3D = mult1.C3D + mult2.C3D + multi.getDireccion() + "=" + mult1.getDireccion() + "*" + mult2.getDireccion() + "\n";

                        return multi;
                    case "/":
                        Nodo mult3 = mult(actual.ChildNodes.ElementAt(0));

                        Nodo mult4 = mult(actual.ChildNodes.ElementAt(2));

                        multi.setDireccion(this.getTemp());
                        multi.setValor(mult3.getValor() / mult4.getValor());

                        multi.hijos.AddLast(mult3);

                        multi.hijos.AddLast(new Nodo("DIV_" + count));
                        count++;

                        multi.hijos.AddLast(mult4);

                        multi.C3D = mult3.C3D + mult4.C3D + multi.getDireccion() + "=" + mult3.getDireccion() + "/" + mult4.getDireccion() + "\n";

                        return multi;
                    default:
                        Console.WriteLine("Toca ver esto");
                        return null;
                }
            }
            else
            {
                Nodo val = valo(actual.ChildNodes.ElementAt(0));

                multi.setValor(val.getValor());
                multi.setDireccion(val.getDireccion());

                multi.hijos.AddLast(val);
                multi.C3D = val.C3D;

                return multi;
            }
        }

        public Nodo valo(ParseTreeNode actual)
        {
            Nodo valo = new Nodo("valo_" + count);
            count++;
            
            Nodo numero = new Nodo("Numero_" + actual.ChildNodes.ElementAt(0).ToString().Split(' ')[0] + "_" + count);
            numero.setValor(Double.Parse(actual.ChildNodes.ElementAt(0).ToString().Split(' ')[0]));
            numero.setDireccion(actual.ChildNodes.ElementAt(0).ToString().Split(' ')[0]);
            count++;

            valo.setValor(numero.getValor());
            valo.setDireccion(numero.getDireccion());
            
            valo.hijos.AddLast(numero);
            return valo;
        }
    }
}