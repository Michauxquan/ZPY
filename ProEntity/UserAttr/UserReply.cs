﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProEntity
{
    public class UserReply
    {
        public int AutoID {get;set;}
        public string ReplyID {get;set;}
        public string GUID {get;set;}
        public string UserName { get; set; }
        public string Content {get;set;}
        public int Status {get;set;}
        public string FromReplyID {get;set;}
        public string FromReplyUserID {get;set;}
        public string FromName { get; set; }
        public DateTime CreateTime {get;set;}
        public string CreateUserID { get; set; }
        public int Type { get; set; }
        public void FillData(System.Data.DataRow dr)
        {
            dr.FillData(this);
        }
    }
}
