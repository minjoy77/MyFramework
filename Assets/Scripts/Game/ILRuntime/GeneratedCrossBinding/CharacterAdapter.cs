using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace HotFix
{   
    public class CharacterAdapter : CrossBindingAdaptor
    {
        static CrossBindingFunctionInfo<global::CharacterBaseData> mcreateCharacterData_0 = new CrossBindingFunctionInfo<global::CharacterBaseData>("createCharacterData");
        static CrossBindingMethodInfo minit_1 = new CrossBindingMethodInfo("init");
        static CrossBindingMethodInfo mresetProperty_2 = new CrossBindingMethodInfo("resetProperty");
        static CrossBindingMethodInfo mdestroyModel_3 = new CrossBindingMethodInfo("destroyModel");
        static CrossBindingFunctionInfo<System.String, System.Single> mgetAnimationLength_4 = new CrossBindingFunctionInfo<System.String, System.Single>("getAnimationLength");
        static CrossBindingMethodInfo<global::GameComponent> mnotifyComponentChanged_5 = new CrossBindingMethodInfo<global::GameComponent>("notifyComponentChanged");
        static CrossBindingMethodInfo minitComponents_6 = new CrossBindingMethodInfo("initComponents");
        static CrossBindingMethodInfo<UnityEngine.GameObject> mnotifyModelLoaded_7 = new CrossBindingMethodInfo<UnityEngine.GameObject>("notifyModelLoaded");
        static CrossBindingMethodInfo mdestroy_8 = new CrossBindingMethodInfo("destroy");
        static CrossBindingMethodInfo<UnityEngine.GameObject, System.Boolean> msetObject_9 = new CrossBindingMethodInfo<UnityEngine.GameObject, System.Boolean>("setObject");
        static CrossBindingMethodInfo<System.Single> mupdate_10 = new CrossBindingMethodInfo<System.Single>("update");
        static CrossBindingMethodInfo<System.Single> mfixedUpdate_11 = new CrossBindingMethodInfo<System.Single>("fixedUpdate");
        static CrossBindingFunctionInfo<UnityEngine.Vector3, UnityEngine.Vector3> mlocalToWorld_12 = new CrossBindingFunctionInfo<UnityEngine.Vector3, UnityEngine.Vector3>("localToWorld");
        static CrossBindingFunctionInfo<UnityEngine.Vector3, UnityEngine.Vector3> mworldToLocal_13 = new CrossBindingFunctionInfo<UnityEngine.Vector3, UnityEngine.Vector3>("worldToLocal");
        static CrossBindingFunctionInfo<UnityEngine.Vector3, UnityEngine.Vector3> mlocalToWorldDirection_14 = new CrossBindingFunctionInfo<UnityEngine.Vector3, UnityEngine.Vector3>("localToWorldDirection");
        static CrossBindingFunctionInfo<UnityEngine.Vector3, UnityEngine.Vector3> mworldToLocalDirection_15 = new CrossBindingFunctionInfo<UnityEngine.Vector3, UnityEngine.Vector3>("worldToLocalDirection");
        static CrossBindingFunctionInfo<UnityEngine.GameObject> mgetObject_16 = new CrossBindingFunctionInfo<UnityEngine.GameObject>("getObject");
        static CrossBindingFunctionInfo<UnityEngine.Vector3> mgetPosition_17 = new CrossBindingFunctionInfo<UnityEngine.Vector3>("getPosition");
        static CrossBindingFunctionInfo<UnityEngine.Vector3> mgetRotation_18 = new CrossBindingFunctionInfo<UnityEngine.Vector3>("getRotation");
        static CrossBindingFunctionInfo<UnityEngine.Vector3> mgetScale_19 = new CrossBindingFunctionInfo<UnityEngine.Vector3>("getScale");
        static CrossBindingFunctionInfo<UnityEngine.Vector3> mgetWorldPosition_20 = new CrossBindingFunctionInfo<UnityEngine.Vector3>("getWorldPosition");
        static CrossBindingFunctionInfo<UnityEngine.Vector3> mgetWorldScale_21 = new CrossBindingFunctionInfo<UnityEngine.Vector3>("getWorldScale");
        static CrossBindingFunctionInfo<UnityEngine.Vector3> mgetWorldRotation_22 = new CrossBindingFunctionInfo<UnityEngine.Vector3>("getWorldRotation");
        static CrossBindingFunctionInfo<System.Boolean> misActive_23 = new CrossBindingFunctionInfo<System.Boolean>("isActive");
        static CrossBindingFunctionInfo<System.Boolean> misActiveInHierarchy_24 = new CrossBindingFunctionInfo<System.Boolean>("isActiveInHierarchy");
        static CrossBindingFunctionInfo<System.Boolean> misHandleInput_25 = new CrossBindingFunctionInfo<System.Boolean>("isHandleInput");
        static CrossBindingFunctionInfo<UnityEngine.Collider> mgetCollider_26 = new CrossBindingFunctionInfo<UnityEngine.Collider>("getCollider");
        static CrossBindingFunctionInfo<global::UIDepth> mgetDepth_27 = new CrossBindingFunctionInfo<global::UIDepth>("getDepth");
        static CrossBindingFunctionInfo<System.Boolean> misReceiveScreenMouse_28 = new CrossBindingFunctionInfo<System.Boolean>("isReceiveScreenMouse");
        static CrossBindingFunctionInfo<System.Boolean> misPassRay_29 = new CrossBindingFunctionInfo<System.Boolean>("isPassRay");
        static CrossBindingFunctionInfo<System.Boolean> misDragable_30 = new CrossBindingFunctionInfo<System.Boolean>("isDragable");
        static CrossBindingFunctionInfo<System.Boolean> misMouseHovered_31 = new CrossBindingFunctionInfo<System.Boolean>("isMouseHovered");
        static CrossBindingFunctionInfo<global::IMouseEventCollect, System.Boolean> misChildOf_32 = new CrossBindingFunctionInfo<global::IMouseEventCollect, System.Boolean>("isChildOf");
        static CrossBindingMethodInfo<System.Boolean> msetPassRay_33 = new CrossBindingMethodInfo<System.Boolean>("setPassRay");
        static CrossBindingMethodInfo<System.Boolean> msetHandleInput_34 = new CrossBindingMethodInfo<System.Boolean>("setHandleInput");
        static CrossBindingMethodInfo<System.String> msetName_35 = new CrossBindingMethodInfo<System.String>("setName");
        static CrossBindingMethodInfo<System.Boolean> msetActive_36 = new CrossBindingMethodInfo<System.Boolean>("setActive");
        static CrossBindingMethodInfo<UnityEngine.Vector3> msetPosition_37 = new CrossBindingMethodInfo<UnityEngine.Vector3>("setPosition");
        static CrossBindingMethodInfo<UnityEngine.Vector3> msetScale_38 = new CrossBindingMethodInfo<UnityEngine.Vector3>("setScale");
        static CrossBindingMethodInfo<UnityEngine.Vector3> msetRotation_39 = new CrossBindingMethodInfo<UnityEngine.Vector3>("setRotation");
        static CrossBindingMethodInfo<UnityEngine.Vector3> msetWorldPosition_40 = new CrossBindingMethodInfo<UnityEngine.Vector3>("setWorldPosition");
        static CrossBindingMethodInfo<UnityEngine.Vector3> msetWorldRotation_41 = new CrossBindingMethodInfo<UnityEngine.Vector3>("setWorldRotation");
        static CrossBindingMethodInfo<UnityEngine.Vector3> msetWorldScale_42 = new CrossBindingMethodInfo<UnityEngine.Vector3>("setWorldScale");
        static CrossBindingMethodInfo<UnityEngine.Vector3, UnityEngine.Space> mmove_43 = new CrossBindingMethodInfo<UnityEngine.Vector3, UnityEngine.Space>("move");
        static CrossBindingMethodInfo<global::ObjectClickCallback> msetClickCallback_44 = new CrossBindingMethodInfo<global::ObjectClickCallback>("setClickCallback");
        static CrossBindingMethodInfo<global::ObjectHoverCallback> msetHoverCallback_45 = new CrossBindingMethodInfo<global::ObjectHoverCallback>("setHoverCallback");
        static CrossBindingMethodInfo<global::ObjectPressCallback> msetPressCallback_46 = new CrossBindingMethodInfo<global::ObjectPressCallback>("setPressCallback");
        static CrossBindingMethodInfo monMouseEnter_47 = new CrossBindingMethodInfo("onMouseEnter");
        static CrossBindingMethodInfo monMouseLeave_48 = new CrossBindingMethodInfo("onMouseLeave");
        static CrossBindingMethodInfo<UnityEngine.Vector3> monMouseDown_49 = new CrossBindingMethodInfo<UnityEngine.Vector3>("onMouseDown");
        static CrossBindingMethodInfo<UnityEngine.Vector3> monMouseUp_50 = new CrossBindingMethodInfo<UnityEngine.Vector3>("onMouseUp");
        class onMouseMove_51Info : CrossBindingMethodInfo
        {
            static Type[] pTypes = new Type[] {typeof(UnityEngine.Vector3).MakeByRefType(), typeof(UnityEngine.Vector3).MakeByRefType(), typeof(System.Single)};

            public onMouseMove_51Info()
                : base("onMouseMove")
            {

            }

            protected override Type ReturnType { get { return null; } }

            protected override Type[] Parameters { get { return pTypes; } }
            public void Invoke(ILTypeInstance instance, ref UnityEngine.Vector3 mousePos, ref UnityEngine.Vector3 moveDelta, System.Single moveTime)
            {
                EnsureMethod(instance);
                if (method != null)
                {
                    invoking = true;
                    try
                    {
                        using (var ctx = domain.BeginInvoke(method))
                        {
                            ctx.PushObject(mousePos);
                            ctx.PushObject(moveDelta);
                            ctx.PushObject(instance);
                            ctx.PushReference(0);
                            ctx.PushReference(1);
                            ctx.PushInteger(moveTime);
                            ctx.Invoke();
                            mousePos = ctx.ReadObject<UnityEngine.Vector3>(0);
                            moveDelta = ctx.ReadObject<UnityEngine.Vector3>(1);
                        }
                    }
                    finally
                    {
                        invoking = false;
                    }
                }
            }

            public override void Invoke(ILTypeInstance instance)
            {
                throw new NotSupportedException();
            }
        }
        static onMouseMove_51Info monMouseMove_51 = new onMouseMove_51Info();
        static CrossBindingMethodInfo<UnityEngine.Vector3> monMouseStay_52 = new CrossBindingMethodInfo<UnityEngine.Vector3>("onMouseStay");
        static CrossBindingMethodInfo<UnityEngine.Vector3> monScreenMouseDown_53 = new CrossBindingMethodInfo<UnityEngine.Vector3>("onScreenMouseDown");
        static CrossBindingMethodInfo<UnityEngine.Vector3> monScreenMouseUp_54 = new CrossBindingMethodInfo<UnityEngine.Vector3>("onScreenMouseUp");
        class onReceiveDrag_55Info : CrossBindingMethodInfo
        {
            static Type[] pTypes = new Type[] {typeof(global::IMouseEventCollect), typeof(System.Boolean).MakeByRefType()};

            public onReceiveDrag_55Info()
                : base("onReceiveDrag")
            {

            }

            protected override Type ReturnType { get { return null; } }

            protected override Type[] Parameters { get { return pTypes; } }
            public void Invoke(ILTypeInstance instance, global::IMouseEventCollect dragObj, ref System.Boolean continueEvent)
            {
                EnsureMethod(instance);
                if (method != null)
                {
                    invoking = true;
                    try
                    {
                        using (var ctx = domain.BeginInvoke(method))
                        {
                            ctx.PushObject(continueEvent);
                            ctx.PushObject(instance);
                            ctx.PushObject(dragObj);
                            ctx.PushReference(0);
                            ctx.Invoke();
                            continueEvent = ctx.ReadObject<System.Boolean>(0);
                        }
                    }
                    finally
                    {
                        invoking = false;
                    }
                }
            }

            public override void Invoke(ILTypeInstance instance)
            {
                throw new NotSupportedException();
            }
        }
        static onReceiveDrag_55Info monReceiveDrag_55 = new onReceiveDrag_55Info();
        static CrossBindingMethodInfo<global::IMouseEventCollect, System.Boolean> monDragHoverd_56 = new CrossBindingMethodInfo<global::IMouseEventCollect, System.Boolean>("onDragHoverd");
        static CrossBindingMethodInfo<UnityEngine.Vector2, UnityEngine.Vector2> monMultiTouchStart_57 = new CrossBindingMethodInfo<UnityEngine.Vector2, UnityEngine.Vector2>("onMultiTouchStart");
        static CrossBindingMethodInfo<UnityEngine.Vector2, UnityEngine.Vector2, UnityEngine.Vector2, UnityEngine.Vector2> monMultiTouchMove_58 = new CrossBindingMethodInfo<UnityEngine.Vector2, UnityEngine.Vector2, UnityEngine.Vector2, UnityEngine.Vector2>("onMultiTouchMove");
        static CrossBindingMethodInfo monMultiTouchEnd_59 = new CrossBindingMethodInfo("onMultiTouchEnd");
        static CrossBindingMethodInfo<System.Single> msetAlpha_60 = new CrossBindingMethodInfo<System.Single>("setAlpha");
        static CrossBindingFunctionInfo<System.Single> mgetAlpha_61 = new CrossBindingFunctionInfo<System.Single>("getAlpha");
        static CrossBindingFunctionInfo<System.Boolean> misEnable_62 = new CrossBindingFunctionInfo<System.Boolean>("isEnable");
        static CrossBindingMethodInfo<System.Boolean> msetEnable_63 = new CrossBindingMethodInfo<System.Boolean>("setEnable");
        static CrossBindingMethodInfo<System.Single> mlateUpdate_64 = new CrossBindingMethodInfo<System.Single>("lateUpdate");
        static CrossBindingMethodInfo<global::GameComponent> mnotifyAddComponent_65 = new CrossBindingMethodInfo<global::GameComponent>("notifyAddComponent");
        static CrossBindingMethodInfo<System.Boolean, System.Boolean> msetIgnoreTimeScale_66 = new CrossBindingMethodInfo<System.Boolean, System.Boolean>("setIgnoreTimeScale");
        static CrossBindingMethodInfo<global::Command> mreceiveCommand_67 = new CrossBindingMethodInfo<global::Command>("receiveCommand");
        static CrossBindingFunctionInfo<System.String> mgetName_68 = new CrossBindingFunctionInfo<System.String>("getName");
        static CrossBindingMethodInfo<System.Boolean> msetDestroy_69 = new CrossBindingMethodInfo<System.Boolean>("setDestroy");
        static CrossBindingFunctionInfo<System.Boolean> misDestroy_70 = new CrossBindingFunctionInfo<System.Boolean>("isDestroy");
        static CrossBindingMethodInfo<System.UInt64> msetAssignID_71 = new CrossBindingMethodInfo<System.UInt64>("setAssignID");
        static CrossBindingFunctionInfo<System.UInt64> mgetAssignID_72 = new CrossBindingFunctionInfo<System.UInt64>("getAssignID");
        static CrossBindingMethodInfo mnotifyConstructDone_73 = new CrossBindingMethodInfo("notifyConstructDone");
        public override Type BaseCLRType
        {
            get
            {
                return typeof(global::Character);
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

        public class Adapter : global::Character, CrossBindingAdaptorType
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

            protected override global::CharacterBaseData createCharacterData()
            {
                if (mcreateCharacterData_0.CheckShouldInvokeBase(this.instance))
                    return base.createCharacterData();
                else
                    return mcreateCharacterData_0.Invoke(this.instance);
            }

            public override void init()
            {
                if (minit_1.CheckShouldInvokeBase(this.instance))
                    base.init();
                else
                    minit_1.Invoke(this.instance);
            }

            public override void resetProperty()
            {
                if (mresetProperty_2.CheckShouldInvokeBase(this.instance))
                    base.resetProperty();
                else
                    mresetProperty_2.Invoke(this.instance);
            }

            public override void destroyModel()
            {
                if (mdestroyModel_3.CheckShouldInvokeBase(this.instance))
                    base.destroyModel();
                else
                    mdestroyModel_3.Invoke(this.instance);
            }

            public override System.Single getAnimationLength(System.String name)
            {
                if (mgetAnimationLength_4.CheckShouldInvokeBase(this.instance))
                    return base.getAnimationLength(name);
                else
                    return mgetAnimationLength_4.Invoke(this.instance, name);
            }

            public override void notifyComponentChanged(global::GameComponent component)
            {
                if (mnotifyComponentChanged_5.CheckShouldInvokeBase(this.instance))
                    base.notifyComponentChanged(component);
                else
                    mnotifyComponentChanged_5.Invoke(this.instance, component);
            }

            protected override void initComponents()
            {
                if (minitComponents_6.CheckShouldInvokeBase(this.instance))
                    base.initComponents();
                else
                    minitComponents_6.Invoke(this.instance);
            }

            protected override void notifyModelLoaded(UnityEngine.GameObject go)
            {
                if (mnotifyModelLoaded_7.CheckShouldInvokeBase(this.instance))
                    base.notifyModelLoaded(go);
                else
                    mnotifyModelLoaded_7.Invoke(this.instance, go);
            }

            public override void destroy()
            {
                if (mdestroy_8.CheckShouldInvokeBase(this.instance))
                    base.destroy();
                else
                    mdestroy_8.Invoke(this.instance);
            }

            public override void setObject(UnityEngine.GameObject obj, System.Boolean destroyOld)
            {
                if (msetObject_9.CheckShouldInvokeBase(this.instance))
                    base.setObject(obj, destroyOld);
                else
                    msetObject_9.Invoke(this.instance, obj, destroyOld);
            }

            public override void update(System.Single elapsedTime)
            {
                if (mupdate_10.CheckShouldInvokeBase(this.instance))
                    base.update(elapsedTime);
                else
                    mupdate_10.Invoke(this.instance, elapsedTime);
            }

            public override void fixedUpdate(System.Single elapsedTime)
            {
                if (mfixedUpdate_11.CheckShouldInvokeBase(this.instance))
                    base.fixedUpdate(elapsedTime);
                else
                    mfixedUpdate_11.Invoke(this.instance, elapsedTime);
            }

            public override UnityEngine.Vector3 localToWorld(UnityEngine.Vector3 point)
            {
                if (mlocalToWorld_12.CheckShouldInvokeBase(this.instance))
                    return base.localToWorld(point);
                else
                    return mlocalToWorld_12.Invoke(this.instance, point);
            }

            public override UnityEngine.Vector3 worldToLocal(UnityEngine.Vector3 point)
            {
                if (mworldToLocal_13.CheckShouldInvokeBase(this.instance))
                    return base.worldToLocal(point);
                else
                    return mworldToLocal_13.Invoke(this.instance, point);
            }

            public override UnityEngine.Vector3 localToWorldDirection(UnityEngine.Vector3 direction)
            {
                if (mlocalToWorldDirection_14.CheckShouldInvokeBase(this.instance))
                    return base.localToWorldDirection(direction);
                else
                    return mlocalToWorldDirection_14.Invoke(this.instance, direction);
            }

            public override UnityEngine.Vector3 worldToLocalDirection(UnityEngine.Vector3 direction)
            {
                if (mworldToLocalDirection_15.CheckShouldInvokeBase(this.instance))
                    return base.worldToLocalDirection(direction);
                else
                    return mworldToLocalDirection_15.Invoke(this.instance, direction);
            }

            public override UnityEngine.GameObject getObject()
            {
                if (mgetObject_16.CheckShouldInvokeBase(this.instance))
                    return base.getObject();
                else
                    return mgetObject_16.Invoke(this.instance);
            }

            public override UnityEngine.Vector3 getPosition()
            {
                if (mgetPosition_17.CheckShouldInvokeBase(this.instance))
                    return base.getPosition();
                else
                    return mgetPosition_17.Invoke(this.instance);
            }

            public override UnityEngine.Vector3 getRotation()
            {
                if (mgetRotation_18.CheckShouldInvokeBase(this.instance))
                    return base.getRotation();
                else
                    return mgetRotation_18.Invoke(this.instance);
            }

            public override UnityEngine.Vector3 getScale()
            {
                if (mgetScale_19.CheckShouldInvokeBase(this.instance))
                    return base.getScale();
                else
                    return mgetScale_19.Invoke(this.instance);
            }

            public override UnityEngine.Vector3 getWorldPosition()
            {
                if (mgetWorldPosition_20.CheckShouldInvokeBase(this.instance))
                    return base.getWorldPosition();
                else
                    return mgetWorldPosition_20.Invoke(this.instance);
            }

            public override UnityEngine.Vector3 getWorldScale()
            {
                if (mgetWorldScale_21.CheckShouldInvokeBase(this.instance))
                    return base.getWorldScale();
                else
                    return mgetWorldScale_21.Invoke(this.instance);
            }

            public override UnityEngine.Vector3 getWorldRotation()
            {
                if (mgetWorldRotation_22.CheckShouldInvokeBase(this.instance))
                    return base.getWorldRotation();
                else
                    return mgetWorldRotation_22.Invoke(this.instance);
            }

            public override System.Boolean isActive()
            {
                if (misActive_23.CheckShouldInvokeBase(this.instance))
                    return base.isActive();
                else
                    return misActive_23.Invoke(this.instance);
            }

            public override System.Boolean isActiveInHierarchy()
            {
                if (misActiveInHierarchy_24.CheckShouldInvokeBase(this.instance))
                    return base.isActiveInHierarchy();
                else
                    return misActiveInHierarchy_24.Invoke(this.instance);
            }

            public override System.Boolean isHandleInput()
            {
                if (misHandleInput_25.CheckShouldInvokeBase(this.instance))
                    return base.isHandleInput();
                else
                    return misHandleInput_25.Invoke(this.instance);
            }

            public override UnityEngine.Collider getCollider()
            {
                if (mgetCollider_26.CheckShouldInvokeBase(this.instance))
                    return base.getCollider();
                else
                    return mgetCollider_26.Invoke(this.instance);
            }

            public override global::UIDepth getDepth()
            {
                if (mgetDepth_27.CheckShouldInvokeBase(this.instance))
                    return base.getDepth();
                else
                    return mgetDepth_27.Invoke(this.instance);
            }

            public override System.Boolean isReceiveScreenMouse()
            {
                if (misReceiveScreenMouse_28.CheckShouldInvokeBase(this.instance))
                    return base.isReceiveScreenMouse();
                else
                    return misReceiveScreenMouse_28.Invoke(this.instance);
            }

            public override System.Boolean isPassRay()
            {
                if (misPassRay_29.CheckShouldInvokeBase(this.instance))
                    return base.isPassRay();
                else
                    return misPassRay_29.Invoke(this.instance);
            }

            public override System.Boolean isDragable()
            {
                if (misDragable_30.CheckShouldInvokeBase(this.instance))
                    return base.isDragable();
                else
                    return misDragable_30.Invoke(this.instance);
            }

            public override System.Boolean isMouseHovered()
            {
                if (misMouseHovered_31.CheckShouldInvokeBase(this.instance))
                    return base.isMouseHovered();
                else
                    return misMouseHovered_31.Invoke(this.instance);
            }

            public override System.Boolean isChildOf(global::IMouseEventCollect parent)
            {
                if (misChildOf_32.CheckShouldInvokeBase(this.instance))
                    return base.isChildOf(parent);
                else
                    return misChildOf_32.Invoke(this.instance, parent);
            }

            public override void setPassRay(System.Boolean passRay)
            {
                if (msetPassRay_33.CheckShouldInvokeBase(this.instance))
                    base.setPassRay(passRay);
                else
                    msetPassRay_33.Invoke(this.instance, passRay);
            }

            public override void setHandleInput(System.Boolean handleInput)
            {
                if (msetHandleInput_34.CheckShouldInvokeBase(this.instance))
                    base.setHandleInput(handleInput);
                else
                    msetHandleInput_34.Invoke(this.instance, handleInput);
            }

            public override void setName(System.String name)
            {
                if (msetName_35.CheckShouldInvokeBase(this.instance))
                    base.setName(name);
                else
                    msetName_35.Invoke(this.instance, name);
            }

            public override void setActive(System.Boolean active)
            {
                if (msetActive_36.CheckShouldInvokeBase(this.instance))
                    base.setActive(active);
                else
                    msetActive_36.Invoke(this.instance, active);
            }

            public override void setPosition(UnityEngine.Vector3 pos)
            {
                if (msetPosition_37.CheckShouldInvokeBase(this.instance))
                    base.setPosition(pos);
                else
                    msetPosition_37.Invoke(this.instance, pos);
            }

            public override void setScale(UnityEngine.Vector3 scale)
            {
                if (msetScale_38.CheckShouldInvokeBase(this.instance))
                    base.setScale(scale);
                else
                    msetScale_38.Invoke(this.instance, scale);
            }

            public override void setRotation(UnityEngine.Vector3 rot)
            {
                if (msetRotation_39.CheckShouldInvokeBase(this.instance))
                    base.setRotation(rot);
                else
                    msetRotation_39.Invoke(this.instance, rot);
            }

            public override void setWorldPosition(UnityEngine.Vector3 pos)
            {
                if (msetWorldPosition_40.CheckShouldInvokeBase(this.instance))
                    base.setWorldPosition(pos);
                else
                    msetWorldPosition_40.Invoke(this.instance, pos);
            }

            public override void setWorldRotation(UnityEngine.Vector3 rot)
            {
                if (msetWorldRotation_41.CheckShouldInvokeBase(this.instance))
                    base.setWorldRotation(rot);
                else
                    msetWorldRotation_41.Invoke(this.instance, rot);
            }

            public override void setWorldScale(UnityEngine.Vector3 scale)
            {
                if (msetWorldScale_42.CheckShouldInvokeBase(this.instance))
                    base.setWorldScale(scale);
                else
                    msetWorldScale_42.Invoke(this.instance, scale);
            }

            public override void move(UnityEngine.Vector3 moveDelta, UnityEngine.Space space)
            {
                if (mmove_43.CheckShouldInvokeBase(this.instance))
                    base.move(moveDelta, space);
                else
                    mmove_43.Invoke(this.instance, moveDelta, space);
            }

            public override void setClickCallback(global::ObjectClickCallback callback)
            {
                if (msetClickCallback_44.CheckShouldInvokeBase(this.instance))
                    base.setClickCallback(callback);
                else
                    msetClickCallback_44.Invoke(this.instance, callback);
            }

            public override void setHoverCallback(global::ObjectHoverCallback callback)
            {
                if (msetHoverCallback_45.CheckShouldInvokeBase(this.instance))
                    base.setHoverCallback(callback);
                else
                    msetHoverCallback_45.Invoke(this.instance, callback);
            }

            public override void setPressCallback(global::ObjectPressCallback callback)
            {
                if (msetPressCallback_46.CheckShouldInvokeBase(this.instance))
                    base.setPressCallback(callback);
                else
                    msetPressCallback_46.Invoke(this.instance, callback);
            }

            public override void onMouseEnter()
            {
                if (monMouseEnter_47.CheckShouldInvokeBase(this.instance))
                    base.onMouseEnter();
                else
                    monMouseEnter_47.Invoke(this.instance);
            }

            public override void onMouseLeave()
            {
                if (monMouseLeave_48.CheckShouldInvokeBase(this.instance))
                    base.onMouseLeave();
                else
                    monMouseLeave_48.Invoke(this.instance);
            }

            public override void onMouseDown(UnityEngine.Vector3 mousePos)
            {
                if (monMouseDown_49.CheckShouldInvokeBase(this.instance))
                    base.onMouseDown(mousePos);
                else
                    monMouseDown_49.Invoke(this.instance, mousePos);
            }

            public override void onMouseUp(UnityEngine.Vector3 mousePos)
            {
                if (monMouseUp_50.CheckShouldInvokeBase(this.instance))
                    base.onMouseUp(mousePos);
                else
                    monMouseUp_50.Invoke(this.instance, mousePos);
            }

            public override void onMouseMove(ref UnityEngine.Vector3 mousePos, ref UnityEngine.Vector3 moveDelta, System.Single moveTime)
            {
                if (monMouseMove_51.CheckShouldInvokeBase(this.instance))
                    base.onMouseMove(ref mousePos, ref moveDelta, moveTime);
                else
                    monMouseMove_51.Invoke(this.instance, ref mousePos, ref moveDelta, moveTime);
            }

            public override void onMouseStay(UnityEngine.Vector3 mousePos)
            {
                if (monMouseStay_52.CheckShouldInvokeBase(this.instance))
                    base.onMouseStay(mousePos);
                else
                    monMouseStay_52.Invoke(this.instance, mousePos);
            }

            public override void onScreenMouseDown(UnityEngine.Vector3 mousePos)
            {
                if (monScreenMouseDown_53.CheckShouldInvokeBase(this.instance))
                    base.onScreenMouseDown(mousePos);
                else
                    monScreenMouseDown_53.Invoke(this.instance, mousePos);
            }

            public override void onScreenMouseUp(UnityEngine.Vector3 mousePos)
            {
                if (monScreenMouseUp_54.CheckShouldInvokeBase(this.instance))
                    base.onScreenMouseUp(mousePos);
                else
                    monScreenMouseUp_54.Invoke(this.instance, mousePos);
            }

            public override void onReceiveDrag(global::IMouseEventCollect dragObj, ref System.Boolean continueEvent)
            {
                if (monReceiveDrag_55.CheckShouldInvokeBase(this.instance))
                    base.onReceiveDrag(dragObj, ref continueEvent);
                else
                    monReceiveDrag_55.Invoke(this.instance, dragObj, ref continueEvent);
            }

            public override void onDragHoverd(global::IMouseEventCollect dragObj, System.Boolean hover)
            {
                if (monDragHoverd_56.CheckShouldInvokeBase(this.instance))
                    base.onDragHoverd(dragObj, hover);
                else
                    monDragHoverd_56.Invoke(this.instance, dragObj, hover);
            }

            public override void onMultiTouchStart(UnityEngine.Vector2 touch0, UnityEngine.Vector2 touch1)
            {
                if (monMultiTouchStart_57.CheckShouldInvokeBase(this.instance))
                    base.onMultiTouchStart(touch0, touch1);
                else
                    monMultiTouchStart_57.Invoke(this.instance, touch0, touch1);
            }

            public override void onMultiTouchMove(UnityEngine.Vector2 touch0, UnityEngine.Vector2 lastTouch0, UnityEngine.Vector2 touch1, UnityEngine.Vector2 lastTouch1)
            {
                if (monMultiTouchMove_58.CheckShouldInvokeBase(this.instance))
                    base.onMultiTouchMove(touch0, lastTouch0, touch1, lastTouch1);
                else
                    monMultiTouchMove_58.Invoke(this.instance, touch0, lastTouch0, touch1, lastTouch1);
            }

            public override void onMultiTouchEnd()
            {
                if (monMultiTouchEnd_59.CheckShouldInvokeBase(this.instance))
                    base.onMultiTouchEnd();
                else
                    monMultiTouchEnd_59.Invoke(this.instance);
            }

            public override void setAlpha(System.Single alpha)
            {
                if (msetAlpha_60.CheckShouldInvokeBase(this.instance))
                    base.setAlpha(alpha);
                else
                    msetAlpha_60.Invoke(this.instance, alpha);
            }

            public override System.Single getAlpha()
            {
                if (mgetAlpha_61.CheckShouldInvokeBase(this.instance))
                    return base.getAlpha();
                else
                    return mgetAlpha_61.Invoke(this.instance);
            }

            public override System.Boolean isEnable()
            {
                if (misEnable_62.CheckShouldInvokeBase(this.instance))
                    return base.isEnable();
                else
                    return misEnable_62.Invoke(this.instance);
            }

            public override void setEnable(System.Boolean enable)
            {
                if (msetEnable_63.CheckShouldInvokeBase(this.instance))
                    base.setEnable(enable);
                else
                    msetEnable_63.Invoke(this.instance, enable);
            }

            public override void lateUpdate(System.Single elapsedTime)
            {
                if (mlateUpdate_64.CheckShouldInvokeBase(this.instance))
                    base.lateUpdate(elapsedTime);
                else
                    mlateUpdate_64.Invoke(this.instance, elapsedTime);
            }

            public override void notifyAddComponent(global::GameComponent component)
            {
                if (mnotifyAddComponent_65.CheckShouldInvokeBase(this.instance))
                    base.notifyAddComponent(component);
                else
                    mnotifyAddComponent_65.Invoke(this.instance, component);
            }

            public override void setIgnoreTimeScale(System.Boolean ignore, System.Boolean componentOnly)
            {
                if (msetIgnoreTimeScale_66.CheckShouldInvokeBase(this.instance))
                    base.setIgnoreTimeScale(ignore, componentOnly);
                else
                    msetIgnoreTimeScale_66.Invoke(this.instance, ignore, componentOnly);
            }

            public override void receiveCommand(global::Command cmd)
            {
                if (mreceiveCommand_67.CheckShouldInvokeBase(this.instance))
                    base.receiveCommand(cmd);
                else
                    mreceiveCommand_67.Invoke(this.instance, cmd);
            }

            public override System.String getName()
            {
                if (mgetName_68.CheckShouldInvokeBase(this.instance))
                    return base.getName();
                else
                    return mgetName_68.Invoke(this.instance);
            }

            public override void setDestroy(System.Boolean isDestroy)
            {
                if (msetDestroy_69.CheckShouldInvokeBase(this.instance))
                    base.setDestroy(isDestroy);
                else
                    msetDestroy_69.Invoke(this.instance, isDestroy);
            }

            public override System.Boolean isDestroy()
            {
                if (misDestroy_70.CheckShouldInvokeBase(this.instance))
                    return base.isDestroy();
                else
                    return misDestroy_70.Invoke(this.instance);
            }

            public override void setAssignID(System.UInt64 assignID)
            {
                if (msetAssignID_71.CheckShouldInvokeBase(this.instance))
                    base.setAssignID(assignID);
                else
                    msetAssignID_71.Invoke(this.instance, assignID);
            }

            public override System.UInt64 getAssignID()
            {
                if (mgetAssignID_72.CheckShouldInvokeBase(this.instance))
                    return base.getAssignID();
                else
                    return mgetAssignID_72.Invoke(this.instance);
            }

            public override void notifyConstructDone()
            {
                if (mnotifyConstructDone_73.CheckShouldInvokeBase(this.instance))
                    base.notifyConstructDone();
                else
                    mnotifyConstructDone_73.Invoke(this.instance);
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

