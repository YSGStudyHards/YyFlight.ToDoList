/*
 Navicat Premium Data Transfer

 Source Server         : MongoDB
 Source Server Type    : MongoDB
 Source Server Version : 60002
 Source Host           : 124.222.133.47:27017
 Source Schema         : yyflight_todolist

 Target Server Type    : MongoDB
 Target Server Version : 60002
 File Encoding         : 65001

 Date: 26/02/2023 17:05:28
*/


// ----------------------------
// Collection structure for yyflight_todolist_content
// ----------------------------
db.getCollection("yyflight_todolist_content").drop();
db.createCollection("yyflight_todolist_content");

// ----------------------------
// Documents of yyflight_todolist_content
// ----------------------------
db.getCollection("yyflight_todolist_content").insert([ {
    _id: ObjectId("63fb20299b4f000077004c65"),
    UserID: "63949e2d9f602f6bdcc35208",
    Content: "今天任务熟记100个英语单词",
    ExpirationTime: ISODate("2023-02-10T14:56:45.531Z"),
    Isremind: true,
    RemindTime: 1,
    CompleteStatus: 0,
    CreateDate: ISODate("2023-02-10T14:56:45.531Z"),
    UpdateDate: ISODate("2023-02-10T14:56:45.531Z")
} ]);

// ----------------------------
// Collection structure for yyflight_todolist_updatelog
// ----------------------------
db.getCollection("yyflight_todolist_updatelog").drop();
db.createCollection("yyflight_todolist_updatelog");

// ----------------------------
// Documents of yyflight_todolist_updatelog
// ----------------------------
db.getCollection("yyflight_todolist_updatelog").insert([ {
    _id: ObjectId("63fb20c29b4f000077004c66"),
    UpdateContent: "系统界面优化升级",
    CreateDate: ISODate("2023-02-10T14:56:45.531Z"),
    UpdateDate: ISODate("2023-02-10T14:56:45.531Z")
} ]);

// ----------------------------
// Collection structure for yyflight_todolist_user
// ----------------------------
db.getCollection("yyflight_todolist_user").drop();
db.createCollection("yyflight_todolist_user");

// ----------------------------
// Documents of yyflight_todolist_user
// ----------------------------
db.getCollection("yyflight_todolist_user").insert([ {
    _id: ObjectId("63fb1dfa9b4f000077004c64"),
    UserName: "admin123",
    Password: "E10ADC3949BA59ABBE56E057F20F883E",
    NickName: "Edwin",
    HeadPortrait: "https://images.cnblogs.com/cnblogs_com/Can-daydayup/1976329/o_210517164541myMpQrcode.png",
    Email: "1070342164@qq.com",
    Status: 1,
    CreateDate: ISODate("2023-02-10T14:56:45.531Z"),
    UpdateDate: ISODate("2023-02-10T14:56:45.531Z")
} ]);
