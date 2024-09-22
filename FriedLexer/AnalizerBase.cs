using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FriedLexer
{
    /// <summary>
    /// use this type when you want to induvidually loop/iterate over an collection of sorts
    /// with extra tools such as peeking ahead without advancing/affecting the position
    /// </summary>
    /// <typeparam name="Type"></typeparam>
    public class AnalizerBase<Type>
    {
        //the list that we loop over
        public List<Type> Analizable = new List<Type>();
        public int Position = 0;

        //this class can be used for any type so we need to know the end
        public Type End { get; set; }
        public Type Current => Peek(0);
        public bool Safe => (!Current?.Equals(End)) ?? false;

        //look forward
        public Type Peek(int off = 0)
        {
            if (Position + off >= Analizable.Count || Position + off < 0) return End;
            return Analizable[Position + off];
        }
        public AnalizerBase(Type end)
        {
            this.Analizable = new List<Type>();
            this.End = end;
        }
        public AnalizerBase(List<Type> txt, Type end)
        {
            this.Analizable = txt;
            this.End = end;
        }
    }
}
