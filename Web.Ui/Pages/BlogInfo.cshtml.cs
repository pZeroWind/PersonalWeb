using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Presenters;
using Web.Ui.ViewModel;

namespace Web.Ui.Pages
{
    public class BlogInfoModel(BlogInfoPresenter presenter) : PageModel
    {
        private readonly BlogInfoPresenter _presenter = presenter;

        public BlogInfoVModel VModel { get; private set; } = new();

        public async Task OnGetAsync([FromRoute]string id)
        {
            await _presenter.SetView<BlogInfoPresenter>(VModel)
                .GetBlogAsync(id);
        }
    }
}
