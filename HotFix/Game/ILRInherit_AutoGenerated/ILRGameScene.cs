using System;
using System.Collections;
using System.Collections.Generic;
	
public abstract class ILRGameScene : GameScene
{	
	public override void init() { base.init(); }
	public override void resetProperty() { base.resetProperty(); }
	public override void destroy() { base.destroy(); }
	public override void update(float elapsedTime) { base.update(elapsedTime); }
	public override void lateUpdate(float elapsedTime) { base.lateUpdate(elapsedTime); }
	public override void keyProcess(float elapsedTime) { base.keyProcess(elapsedTime); }
	public override void exit() { base.exit(); }
	public override void createSceneProcedure() { base.createSceneProcedure(); }
	public override void setActive(bool active) { base.setActive(active); }
	public override void fixedUpdate(float elapsedTime) { base.fixedUpdate(elapsedTime); }
	public override void notifyAddComponent(GameComponent component) { base.notifyAddComponent(component); }
	public override void setIgnoreTimeScale(bool ignore, bool componentOnly = false) { base.setIgnoreTimeScale(ignore, componentOnly); }
	protected override void initComponents() { base.initComponents(); }
	public override void receiveCommand(Command cmd) { base.receiveCommand(cmd); }
	public override string getName() { return base.getName(); }
	public override void setName(string name) { base.setName(name); }
	public override void setDestroy(bool isDestroy) { base.setDestroy(isDestroy); }
	public override bool isDestroy() { return base.isDestroy(); }
	public override void setAssignID(UInt64 assignID) { base.setAssignID(assignID); }
	public override UInt64 getAssignID() { return base.getAssignID(); }
	public override void notifyConstructDone() { base.notifyConstructDone(); }
	public override bool Equals(object obj) { return base.Equals(obj); }
	public override int GetHashCode() { return base.GetHashCode(); }
	public override string ToString() { return base.ToString(); }
}	
