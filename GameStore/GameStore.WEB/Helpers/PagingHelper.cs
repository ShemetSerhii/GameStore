using GameStore.WEB.Models.PaginationModel;
using System;
using System.Text;
using System.Web.Mvc;

namespace GameStore.WEB.Helpers
{
    public static class PagingHelper
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PageInfo pageInfo, Func<int, string> pageUrl)
        {
            var result = new StringBuilder();
            var tag = new TagBuilder("a");
            var middleFlag = false;

            if (pageInfo.PageNumber != decimal.One)
            {
                tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(1));
                tag.AddCssClass("link-btn start");
                result.Append(tag.ToString());

                tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(pageInfo.PageNumber - 1));
                tag.AddCssClass("link-btn previous");
                result.Append(tag.ToString());
            }

            var count = 1;

            if (pageInfo.PageNumber - 4 > 1)
            {
                count = pageInfo.PageNumber - 4;
            }

            for (var i = count; i <= pageInfo.TotalPages; i++)
            {
                if (middleFlag && i == pageInfo.PageNumber + 5) break;

                tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();

                if (i == pageInfo.PageNumber)
                {
                    tag.AddCssClass("selected");
                    middleFlag = true;
                }

                tag.AddCssClass("link-btn");
                result.Append(tag.ToString());
            }

            if (pageInfo.PageNumber != pageInfo.TotalPages)
            {
                tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(pageInfo.PageNumber + 1));
                tag.AddCssClass("link-btn next");
                result.Append(tag.ToString());

                tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(pageInfo.TotalPages));
                tag.AddCssClass("link-btn end");
                result.Append(tag.ToString());
            }

            return MvcHtmlString.Create(result.ToString());
        }
    }
}