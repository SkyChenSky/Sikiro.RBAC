using System;
using System.Collections.Generic;

namespace Sikiro.Common.Utils
{
    public class AccountHelper
    {
        public const string AllPerson = "000000000000000000000000";


        /// <summary>
        /// 得到验证码
        /// </summary>
        /// <param name="num">验证码的长度</param>
        /// <returns></returns>
        public static string GetVcode(int num)
        {
            Random r = new Random();
            //生成验证码的集合
            char[] arr = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            //生成num个随机的验证码
            string strVcode = string.Empty;
            for (int i = 0; i < num; i++)
            {
                strVcode += arr[r.Next(arr.Length)];
            }
            return strVcode;
        }

        /// <summary>
        /// 一周的开始
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime GetWeekFirstDayMon(DateTime datetime)
        {
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = (weeknow == 0 ? (7 - 1) : (weeknow - 1));
            int daydiff = (-1) * weeknow;
            string FirstDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(FirstDay);
        }

        /// <summary>
        /// 一周的结束
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static DateTime GetWeekLastDaySun(DateTime datetime)
        {
            int weeknow = Convert.ToInt32(datetime.DayOfWeek);
            weeknow = (weeknow == 0 ? 7 : weeknow);
            int daydiff = (7 - weeknow);
            string LastDay = datetime.AddDays(daydiff).ToString("yyyy-MM-dd");
            return Convert.ToDateTime(LastDay);

        }


        /// <summary>
        /// 本月的开始
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMonthStart()
        {

            return DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
        }
        /// <summary>
        /// 本月的结束
        /// </summary>
        /// <returns></returns>
        public static DateTime GetMonthEnd()
        {

            return DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1);
        }


        public static string GetDateName()
        {
            var hour = DateTime.Now.Hour;
            if (hour == 12 && hour < 13)
            {
                return "中午好";
            }
            if (hour >= 13 && hour < 19)
            {
                return "下午好";
            }
            if (hour >= 19)
            {
                return "晚上好";
            }
            return "早上好";
        }



        public static string GetTimeName(DateTime dt)
        {
            var now = DateTime.Now;
            TimeSpan interval = now - dt;
            var seconds = interval.TotalSeconds;
            if (seconds < 60 && seconds > 0)
            {
                return "1分钟前";
            }

            if (seconds < (60 * 5 * 12) && seconds >= 60)
            {
                return "一个小时前";
            }
            if (seconds < (60 * 5 * 12 * 24) && seconds >= (60 * 5 * 12))
            {
                return "一天前";
            }
            if (seconds < (60 * 5 * 12 * 24 * 3) && seconds >= (60 * 5 * 12 * 24))
            {
                return "三天前";
            }


            if (seconds < (60 * 5 * 12 * 24 * 7) && seconds >= (60 * 5 * 12 * 24 * 3))
            {
                return "七天前";
            }

            return dt.ToString("yyyy-MM-dd");
        }



        /// <summary>
        /// 用户信息-查询的进度条
        /// </summary>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <param name="openId"></param>
        /// <returns></returns>
        public static decimal GetProgress(string name, string phone, string openId)
        {
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone) && !string.IsNullOrEmpty(openId))
            {
                return 100m;
            }
            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(phone) && string.IsNullOrEmpty(openId))
            {
                return 0m;
            }
            var d = 0m;
            if (!string.IsNullOrEmpty(name))
            {
                d = d + 33.33m;
            }
            if (!string.IsNullOrEmpty(phone))
            {
                d = d + 33.33m;
            }
            if (!string.IsNullOrEmpty(openId))
            {
                d = d + 33.33m;
            }
            return d;
        }
        public static List<string> GetLableList()
        {
            var lis = new List<string>()
             {
                 "layui-icon-rate-half",
                 "layui-icon-rate",
                 "layui-icon-rate-solid",
                 "layui-icon-cellphone",
                 "layui-icon-password",
                 "layui-icon-username",
                 "layui-icon-refresh-3",
                 "layui-icon-auz",
                 "layui-icon-snowflake",
                 "layui-icon-tips",
                 "layui-icon-note",
                 "layui-icon-home",
                 "layui-icon-website",
                 "layui-icon-senior",
                 "layui-icon-flag",
                 "layui-icon-notice",
                 "layui-icon-set",
                 "layui-icon-template-1",
                 "layui-icon-app",
                 "layui-icon-read",
                 "layui-icon-user",
                 "layui-icon-group",
                 "layui-icon-friends",
                 "layui-icon-set-sm",
                 "layui-icon-star",
                 "layui-icon-star-fill",
                 "layui-icon-chat",
                 "layui-icon-date"
             };

            return lis;
        }
        /// <summary>
        /// 菜单权限
        /// </summary>
        /// <returns></returns>
        public static List<string> GetMenuAction()
        {
            List<string> lis = new List<string>()
            {
                "查看",
                "添加",
                "修改",
                "删除",
                "审核",
                "导入",
                "导出",
                "禁用",
                "重置"
            };


            return lis;
        }
        /// <summary>
        /// 目的国家
        /// </summary>
        /// <returns></returns>

        public static List<string> GetFormStateAdress()
        {
            List<string> lis = new List<string>()
            {
                "柬埔寨-金边",
                "柬埔寨-西哈努克(西港",
                "柬埔寨-暹粒(吴哥)",
                "柬埔寨-木牌(巴域)",
                "柬埔寨-其他",
                "越南-胡志明",
                "越南-河内",
                "缅甸",
                "缅甸-曼德勒(瓦城)",
                "马来西亚",
                "新加坡",
                "泰国",
                "中国-大陆",
                "中国-香港",
                "中国-台湾"
            };
            return lis;
        }

        /// <summary>
        /// 取货方式
        /// </summary>
        /// <returns></returns>
        public static List<string> GetFormStateClaimGoods()
        {
            List<string> lis = new List<string>()
            {
                "送货上门",
                "自提点取货"

            };
            return lis;
        }

        /// <summary>
        /// 时效
        /// </summary>
        /// <returns></returns>
        public static List<string> GetFormAging()
        {
            List<string> lis = new List<string>()
            {
                "A+",
                "A",
                "B",
                "C",
                "C1",
                "D"


            };
            return lis;
        }


        /// <summary>
        /// 付款方式
        /// </summary>
        /// <returns></returns>
        public static List<string> GetPayment()
        {

            List<string> lis = new List<string>()
            {
                "微信付款",
                "银行转账",
                "到付现金"

            };
            return lis;
        }

        /// <summary>
        /// 时间段
        /// </summary>
        /// <returns></returns>
        public static List<string> GetTimeQuantum()
        {

            List<string> lis = new List<string>()
            {
                "上午",
                "下午",
                "傍晚",
                "夜间"

            };
            return lis;
        }

        public static List<string> GetTimeSection()
        {
            List<string> lis = new List<string>()
            {
                "8:00-10:00",
                "10:00-12:00",
                "12:00-14:00",
                "14:00-16:00",
                "16:00-18:00",
                "18:00-20:00",
                "20:00-22:00"

            };
            return lis;
        }

    }
}
