# 基于Unity的游戏框架
## 工具类Util

<lio><b>日期类</b></lio> : 时间戳的转换

<lio><b>数学类</b></lio> ：ReMap重映射，RandomNotSame生成不重复的随机数

<lio><b>路径类</b></lio> ：（提供各平台的） DataPath持久化目录， AppContentPath内容目录，相当于StreamingAssets目录 

<lio><b>Unity类</b></lio>： StringLengthIgnoreColor 无视颜色的取字符串长度， 添加和删除脚本以及脚本归一化操作等

<lio><b>验证类</b></lio>：Md5验证，数字验证，网络验证 等等

## 任务Task

任务类型：不同类型的任务流程，并自动完成对任务的重置，以便下次再次使用任务，可以拷贝Task文件夹独立使用

基于<lio><b>时间条件</b></lio>的任务： TimeCondition

基于<lio><b>次数条件</b></lio>的任务</b></lio>： TimesCondition

基于<lio><b>事件条件</b></lio>的任务</b></lio>： EvnetCondition

基于<lio><b>按钮触发器条件</b></lio>的任务</b></lio>： TriggerCondition

使用参考：<lio><b>TaskManager</b></lio>

## 缓存池 Pool

这玩意是大名鼎鼎的<b>PoolManager</b>，别人的东西我也不介绍了，具体查看使用即可

## 事件系统 Event

简单的事件管理器

## 网络系统 并且基于protobuf
使用参考：<lio><b>NetManager</b></lio>

## 资源管理系统 基于ZCode的代码，并修改了几次bug，后续准备重新封装他的打包工具

## UI框架 + 通用的可配置的UIFrame

## 计时器（基于UnityTimer）
使用参考：<lio><b>TestTimer</b></lio>

![麻辣烫配老冰棍很爽](https://github.com/dongweiPeng/XGameFramework/blob/master/QQ%E5%9B%BE%E7%89%8720180730104427.png)
