using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEnum
{

    /// <summary>
    /// 状态枚举
    /// </summary>
    public enum EnumStatus
    {
        [Description("全部")]
        All = -1,
        [DescriptionAttribute("禁用")]
        Invalid = 0,
        [DescriptionAttribute("正常")]
        Valid = 1,
        [DescriptionAttribute("删除")]
        Delete = 9
    }
    /// <summary>
    /// 执行状态码
    /// </summary>
    public enum EnumResultStatus
    {
        [DescriptionAttribute("失败")]
        Failed = 0,
        [DescriptionAttribute("成功")]
        Success = 1,
        [DescriptionAttribute("无权限")]
        IsLimit = 10000,
        [DescriptionAttribute("系统错误")]
        Error = 10001,
        [DescriptionAttribute("存在数据")]
        Exists = 10002
    }
    public enum EnumDateType
    {
        Year = 1,
        Quarter = 2,
        Month = 3,
        Week = 4,
        Day = 5,
        Hour=6,
        Other=7
    }
    public enum EnumUserOperateType
    {
        [DescriptionAttribute("登录")]
        Login = 0,
        [DescriptionAttribute("发表日志")]
        SendLog = 1,
        [DescriptionAttribute("查看联系信息")]
        SeeLink = 2,
        [DescriptionAttribute("浏览用户")]
        SeeUser = 3,
        [DescriptionAttribute("其他")]
        Other = 4,
        [DescriptionAttribute("错误")]
        Error = 5,
        [DescriptionAttribute("购买")]
        Pay = 6,
        [DescriptionAttribute("后台管理")]
        Manage = 7,
    }
}
