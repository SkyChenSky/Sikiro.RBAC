# Blog
+ [.Net Core实战之基于角色的访问控制的设计](https://www.cnblogs.com/skychen1218/p/13053878.html)

# Tools
Download Studio 3T or other mongo view client.And Then Copy Bson Script and Excute.

# Logon Account
+ SuperAdmin
    + UserName:chengong
    + Password:123456
+ NormalAdmin
    + UserName:chengong2
    + Password:123456

# Init Data

## Administrator
```json
{ 
    "_id" : ObjectId("5edb160d5754351424147403"), 
    "RealName" : "陈珙", 
    "UserName" : "chengong", 
    "DepartmentId" : ObjectId("5edf17ebdd030c4094d7491d"), 
    "PositionIds" : [
        ObjectId("5edf1819dd030c4094d7491e")
    ], 
    "RoleIds" : [
        ObjectId("5ede11018cf4124570e620ed")
    ], 
    "Phone" : "18988561110", 
    "Password" : "8FC2C60F38E7159FF01A183D41C9BF8C", 
    "Status" : NumberInt(1), 
    "IsSuper" : true, 
    "CreateDateTime" : ISODate("2020-06-06T04:05:33.540+0000"), 
    "UpdateDateTime" : ISODate("2020-06-09T05:04:06.568+0000")
}
{ 
    "_id" : ObjectId("5edf1d10c215e61e0870b2dc"), 
    "RealName" : "具有权限限制的用户", 
    "UserName" : "chengong2", 
    "DepartmentId" : ObjectId("5edf18ef64a8aa2580c6607c"), 
    "PositionIds" : [
        ObjectId("5edf1819dd030c4094d7491e")
    ], 
    "RoleIds" : [
        ObjectId("5ede11018cf4124570e620ed")
    ], 
    "Phone" : "18988561111", 
    "Password" : "E47B0E927E1E7CA5460250ABD717836E", 
    "Status" : NumberInt(1), 
    "IsSuper" : false, 
    "CreateDateTime" : ISODate("2020-06-09T05:24:32.267+0000"), 
    "UpdateDateTime" : ISODate("2020-06-12T06:50:32.082+0000")
}

```

# Menu
```json
{ 
    "_id" : ObjectId("5edb647486f170631806a128"), 
    "Name" : "权限管理", 
    "Icon" : "layui-icon-set", 
    "Url" : null, 
    "Order" : NumberInt(0), 
    "ParentId" : null, 
    "MenuActionIds" : [

    ], 
    "CreateDateTime" : ISODate("0001-01-01T00:00:00.000+0000")
}
{ 
    "_id" : ObjectId("5edb649986f170631806a129"), 
    "Name" : "管理员", 
    "Icon" : "layui-icon-friends", 
    "Url" : "/administrator/index", 
    "Order" : NumberInt(1), 
    "ParentId" : ObjectId("5edb647486f170631806a128"), 
    "MenuActionIds" : [
        ObjectId("19b1b73d63d4c9ea79f8ca57"), 
        ObjectId("6eb887126d24e8f1cd8ad503"), 
        ObjectId("9103c8c82514f39d8360c743"), 
        ObjectId("9cdf26568d166bc6793ef8da"), 
        ObjectId("d783823cc6284b929c2cd8df"), 
        ObjectId("d89f3a35931c386956c1a402"), 
        ObjectId("f5dffc111454b227fbcdf361")
    ], 
    "CreateDateTime" : ISODate("0001-01-01T00:00:00.000+0000")
}
{ 
    "_id" : ObjectId("5edb64d586f170631806a12a"), 
    "Name" : "菜单管理", 
    "Icon" : "layui-icon-date", 
    "Url" : "/menu/index", 
    "Order" : NumberInt(1), 
    "ParentId" : ObjectId("5edb647486f170631806a128"), 
    "MenuActionIds" : [
        ObjectId("08d562c1eedd30b15b51e35d"), 
        ObjectId("10fa5eb83300e5f592b9b35a"), 
        ObjectId("c63a5650dcd0bf04b35bd712"), 
        ObjectId("d2cb583f4b5bdc51b965ae55")
    ], 
    "CreateDateTime" : ISODate("0001-01-01T00:00:00.000+0000")
}
{ 
    "_id" : ObjectId("5edde8513baed45e741c70d3"), 
    "Name" : "部门管理", 
    "Icon" : "layui-icon-date", 
    "Url" : "/department/index", 
    "Order" : NumberInt(1), 
    "ParentId" : ObjectId("5edb647486f170631806a128"), 
    "MenuActionIds" : [
        ObjectId("93a27b0bd99bac3e68a440b4"), 
        ObjectId("c1722a7941d61aad6e651a35"), 
        ObjectId("f702defbc67edb455949f46b")
    ], 
    "CreateDateTime" : ISODate("0001-01-01T00:00:00.000+0000")
}
{ 
    "_id" : ObjectId("5edded093baed45e741c70d4"), 
    "Name" : "岗位管理", 
    "Icon" : "layui-icon-username", 
    "Url" : "/position/index", 
    "Order" : NumberInt(1), 
    "ParentId" : ObjectId("5edb647486f170631806a128"), 
    "MenuActionIds" : [
        ObjectId("1ce9168a60deae4a994dbd5b"), 
        ObjectId("483d8df877b31405c1e8fe42"), 
        ObjectId("a17479231dc298309a3fda7d"), 
        ObjectId("a2369958a9645eac52b58a81")
    ], 
    "CreateDateTime" : ISODate("0001-01-01T00:00:00.000+0000")
}
{ 
    "_id" : ObjectId("5edded433baed45e741c70d5"), 
    "Name" : "角色管理", 
    "Icon" : "layui-icon-friends", 
    "Url" : "/role/index", 
    "Order" : NumberInt(1), 
    "ParentId" : ObjectId("5edb647486f170631806a128"), 
    "MenuActionIds" : [
        ObjectId("0023a1e3447fdb31836536cc"), 
        ObjectId("342b5fe6486788799659c39b"), 
        ObjectId("7b9d31aa17b849b238ab79ce"), 
        ObjectId("d3ab9b41f98222ad7b5ff8a8")
    ], 
    "CreateDateTime" : ISODate("0001-01-01T00:00:00.000+0000")
}

```
