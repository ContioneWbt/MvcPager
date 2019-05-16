using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PagerCheck
{
    public static class PagingExtensions
    {
        #region HtmlHelper extensions

        public static string Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, bool isAjax, string funcName)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, null, isAjax, funcName);
        }


        public static string Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string actionName, bool isAjax, string funcName)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, actionName, null, isAjax, funcName);
        }

        public static string Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, object values, bool isAjax, string funcName)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, new RouteValueDictionary(values), isAjax, funcName);
        }

        public static string Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string actionName, object values, bool isAjax, string funcName)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, actionName, new RouteValueDictionary(values), isAjax, funcName);
        }

        public static string Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary, bool isAjax, string funcName)
        {
            return Pager(htmlHelper, pageSize, currentPage, totalItemCount, null, valuesDictionary, isAjax, funcName);
        }

        public static string Pager(this HtmlHelper htmlHelper, int pageSize, int currentPage, int totalItemCount, string actionName, RouteValueDictionary valuesDictionary, bool isAjax, string funcName)
        {
            if (totalItemCount <= pageSize)
            {
                return "";
            }
            if (valuesDictionary == null)
            {
                valuesDictionary = new RouteValueDictionary();
            }
            if (actionName != null)
            {
                if (valuesDictionary.ContainsKey("action"))
                {
                    throw new ArgumentException("The valuesDictionary already contains an action.", "actionName");
                }
                valuesDictionary.Add("action", actionName);
            }
            var pager = new Pager(htmlHelper.ViewContext, pageSize, currentPage, totalItemCount, valuesDictionary, isAjax, funcName);
            return pager.RenderHtml();
        }
        #endregion
    }
}