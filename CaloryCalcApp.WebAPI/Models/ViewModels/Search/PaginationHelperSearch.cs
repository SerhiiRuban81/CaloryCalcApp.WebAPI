using CaloryCalcApp.WebAPI.Data;
using CaloryCalcApp.WebAPI.Models.DTOs.DishProducts;
using CaloryCalcApp.WebAPI.Models.DTOs.Products;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CaloryCalcApp.WebAPI.Models.ViewModels.Search
{
    public class PaginationHelperSearch
    {
        public static string RenderPagination<T>(
            List<T> list, // Full list of objects which we will use
            string caption1, // Caption of the first column in the table
            string caption2, // Caption of the second column in the table
            Func<T, string> valueSelector1,
            Func<T, string> valueSelector2,
            int currentPage,
            int pageSize,
            string searchText,
            string searchType,
            string pageParameterName
            )
        {
            int totalCount = list.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Let's get only items for the current page
            var pagedList = list
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var sb = new StringBuilder();            

            // Table header
            sb.AppendLine("<table class='table table-hover table-stripped align-middle shadow-sm rounded-3 overflow-hidden'>");
            sb.AppendLine("<thead><tr>");
            sb.AppendLine($"<th>{caption1}</th>");
            sb.AppendLine($"<th>{caption2}</th>");
            sb.AppendLine("</tr></thead>");
            sb.AppendLine("<tbody>");

            // Loop throug items
            foreach (var item in pagedList)
            {
                var val1 = valueSelector1(item);
                var val2 = valueSelector2(item); // buttons HTML

                sb.AppendLine("<tr>");
                sb.AppendLine($"<td>{val1}</td>");
                sb.AppendLine($"<td>{val2}</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</tbody></table>");

            // Pagination controls
            sb.AppendLine("<nav>");
            sb.AppendLine("<ul class='pagination'>");
            for (int i = 1; i <= totalPages; i++)
            {
                var activeClass = i == currentPage ? "active" : "";
                sb.AppendLine($"<li class='page-item {activeClass}'>");
                sb.AppendLine($"<a class='page-link' href='?searchText={searchText}&searchType={searchType}&{pageParameterName}={i}'>{i}</a>");
                sb.AppendLine("</li>");
            }
            sb.AppendLine("</ul>");
            sb.AppendLine("</nav>");

            return sb.ToString();
        }

    }
}
