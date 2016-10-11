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
        Day = 5
    }
}
