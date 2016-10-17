 Date.prototype.format = function (format) {
        /* 
        * format="yyyy-MM-dd hh:mm:ss"; 
        */
        var o = {
            "M+": this.getMonth() + 1,
            "d+": this.getDate(),
            "h+": this.getHours(),
            "m+": this.getMinutes(),
            "s+": this.getSeconds(),
            "q+": Math.floor((this.getMonth() + 3) / 3),
            "S": this.getMilliseconds()
        }

        if (/(y+)/.test(format)) {
            format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4
            - RegExp.$1.length));
        }

        for (var k in o) {
            if (new RegExp("(" + k + ")").test(format)) {
                format = format.replace(RegExp.$1, RegExp.$1.length == 1
                ? o[k]
                : ("00" + o[k]).substr(("" + o[k]).length));
            }
        }
        return format;
 }


 /** 
  * @param {} btime 
  * @param {} etime 
  * @param {} type  是否时间戳
  * @returns {} 
  */
 function getdiff(btime, type, etime) {
     btime = btime.replace("年", "/").replace("月", "/").replace("日", "");
     if (type) {
         btime = parseInt(btime.replace("/Date(", '').replace(")/", ''));
     }
     var days = 0;
     var edntime = new Date();
     if (etime != null && etime != '') {
         etime = etime.replace("年", "/").replace("月", "/").replace("日", "");
         if (type) {
             etime = parseInt(etime.replace("/Date(", '').replace(")/", ''));
         }
         edntime = new Date(etime);
     }
     days = edntime.getDate() - new Date(btime).getDate();
     if (days < 1) {
         days = edntime.getHours() - new Date(btime).getHours();
         if (days < 1) {
             days = (edntime.getMinutes() - new Date(btime).getMinutes()) + '分钟';
         } else {
             days = days + '小时';
         }
     } else {
         days = days + '天';
     }
     return days;
 }

 function convertdate(btime, type) {
     btime = btime.replace("年", "/").replace("月", "/").replace("日", "");
     if (type) {
         btime = parseInt(btime.replace("/Date(", '').replace(")/", ''));
     }
     return new Date(btime).format("yyyy-MM-dd");
 }
