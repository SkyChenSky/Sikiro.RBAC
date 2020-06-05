using System;
using System.Collections;
using System.Linq;

namespace Sikiro.Tookits.Base
{
    public class PageListResult
    {
        public PageListResult()
        {

        }

        public PageListResult(IEnumerable data, int totalCount)
        {
            Data = data;
            Count = totalCount;
        }

        public object Data { get; set; }

        public int Count { get; set; }

        public int Code { get; set; }

        public string Msg { get; set; }
    }

    public static class PageListResultExtension
    {
        public static PageListResult UpdateForPageListResult<T>(this PageList<T> list)
        {
            return new PageListResult(list.Items, list.Total);
        }

        public static PageListResult UpdateForPageListResult<T, TResult>(this PageList<T> list, Func<T, TResult> selector)
        {
            return new PageListResult(list.Items.Select(selector), list.Total);
        }
    }
}
