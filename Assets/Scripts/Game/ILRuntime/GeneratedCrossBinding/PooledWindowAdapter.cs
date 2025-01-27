using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace HotFix
{   
    public class PooledWindowAdapter : CrossBindingAdaptor
    {
        static CrossBindingMethodInfo<global::LayoutScript> msetScript_0 = new CrossBindingMethodInfo<global::LayoutScript>("setScript");
        static CrossBindingMethodInfo<global::myUIObject, global::myUIObject, System.String> massignWindow_1 = new CrossBindingMethodInfo<global::myUIObject, global::myUIObject, System.String>("assignWindow");
        static CrossBindingMethodInfo minit_2 = new CrossBindingMethodInfo("init");
        static CrossBindingMethodInfo mdestroy_3 = new CrossBindingMethodInfo("destroy");
        static CrossBindingMethodInfo mreset_4 = new CrossBindingMethodInfo("reset");
        static CrossBindingMethodInfo mrecycle_5 = new CrossBindingMethodInfo("recycle");
        static CrossBindingMethodInfo<System.Boolean> msetVisible_6 = new CrossBindingMethodInfo<System.Boolean>("setVisible");
        static CrossBindingMethodInfo<global::myUIObject, System.Boolean> msetParent_7 = new CrossBindingMethodInfo<global::myUIObject, System.Boolean>("setParent");
        static CrossBindingMethodInfo<System.Boolean> msetAsLastSibling_8 = new CrossBindingMethodInfo<System.Boolean>("setAsLastSibling");
        static CrossBindingMethodInfo mresetProperty_9 = new CrossBindingMethodInfo("resetProperty");
        static CrossBindingMethodInfo<System.Boolean> msetDestroy_10 = new CrossBindingMethodInfo<System.Boolean>("setDestroy");
        static CrossBindingFunctionInfo<System.Boolean> misDestroy_11 = new CrossBindingFunctionInfo<System.Boolean>("isDestroy");
        static CrossBindingMethodInfo<System.Int64> msetAssignID_12 = new CrossBindingMethodInfo<System.Int64>("setAssignID");
        static CrossBindingFunctionInfo<System.Int64> mgetAssignID_13 = new CrossBindingFunctionInfo<System.Int64>("getAssignID");
        public override Type BaseCLRType
        {
            get
            {
                return typeof(global::PooledWindow);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adapter);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adapter(appdomain, instance);
        }

        public class Adapter : global::PooledWindow, CrossBindingAdaptorType
        {
            ILTypeInstance instance;
            ILRuntime.Runtime.Enviorment.AppDomain appdomain;

            public Adapter()
            {

            }

            public Adapter(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                this.appdomain = appdomain;
                this.instance = instance;
            }

            public ILTypeInstance ILInstance { get { return instance; } }

            public override void setScript(global::LayoutScript script)
            {
                if (msetScript_0.CheckShouldInvokeBase(this.instance))
                    base.setScript(script);
                else
                    msetScript_0.Invoke(this.instance, script);
            }

            public override void assignWindow(global::myUIObject parent, global::myUIObject template, System.String name)
            {
                massignWindow_1.Invoke(this.instance, parent, template, name);
            }

            public override void init()
            {
                if (minit_2.CheckShouldInvokeBase(this.instance))
                    base.init();
                else
                    minit_2.Invoke(this.instance);
            }

            public override void destroy()
            {
                if (mdestroy_3.CheckShouldInvokeBase(this.instance))
                    base.destroy();
                else
                    mdestroy_3.Invoke(this.instance);
            }

            public override void reset()
            {
                if (mreset_4.CheckShouldInvokeBase(this.instance))
                    base.reset();
                else
                    mreset_4.Invoke(this.instance);
            }

            public override void recycle()
            {
                if (mrecycle_5.CheckShouldInvokeBase(this.instance))
                    base.recycle();
                else
                    mrecycle_5.Invoke(this.instance);
            }

            public override void setVisible(System.Boolean visible)
            {
                if (msetVisible_6.CheckShouldInvokeBase(this.instance))
                    base.setVisible(visible);
                else
                    msetVisible_6.Invoke(this.instance, visible);
            }

            public override void setParent(global::myUIObject parent, System.Boolean refreshDepth)
            {
                if (msetParent_7.CheckShouldInvokeBase(this.instance))
                    base.setParent(parent, refreshDepth);
                else
                    msetParent_7.Invoke(this.instance, parent, refreshDepth);
            }

            public override void setAsLastSibling(System.Boolean refreshDepth)
            {
                if (msetAsLastSibling_8.CheckShouldInvokeBase(this.instance))
                    base.setAsLastSibling(refreshDepth);
                else
                    msetAsLastSibling_8.Invoke(this.instance, refreshDepth);
            }

            public override void resetProperty()
            {
                if (mresetProperty_9.CheckShouldInvokeBase(this.instance))
                    base.resetProperty();
                else
                    mresetProperty_9.Invoke(this.instance);
            }

            public override void setDestroy(System.Boolean isDestroy)
            {
                if (msetDestroy_10.CheckShouldInvokeBase(this.instance))
                    base.setDestroy(isDestroy);
                else
                    msetDestroy_10.Invoke(this.instance, isDestroy);
            }

            public override System.Boolean isDestroy()
            {
                if (misDestroy_11.CheckShouldInvokeBase(this.instance))
                    return base.isDestroy();
                else
                    return misDestroy_11.Invoke(this.instance);
            }

            public override void setAssignID(System.Int64 assignID)
            {
                if (msetAssignID_12.CheckShouldInvokeBase(this.instance))
                    base.setAssignID(assignID);
                else
                    msetAssignID_12.Invoke(this.instance, assignID);
            }

            public override System.Int64 getAssignID()
            {
                if (mgetAssignID_13.CheckShouldInvokeBase(this.instance))
                    return base.getAssignID();
                else
                    return mgetAssignID_13.Invoke(this.instance);
            }

            public override string ToString()
            {
                IMethod m = appdomain.ObjectType.GetMethod("ToString", 0);
                m = instance.Type.GetVirtualMethod(m);
                if (m == null || m is ILMethod)
                {
                    return instance.ToString();
                }
                else
                    return instance.Type.FullName;
            }
        }
    }
}

