# MyFramework

框架特点:


组件: 部分地方使用组件思想,与unity类似.

命令系统: 使用命令系统作为事件发送.将逻辑封装为命令,在一个类中处理一个事件所涉及的所有逻辑.

兼容UGUI和NGUI: 主要使用UGUI,也可以兼容NGUI.

完善的UI框架: 每个类型的UI都有对应的窗口封装,避免直接访问UI组件.封装对于UI布局的各种操作.

自定义输入检测: 重写全局输入检测,不使用UGUI的EventSystem,不过部分UGUI的组件仍然可以EventSystem,比如ScrollRect.

自定义缓动效果: 自己使用组件的方式实现各种缓动效果.用于实现UI和可移动物体的各种变换效果.不依赖DoTween等插件

显式打包图集: 图集使用TexturePacker进行手动打包,不使用UGUI自动打图集.因为希望能完全掌控图集.

ILRuntime热更: 加入了ILRuntime实现代码热更功能.并且Frame层位于主工程,热更工程只有游戏自身的逻辑代码.

游戏流程划分: 场景流程划分可以更加清晰地知道当前游戏所处的状态.

不使用Unity的基于MonoBehaviour的工作流: 极少使用MonoBehaviour,避免过度依赖Unity的工作流.

自定义分辨率适配: 手动编写组件实现不同分辨率的适配.不使用UGUI的锚点.因为需要对代码的执行有最大程度上的把控.摒弃很多自动执行,但是不容易掌控的部分功能.

自定义网络系统: 基于TCP实现服务器或者客户端.

自定义网络消息序列化: 不使用ProtoBuf等序列化插件,因为不考虑跨平台或者跨语言的需求,仅仅只是为了实现最低需求且最高效率的消息序列化和反序列化.

不使用语言高级特性:为了使代码易于阅读,尽量避免了使用语言的高级特性,极少使用协程,lambda表达式等.

完善的资源管理系统:无缝衔接从AssetBundle加载或从AssetDataBase加载,应用层不会感知资源是从哪儿加载的.AssetBundle或AssetDataBase都支持同步和异步加载.暂不考虑Addressable是因为Addressable只支持异步加载,有些时候需要以同步的方式加载资源.

完善的工具函数: 编写了BinaryUtility,StringUtility,MathUtility,FileUtility,UnityUtility等功能完善的工具函数,重写了几乎所有可能会用到了工具函数,从而避免直接调用Unity或者C#本身的工具函数.并且部分工具函数由于减少了堆内存的使用,而比内置工具函数更加高效.FileUtility也完善支持在Android真机上的文件读写.

完善的SQLite支持: 如果使用SQLite配置表格数据,会有完善的对SQLite的访问方式.

只是项目框架,不包含业务逻辑

这是自己写的一个unity游戏框架,使用了命令,组件等思想,依赖的插件有avprovideo,TexturePacker(其实并没有依赖,只是用来生成图集)

基本思想还是逻辑与界面分离,只不过里面还添加了其他系统.

目前我自己的项目在使用这框架,公司的手游项目也在使用.

从框架的初步形成到现在都有4,5年了,最初是C++写的,后来才改成了C#.所以代码风格也是C++的风格.


里面会包含一些简单的使用方式,但是很少,因为我太懒.

启动场景为start.unity.不支持在其他场景启动.

设计上是无缝支持UGUI和NGUI的,但是由于NGUI很久没用了,所以如果NGUI用起来有问题,那也没办法.

第一次打开项目,可以按F9直接打开初始场景

现在使用的版本是Unity2018.4.21f1

//-----------------------------------------------------------------------------------------------------------------------------------------

代码中重要模块简介

Character:角色

用于表示所有的可见的,有一定逻辑的角色,一般人型模型表示的都应该是一个角色,比如战斗中的角色,主界面的角色,高模展示的角色,次级场景中的角色.

角色中一般包含一个数据类,和对该角色的访问方法.数据类中只有成员变量,不能有函数


CommandSystem:命令系统

特点:

1.封装逻辑

2.连接逻辑与界面,以及项目中任何模块

3.延迟执行

4.线程安全,在子线程中可以使用延迟命令将逻辑放到主线程执行

5.日志打印

基于以上特点,命令系统一般是用在表示一件逻辑意义上的事件,命令中可访问任意项目代码,所以当一个事件关联多个模块时,应该将相关代码封装到一个命令中

比如数据改变时,即有数据存储的改变,也有界面相应的改变,那就需要将该逻辑封装到命令中,在合适的地方调用命令即可.调用方只需要关心触发了什么事件,

而不需要关心事件具体内容.

命名规范:文件夹名字格式为Command+接收者类名,命令格式为Command+接收者类名+命令名,接收者必须为一个存在的类名,一般只使用父类名即可.

禁止将命令发送给不对应的接收者.

难以确定具体接收者类型的命令可考虑将场景作为接收者,但是尽量避免这么做.

使用延迟命令需要考虑到中断情况,也就是在还没有到命令执行的事件时不满足命令执行的条件.这时候应该将命令中断,也就是调用CommandSystem.interruptCommand(),将命令从等待列表中移除.

Common:公共代码

一般存放枚举,全局基类,当前项目使用的工具函数等.

Component:组件

类似于unity的组件思想.使用组合的方式代替继承,实现不同对象的不同行为.

命名规范:文件夹名格式为组件拥有者类名+Component,组件类名格式为组件拥有者类名+组件名,不加Component前缀是为了简化类名

组件一般是用于实现部分可以独立更新的逻辑.需要更新,并且与拥有者本身没有太大的耦合性的逻辑,可以写到组件中添加到拥有者上.

DataBase:仅用于存放表格数据,json表格,SQLite表格,或自定义表格数据

DynamicAttachScript:运行时动态添加的MonoBehaviour脚本,一般用于运行时在检视面板查看调试信息.

Game:程序入口,也是程序结构的根节点,最顶层的管理类

GameScene:游戏逻辑场景

游戏总体划分为多个逻辑场景,分别代表游戏不同的阶段,一般由所使用的资源和逻辑共同决定逻辑场景划分.

一个逻辑场景包含若干个流程,流程以树形结构存储.逻辑场景至少包含一个起始流程和退出流程.

流程表示逻辑场景内部的状态划分,流程的切换一般都会有界面的相应切换.

进入流程时的操作一般与退出流程时的操作对应,比如进入流程时打开了一个界面,那退出流程时就应该将此界面关闭.进入流程时禁用了某项操作,退出流程时就应该重新启用此操作.

流程之间不允许互相访问,流程之间应该是相对隔离的,仅允许在进入或者退出流程时判断上一个流程或者下一个流程的类型来执行不同的逻辑.

流程切换时仅会停止对旧流程的更新,启用新流程的更新,不会销毁任何流程.

逻辑场景切换时会销毁旧逻辑场景以及此场景的所有流程,加载并初始化新逻辑场景.

一个逻辑场景一般会使用若干个资源场景,并且根据流程切换资源场景显示

命名规范:逻辑场景名:场景名+Scene,流程名:场景名+Scene+父流程名(如果有父流程)+流程名

LayoutScript:界面脚本,只要是使用了Canvas的都可以归类到界面.无论是2D的UI还是场景中的3DUI.

Net:网络通信.存放网络管理器,网络消息包

SceneSystem:资源场景逻辑脚本.每一个资源场景都会对应一个逻辑脚本,允许多个相似的资源场景使用一个逻辑脚本.


//--------------------------------------------------------------------------------------------------------------------------------

项目代码规范

空格空行:

1.代码中禁止出现无意义空行.

2.尽量使用注释来代替空行,实在不需要注释的地方,并且与上文逻辑有明显区分的可使用空行分隔

3.在该打空格的地方必须打空格,比如运算符前后,等号前后,for循环中的分号后,if或者for等关键字后可以不打空格.

4.空格和缩进格式可使用Ctrl+K+F让VS使用默认的格式进行调整.


命名规范:

1.类名命名单词首字母大写.

2.类成员变量以m开头,后面的单词首字母大写,必须保证单词拼写正确.

3.函数中的临时变量以小写开头,后面的单词首字母大写.

4.函数名首字母小写.

5.部分情况下使用缩写时允许以大写字母开头,但是仍需要尽量避免这种情况.

6.回调函数以on开头,非回调函数禁止以on开头.且回调函数权限为protected.

7.以set开头的函数应该只用于根据参数设置成员变量的值,而不能做其他的事情.

8.以get开头的函数尽量不要在函数内对成员变量有修改.

9.明确is,if,can此类单词的含义,is是否,if如果,can能否,一般用于获取一个bool状态时尽量使用is

10.枚举名为全大写,单词之间以下划线分隔.

11.枚举值为全大写,单词之间以下划线分隔,可以不使用枚举名首字母为前缀.


代码结构:

1.类成员变量写在类的开头,并且访问权限为保护.

2.不允许在函数之间插入成员变量定义.

3.类函数写在成员变量之后,先写公有函数,然后加分割线,再写保护的函数.

4.不允许出现不同访问权限的函数混合排布.

5.如果一个函数的代码只有一行,则可以将整个函数写成一行,比如简单的的get和set函数

6.if下必须添加大括号,即使只有一行也需要添加大括号.


常量:

1.禁止在代码中出现意义不明的数字,需要定义常量或者枚举来代替.

2.非显示类的字符串必须定义为常量.比如某个资源的名字等

3.不需要使用readonly标识运行时常量,一般只使用编译时常量即可


注释:

1.提交的代码中禁止出现被注释的代码.

2.禁止使用除了双斜杠以外的注释形式.比如/**/局部注释,///注释

3.行注释双斜杠后需要加一个空格.

4.在代码逻辑不是非常浅显易懂的地方应该添加相应的注释,提高代码阅读效率.

5.尽量在成员变量后添加注释,用于说明此变量用处,以及使用方式等等.

6.成员变量注释或者枚举类型的注释需要对齐.


其他:

1.禁止使用switch.可使用if代替.虽然switch更快一些,但是排版太丑!而且case少的switch可以用if代替,case多的就需要用字典代替,对于需要写较多代码的case就写成类,不要直接switch-case,太难看,而且容易不易阅读.

2.提交的代码中禁止出现打印日志.日志打印目前只应该出现在命令的调试信息中,以及部分关键代码部分.

3.允许使用错误日志弹窗提示错误信息.以便及时发现表格或者是资源上不允许出现的错误.

4.界面之间不允许互相访问,也就是说一个界面不应该知道还存在有其他界面,每个界面之间的逻辑应该完全隔离.

5.不允许在工具函数中访问界面流程以及这种游戏状态相关的部分,如果有必要,应该使用命令代替.

6.界面的开启和关闭尽量写在流程的进入和退出里面.

7.尽量避免在除了命令和流程以外的地方访问布局.

8.明确界面与逻辑之间的边界,尽量保证去除界面以后逻辑仍然可以正常运行不受任何影响.

9.界面中不能直接访问网络消息.

10.资源加载时必须考虑资源的生存周期,确保不会产生无人管理的资源.

11.MonoBehaviour目前只用于部分不得不使用此脚本的地方,或者是需要保存编辑器中编辑的参数的情况,比如界面自适应中使用的脚本.

12.避免使用Unity的物理模拟,碰撞事件不使用MonoBehaviour的OnCollisionEnter等通知回调,使用主动的碰撞检测判断.

13.尽量不使用第三方插件.

14.避免直接访问unity组件,使用已封装对象提供的方法进行访问.

15.尽量使用已封装的工具函数,包括文件函数,字符串函数,数学函数,系统函数等,如果有需求也可以添加需要的通用工具函数.

16.避免使用除法,如果可以,使用乘法代替.

17.将具有一定通用性的函数抽象为工具函数放到GameUtility中.

18.谨慎申请堆内存,尽量减少GC.

19.遍历List时尽量使用for,并且定义一个变量存储列表长度,而不是i < list.Count.

20.使用++i,避免使用i++.

21.避免使用Action<>定义委托类型的函数参数,尽量显式定义委托类型.

22.尽量避免使用一些作用不是很大并且使用非常少的语法形式,尽量与整体代码风格保持一致.

23.使用浮点数的地方需要显式表示为浮点数,而不是在需要浮点数的地方使用整数.

24.避免产生警告,有警告或者运行时有非正常提示时需要避免.比如Can't add 'GridLayoutGroup' to LeftWidget because a 'VerticalLayoutGroup' is already added to the game object!

25.尽量不使用get/set属性,显式添加getXX或者setXX代替.

26.避免使用协程,仅在部分只能使用协程的情况下才用.


//-------------------------------------------------------------------------------------------------------------------------------------------

布局脚本代码规范

1.命名规范,成员变量,临时变量,函数命名规则

2.窗口变量名应该与窗口名保持一致

3.变量访问权限规范,尽量使用protected,临时的用于避免GC的变量使用private.

4.变量定义顺序应该与布局中顺序相同.

5.在每个非窗口变量后尽量添加注释.

6.成员变量非0初始化尽量写在构造函数中.

8.严格区分构造,assignWindow,init,onReset,onGameState,onShow,onHide用途

	构造:用于数据初始化,分配内存.
	
	assignWindow:仅用于查找窗口赋值到成员变量
	
	init:窗口初始化,注册窗口事件.
	
	onReset:将布局运行过程中可能会变化的成员变量重置到刚初始化完成时的状态.
	
	onGameState:根据游戏中逻辑状态和数据设置界面显示.
	
	onShow:仅用于执行界面显示时的动态效果.
	
	onHide:仅用于执行界面隐藏时的动态效果.
	
9.assignWindow中不能查找窗口赋值给临时变量

10.assignWindow中不允许使用new分配内存

11.仅在当前布局使用的函数应该设置为protected,并且放到脚本文件的最下方

12.尽量避免使用region

13.所有加载资源或者创建对象的地方必须有统一的创建和销毁

14.减少不必要的函数创建

15.尽量避免直接访问Transform,UGUI的组件等,使用已封装对象提供的方法访问

16.避免使用反射.

17.脚本中使用的内部类需要放到单独的代码文件中,放到InnerClass中

18.脚本内部类需要提供构造,assignWindow,init,reset方法.

19.脚本内部类构造函数只允许有一个LayoutScript参数

20.脚本内部类assignWindow,init规范与脚本一致

21.脚本中除了onGameState以外,其他地方尽量不能引用除了脚本自身以外的游戏逻辑对象,也不能引用其他脚本.包括不能收发网络消息

22.触发界面事件时尽量通过命令的方式向外发送事件

23.成员变量排列顺序,从上往下,窗口对象变量,列表,内部类对象,结构体,枚举,string,float,bool,int

24.避免使用lambda表达式编写匿名函数,使用常规函数代替

25.避免使用switch

26.删除已注释代码

27.事件注册回调不允许对应多个窗口,事件注册回调中不允许根据点击物体来执行不同的逻辑

28.谨慎使用new申请堆内存,确保不申请仅临时使用且执行频率较高的堆内存

29.在适当的地方使用?.和?.Invoke简化代码

30.所有内部类尽量是GameBase的子类

31.善用窗口对象池重复利用对象

32.窗口或变量命名避免使用简写,只有在名称比较长时使用简写

33.尽量添加注释

34.避免添加无用空行

