using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvenShittyerCalc
{
    public static class  BasicCalculations 
    {
        public static string Calculate(double a,double b,string operation)
        {
            switch (operation)
            {
                case "+": return (a + b).ToString();
                case "-": return (a - b).ToString();
                case "/": return (b!=0 ? (a/b).ToString() : "Cannot divide");
                case "*": return (a * b).ToString();
                default: return "UnknownBasicOperation";
            }
        }
    }
}
