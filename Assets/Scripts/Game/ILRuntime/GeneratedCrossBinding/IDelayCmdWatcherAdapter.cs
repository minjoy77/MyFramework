using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace HotFix
{   
    public class IDelayCmdWatcherAdapter : CrossBindingAdaptor
    {
        static CrossBindingMethodInfo<global::Command> maddDelayCmd_0 = new CrossBindingMethodInfo<global::Command>("addDelayCmd");
        static CrossBindingMethodInfo<global::Command> monCmdStarted_1 = new CrossBindingMethodInfo<global::Command>("onCmdStarted");
        static CrossBindingMethodInfo minterruptAllCommand_2 = new CrossBindingMethodInfo("interruptAllCommand");
        static CrossBindingMethodInfo<System.Int64, System.Boolean> minterruptCommand_3 = new CrossBindingMethodInfo<System.Int64, System.Boolean>("interruptCommand");
        static CrossBindingMethodInfo mnotifyConstructDone_4 = new CrossBindingMethodInfo("notifyConstructDone");
        static CrossBindingMethodInfo mresetProperty_5 = new CrossBindingMethodInfo("resetProperty");
        static CrossBindingMethodInfo<System.Boolean> msetDestroy_6 = new CrossBindingMethodInfo<System.Boolean>("setDestroy");
        static CrossBindingFunctionInfo<System.Boolean> misDestroy_7 = new CrossBindingFunctionInfo<System.Boolean>("isDestroy");
        static CrossBindingMethodInfo<System.Int64> msetAssignID_8 = new CrossBindingMethodInfo<System.Int64>("setAssignID");
        static CrossBindingFunctionInfo<System.Int64> mgetAssignID_9 = new CrossBindingFunctionInfo<System.Int64>("getAssignID");
        public override Type BaseCLRType
        {
            get
            {
                return typeof(global::IDelayCmdWatcher);
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

        public class Adapter : global::IDelayCmdWatcher, CrossBindingAdaptorType
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

            public override void addDelayCmd(global::Command cmd)
            {
                maddDelayCmd_0.Invoke(this.instance, cmd);
            }

            public override void onCmdStarted(global::Command cmd)
            {
                monCmdStarted_1.Invoke(this.instance, cmd);
            }

            public override void interruptAllCommand()
            {
                minterruptAllCommand_2.Invoke(this.instance);
            }

            public override void interruptCommand(System.Int64 assignID, System.Boolean showError)
            {
                minterruptCommand_3.Invoke(this.instance, assignID, showError);
            }

            public override void notifyConstructDone()
            {
                if (mnotifyConstructDone_4.CheckShouldInvokeBase(this.instance))
                    base.notifyConstructDone();
                else
                    mnotifyConstructDone_4.Invoke(this.instance);
            }

            public override void resetProperty()
            {
                if (mresetProperty_5.CheckShouldInvokeBase(this.instance))
                    base.resetProperty();
                else
                    mresetProperty_5.Invoke(this.instance);
            }

            public override void setDestroy(System.Boolean isDestroy)
            {
                if (msetDestroy_6.CheckShouldInvokeBase(this.instance))
                    base.setDestroy(isDestroy);
                else
                    msetDestroy_6.Invoke(this.instance, isDestroy);
            }

            public override System.Boolean isDestroy()
            {
                if (misDestroy_7.CheckShouldInvokeBase(this.instance))
                    return base.isDestroy();
                else
                    return misDestroy_7.Invoke(this.instance);
            }

            public override void setAssignID(System.Int64 assignID)
            {
                if (msetAssignID_8.CheckShouldInvokeBase(this.instance))
                    base.setAssignID(assignID);
                else
                    msetAssignID_8.Invoke(this.instance, assignID);
            }

            public override System.Int64 getAssignID()
            {
                if (mgetAssignID_9.CheckShouldInvokeBase(this.instance))
                    return base.getAssignID();
                else
                    return mgetAssignID_9.Invoke(this.instance);
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

