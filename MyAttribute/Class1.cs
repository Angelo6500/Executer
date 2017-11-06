using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MyAttribute
{
    //attributo introdotto per le classi senza costruttore di default
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = true)]
    public class ExecuteMe2Attribute : Attribute
    {
        public object[] Parameters { get; }

        public ExecuteMe2Attribute()
        {
            Parameters = null;
        }


        public ExecuteMe2Attribute(params object[] parameters)
        {
            this.Parameters = new object[parameters.Length];
            for (int i = 0; i < parameters.Length; ++i)
            {
                this.Parameters[i] = parameters[i];
            }

        }



    }

    

    [AttributeUsage(AttributeTargets.Method,AllowMultiple = true )]
    public class ExecuteMeAttribute : Attribute
    {
        public object[] Parameters { get; }

        public ExecuteMeAttribute()
        {
            Parameters = null;
        }

        public ExecuteMeAttribute(params string[] parameters)
        {
            Parameters = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; ++i)
            {
                Parameters[i] = parameters[i];
            }
        }

        public ExecuteMeAttribute(params object[] parameters)
        {
            Parameters = new object[parameters.Length];
            for (var i = 0; i < parameters.Length; ++i)
            {
                Parameters[i] = parameters[i];
            }

        }






    }
}
