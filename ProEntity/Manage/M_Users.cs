/**  版本信息模板在安装目录下，可自行修改。
* M_Users.cs
*
* 功 能： N/A
* 类 名： M_Users
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/3/6 19:52:53   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
namespace ProEntity.Manage
{
	/// <summary>
	/// M_Users:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class M_Users:UserReport
	{
		public M_Users()
		{}


        public List<Menu> Menus { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int AutoID{ get; set; } 
		/// <summary>
		/// 
		/// </summary>
		public string LoginName{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string LoginPWD{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Name{ get; set; }

        public string RoleID{ get; set; }

        public M_Role Role{ get; set; }
        public int? Age { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Email{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string MobilePhone{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string OfficePhone{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Jobs{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Avatar{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? IsAdmin{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public int? Status{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public string Description{ get; set; }
		/// <summary>
		/// 
		/// </summary>
		public DateTime? CreateTime{ get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Property("Lower")] 
        public string CreateUserID{ get; set; }

        public M_Users CreateUser{ get; set; }

        public int? Sex { get; set; }

        public int? IsMarry { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string District { get; set; }

        public string Education { get; set; }

        public string BHeight { get; set; }

        public string QQ { get; set; }
        /// <summary>
        /// 等级
        /// </summary>
	    public string Levelid { get; set; }
        /// <summary>
        /// 体重
        /// </summary>
        public string BWeight { get; set; }
        /// <summary>
        /// 自评
        /// </summary>
        public string MyContent { get; set; }
        /// <summary>
        /// 宣言
        /// </summary>
        public string TalkTo { get; set; }
        public DateTime BirthDay { get; set; }
        /// <summary>
        /// 个性
        /// </summary>
        public string MyCharacter { get; set; }
        /// <summary>
        /// 工资
        /// </summary>
        public string BPay { get; set; }
        public decimal Account { get; set; }
        public decimal InAccount { get; set; }
        public decimal OutAccount { get; set; }
	    public string MyService { get; set; } 
	    /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="dr"></param>
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }

	}
}

