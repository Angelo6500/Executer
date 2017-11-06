using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MyAttribute;

namespace Executer
{
   
    class Program
    {
        static void Main(string[] args)
        {
            try
            {

                var path = Directory.GetCurrentDirectory();
                path = Path.GetFullPath(Path.Combine(path, @"..\..\..\"));

                path += @"MyLibrary\bin\Debug\MyLibrary.dll";

                var a = Assembly.LoadFrom(path);

                foreach (var type in a.GetTypes())
                    if (type.IsClass)
                        Console.WriteLine(type.FullName);

                Console.ReadLine();

                //Reflection

                var types = a.GetTypes();
                foreach (var type in types)
                {
                    if (!type.IsClass)
                        continue;
                    Console.WriteLine(type.Name);
                    object o = null;
                    try
                    {
                        o = Activator.CreateInstance(type);
                    }
                    catch (MissingMethodException)
                    {
                       CreateInstanceOfATypeWithoutDefaultConstructor(type,ref o);
                    }

                    var methods = type.GetMethods();

                    foreach (var methodsSignature in methods)
                    {
                        var annotations = methodsSignature.GetCustomAttributes<ExecuteMeAttribute>();
                        var methodParameters = methodsSignature.GetParameters();
                        var mandatory = 0;
                        var optional = 0;
                        var withParams = 0;
                        var typeParams = new List<Type>();
                        //var listOptional = new List<object>();
                        var d = new Dictionary<object,object>();

                        CountingOfParameters(ref methodParameters, ref mandatory,ref optional, ref withParams,ref typeParams);

                        ExtractOptionalParameters( ref methodParameters,ref d/*,ref listOptional*/);

                        foreach (var annotation in annotations)
                        {
                           
                            var annotationParameters = annotation.Parameters;
                            var mandatoryList = new List<object>();
                            var listWithParams = new List<object>();
                            var parametersList = new List<object>();
                            if (annotationParameters != null)
                            {

                                var k = MandatoryParameters(ref annotationParameters, ref mandatory, ref mandatoryList);
                                OptionalParameters(ref annotationParameters,ref k, ref mandatory, ref optional, ref d);
                                if(withParams>0)
                                    WithParamsParameters(ref annotationParameters, ref k, ref listWithParams);

                                parametersList = mandatoryList;

                                foreach (var e in d)
                                {
                                    parametersList.Add(e.Value);
                                }

                                if (withParams > 0)
                                {
                                    var rest = listWithParams.ToArray();
                                    parametersList.Add(rest);

                                }
                                while (k < annotationParameters.Length)
                                {
                                    parametersList.Add(annotationParameters[k]);
                                    k++;
                                }
                                try
                                {
                                    //var m = methodsSignature.MakeGenericMethod(typeParams.ToArray());
                                    //m.Invoke(o, parametersList.ToArray());
                                    methodsSignature.Invoke(o, parametersList.ToArray());
                                }
                                catch (TargetInvocationException e)
                                {
                                    Console.WriteLine(methodsSignature.Name + " " + e.Message);
                                    Console.ReadLine();
                                }
                                catch (ArgumentException e)
                                {
                                    Console.WriteLine(methodsSignature.Name + " " + e.Message);
                                    Console.ReadLine();

                                }
                                catch (TargetParameterCountException e)
                                {
                                    Console.WriteLine(methodsSignature.Name + " " + e.Message);
                                    Console.ReadLine();
                                }
                            }
                            else
                            {
                                if(withParams>0)
                                    parametersList.Add(new object[] { });
                                try
                                {
                                    methodsSignature.Invoke(o, parametersList.ToArray());
                                }
                                catch (TargetInvocationException e)
                                {
                                    Console.WriteLine(methodsSignature.Name + " " + e.Message);
                                    Console.ReadLine();
                                }
                                catch (ArgumentException e)
                                {
                                    Console.WriteLine(methodsSignature.Name + " " + e.Message);
                                    Console.ReadLine();

                                }
                                catch (TargetParameterCountException e)
                                {
                                    Console.WriteLine(methodsSignature.Name + " " + e.Message);
                                    Console.ReadLine();
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {

                Debug.WriteLine(e);
                throw;
            }

        }

        private static void CreateInstanceOfATypeWithoutDefaultConstructor(Type type, ref object o)
        {
            var constructors = type.GetConstructors();

            foreach (var constructor in constructors)
            {
                var annotationConstructors = constructor.GetCustomAttributes<ExecuteMe2Attribute>();

                foreach (var annotation in annotationConstructors)
                {
                    o = Activator.CreateInstance(type,
                        BindingFlags.CreateInstance &
                        BindingFlags.Public &
                        BindingFlags.Instance &
                        BindingFlags.OptionalParamBinding,
                        null, annotation.Parameters, null);
                }
            }
            
        }

        private static void ExtractOptionalParameters(ref ParameterInfo[] methodParameters,ref Dictionary<object,object> optionalParametersDictionary)
        {
            foreach (var parameter in methodParameters)
            {
                if (parameter.IsOptional)
                {
                    optionalParametersDictionary.Add(parameter.Name, parameter.DefaultValue);
                }
            }
        }



        private static void CountingOfParameters( 
            ref ParameterInfo[] methodParameters,
            ref int mandatory,
            ref int optional, 
            ref int withParams,
            ref List<Type> parameterTypeParams
            )
        {
            var d = new Dictionary<object,object>();
            foreach (var parameter in methodParameters)
            {
                if (parameter.IsOptional)
                {
                    optional++;
                    var tmp = parameter.DefaultValue;
                    parameterTypeParams.Add(parameter.ParameterType);
                }
                else 
                 if (!Attribute.IsDefined(parameter, typeof(ParamArrayAttribute)))
                {
                    parameterTypeParams.Add(parameter.ParameterType);
                    mandatory++;
                }
                else
                 {
                     parameterTypeParams.Add(parameter.ParameterType);
                    withParams++;
                }
            }
            
        }



        private static int MandatoryParameters( ref object[] annotationParameters, ref int mandatory, ref List<object> mandatoryList)
        {
            var k = 0;
            for (var i = 0; i < mandatory && i<annotationParameters.Length; i++)
            {
                var x = annotationParameters[i];
                mandatoryList.Add(x);
                k = i;

            }
            return k;
        }

        private static void OptionalParameters(ref object[] annotationParameters, ref int k, ref int mandatory, ref int optional,
            ref Dictionary<object, object> d)
        {
            int i = mandatory;
            k = i;
            while (i < mandatory + 2 * optional && i + 1 < annotationParameters.Length)
            {
                var key = annotationParameters[i];
                if (d.ContainsKey(key) && i + 1 < annotationParameters.Length)
                {
                    d[key] = annotationParameters[i + 1];
                    i = i + 2;
                    k = i;
                }
                else
                {
                    break;
                }

            }
            //return k;
        }

        private static void WithParamsParameters(ref object[] annotationParameters, ref int k,
            ref List<object> listWithParams)
        {
            
            for (var i = k; i < annotationParameters.Length; i++)
            {
                var x = annotationParameters[i];
                listWithParams.Add(x);
                k++;
            }
        }

        
    }
}
