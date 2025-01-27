using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class CmdGameSceneManagerEnter_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::CmdGameSceneManagerEnter);

            field = type.GetField("mSceneType", flag);
            app.RegisterCLRFieldGetter(field, get_mSceneType_0);
            app.RegisterCLRFieldSetter(field, set_mSceneType_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_mSceneType_0, AssignFromStack_mSceneType_0);


        }



        static object get_mSceneType_0(ref object o)
        {
            return ((global::CmdGameSceneManagerEnter)o).mSceneType;
        }

        static StackObject* CopyToStack_mSceneType_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::CmdGameSceneManagerEnter)o).mSceneType;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_mSceneType_0(ref object o, object v)
        {
            ((global::CmdGameSceneManagerEnter)o).mSceneType = (System.Type)v;
        }

        static StackObject* AssignFromStack_mSceneType_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Type @mSceneType = (System.Type)typeof(System.Type).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::CmdGameSceneManagerEnter)o).mSceneType = @mSceneType;
            return ptr_of_this_method;
        }



    }
}
