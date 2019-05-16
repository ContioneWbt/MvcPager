using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PagerCheck
{
    public class Pager
    {
        private ViewContext viewContext;
        private readonly int pageSize;
        private readonly int currentPage;
        private readonly int totalItemCount;
        private readonly RouteValueDictionary linkWithoutPageValuesDictionary;
        private readonly bool m_IsAjax;
        private readonly string functionName;

        public Pager(ViewContext viewContext, int pageSize, int currentPage, int totalItemCount, RouteValueDictionary valuesDictionary, bool isAjax, string funcName)
        {
            this.viewContext = viewContext;
            this.pageSize = pageSize;
            this.currentPage = currentPage;
            this.totalItemCount = totalItemCount;
            this.linkWithoutPageValuesDictionary = valuesDictionary;
            m_IsAjax = isAjax;
            functionName = funcName;
        }

        public string RenderHtml()
        {
            int pageCount = (int)Math.Ceiling(this.totalItemCount / (double)this.pageSize);
            //连续显示的页块数
            int nrOfPagesToDisplay = 5;

            var sb = new StringBuilder();

            // Previous
            //sb.Append("<ul>");
            //if (this.currentPage > 1)
            //{
            //    sb.Append("<div class=\"pageL\" style=\"float:left\">").Append(GeneratePageLink("上一页", this.currentPage - 1)).Append("</div>");
            //}
            //else
            //{
            //    sb.Append("<div class=\"pageL\" style=\"float:left\"> 上一页</div>");
            //}

            //分页起始页          
            int start = 1;
            int end = pageCount;

            if (pageCount > nrOfPagesToDisplay)
            {
                int middle = (int)Math.Ceiling(nrOfPagesToDisplay / 2d) - 1;
                int below = (this.currentPage - middle);
                int above = (this.currentPage + middle);

                if (below <= 2)
                {
                    above = (pageCount - nrOfPagesToDisplay) > 2 ? nrOfPagesToDisplay : pageCount;
                    below = 1;
                }
                else if (above >= (pageCount - 2))
                {
                    above = pageCount;
                    below = (pageCount - nrOfPagesToDisplay) > 2 ? (pageCount - nrOfPagesToDisplay) : 1;
                }

                start = below;
                end = above;
            }

            if (start >= 3)
            {
                //从第三页开始 就显示第一页
                sb.Append(GeneratePageLink("1", 1));
                //sb.Append("<div class=\"pageR\">").Append(GeneratePageLink("2", 2)).Append("</div>");
                sb.Append("<a> ...</a>");
            }
            for (int i = start; i <= end; i++)
            {
                if (i == this.currentPage)
                {
                    sb.Append($"<span class='layui-laypage-curr'><em class='layui-laypage-em'></em><em>{i}</em></span>");
                    //sb.Append("<li class=\"pageactive\" >").Append("<a href=\"javascript:;\">" + i + "</a>").Append("</li>");
                }
                else
                {
                    //sb.Append("<li>").Append(GeneratePageLink(i.ToString(), i)).Append("</li>");
                    sb.Append(GeneratePageLink(i.ToString(), i));
                }
            }
            if (end < (pageCount - 2))
            {
                //最后一页
                sb.Append("<a> ...</a>");
                //sb.Append("<div class=\"pageR\">").Append(GeneratePageLink((pageCount - 1).ToString(), pageCount - 1)).Append("</div>");
                sb.Append(GeneratePageLink(pageCount.ToString(), pageCount));
            }
            int valuePage = currentPage == pageCount ? 1 : currentPage + 1 > pageCount ? pageCount : currentPage + 1;
            int currentCount = pageCount - currentPage == 0 ? (totalItemCount - pageSize * (currentPage - 1)) : pageSize;
            //屏蔽跳转第几页
            //sb.Append("<li class=\"jump\"><span>跳转</span><input type=\"text\" class=\"inp_page\" value=\"" + valuePage + "\"><span>页</span></li>");
            //sb.Append("<li><input type=\"button\" value=\"确定\" class=\"inpbtns linkbtn\" ></li>");
            //sb.Append("</ul>");
            //sb.Append("<div class=\"toto\">共<span class=\"blue\">" + totalItemCount + "</span>条信息，本页展示<span class=\"blue\">" + currentCount + "</span>条</div>");
            sb.Append($@"<span class='layui-laypage-count'>共 {totalItemCount} 条信息，本页展示{currentCount}条</span>");
            sb.Append("<a  class='layui-laypage-refresh' title=\"刷新\" onclick=\"" + functionName + "(" + currentPage + ")\"><i class='layui-icon layui-icon-refresh'></i></a>");
            //sb.Append("<a class=\"reload\" title=\"刷新\" onclick=\"" + functionName + "(" + currentPage + ")\"></a>");
            // sb.Append("<script type=\"text/javascript\">" + "$(function () {$(\".linkbtn\").click(function () {").Append(functionName + "($(\".inp_page\").val());})})").Append("<\\/script>");
            // Next
            //if (this.currentPage < pageCount)
            //{
            //    sb.Append("<div class=\"pageL\"  style=\" float:left\">").Append(GeneratePageLink("下一页", (this.currentPage + 1))).Append("</div>");
            //}
            //else
            //{
            //    sb.Append("<div class=\"pageL\" style=\" float:left\">下一页</div>");
            //}
            //sb.AppendFormat("<div class=\"pagebottom\">共<span>{0}</span>页-<span>{1}</span>条记录</div>", pageCount, this.totalItemCount);
            //sb.Append("</ul>");
            return sb.ToString();
        }

        private string GeneratePageLink(string linkText, int pageNumber)
        {
            var pageLinkValueDictionary = new RouteValueDictionary(this.linkWithoutPageValuesDictionary);
            pageLinkValueDictionary.Add("page", pageNumber);
            //string areaName = string.Empty;
            //string actionName = string.Empty;
            //string controllerName = string.Empty;
            //if (viewContext.RequestContext.RouteData.DataTokens.ContainsKey("area"))
            //{
            //    areaName = viewContext.RequestContext.RouteData.DataTokens["area"].ToString();
            //}
            //controllerName = viewContext.RequestContext.RouteData.GetRequiredString("controller");
            //actionName = viewContext.RequestContext.RouteData.GetRequiredString("action");

            string url = string.Empty;
            //if (areaName == string.Empty)
            //{
            //    url = string.Format("/{0}/{1}", controllerName, actionName);
            //}
            //else
            //    url = string.Format("/{0}/{1}/{2}", areaName, controllerName, actionName);

            //var virtualPathData = RouteTable.Routes.GetVirtualPathForArea(viewContext.RequestContext, pageLinkValueDictionary);
            //string url = virtualPathData.VirtualPath;
            //if (url.IndexOf(areaName) != 1)
            //{
            //    url = string.Format("/{0}{1}", areaName, url.Substring(url.IndexOf('/', 2)));
            //}

            var routes = RouteTable.Routes;
            var requestContext = viewContext.RequestContext;
            var routeData = requestContext.RouteData;
            var dataTokens = routeData.DataTokens;
            if (dataTokens["area"] == null)
                dataTokens.Add("area", "");
            var virtualPathData = routes.GetVirtualPathForArea(requestContext, routeData.Values);
            if (virtualPathData != null)
            {
                var virtualPath = virtualPathData.VirtualPath.ToLower();
                var request = requestContext.HttpContext.Request;
                string queryString = request.Url.Query;
                if (string.IsNullOrEmpty(queryString))
                {
                    queryString = queryString + "?page=" + pageNumber.ToString();
                }
                else if (queryString.ToLower().IndexOf("page=") > 0)
                {
                    int i = queryString.ToLower().IndexOf("page=");
                    queryString = queryString.Substring(0, i + 5) + pageNumber.ToString() + queryString.Substring(i + 6);
                }
                else
                {
                    queryString += "&page" + pageNumber.ToString();
                }
                url = virtualPath + queryString;
            }

            if (virtualPathData != null)
            {
                if (!m_IsAjax)
                {
                    string linkFormat = "<a href=\"{0}\">{1}</a>";
                    return string.Format(linkFormat, url, linkText);
                    //return String.Format(linkFormat, virtualPathData.VirtualPath, linkText);
                }
                else
                {
                    string linkFormat = "<a href=\"javascript:{1}({2})\">{0}</a>";
                    return String.Format(linkFormat, linkText, functionName, pageNumber);
                }
            }
            else
            {
                return null;
            }
        }
    }
}