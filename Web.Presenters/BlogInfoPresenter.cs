using Markdig;
using System.Text.RegularExpressions;
using Web.Managers;
using Web.Managers.ComponentModel;
using Web.Presenters.IViews;

namespace Web.Presenters
{
    public class BlogInfoPresenter(BlogModel model) : BasePresenter<BlogModel, IBlogInfoView>(model)
    {
        public async Task GetBlogAsync(string id)
        {
            if (View == null) return;
            var blog = await Model.GetAsync(id.ToID());

            if (blog.Content != null)
            {
                var pipeline = new MarkdownPipelineBuilder()
                                    .UseAdvancedExtensions() // 启用高级扩展
                                    .Build();
                var htmlContent = Markdown.ToHtml(blog.Content, pipeline);

                string pattern = @"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>";
                // 替换 src 属性的值
                blog.Content = Regex.Replace(htmlContent, pattern, match =>
                {
                    // 获取匹配到的 src 属性的值
                    string oldSrc = match.Groups[1].Value;

                    // 仅替换相对路径（避免替换外部 URL）
                    if (!oldSrc.StartsWith("http"))
                    {
                        string newSrc = $"/{blog.FileName}/{oldSrc}";
                        return $"<div class=\"image-box\">{match.Value.Replace(oldSrc, newSrc)}</div>";  // 返回替换后的 img 标签
                    }

                    return match.Value;  // 如果是外部 URL，不做替换
                });
            }
            View.Data = blog;
        }
    }
}
