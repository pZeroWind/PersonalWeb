using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Web.Presenters;
using Web.Ui.ViewModel;

namespace Web.Ui.Pages
{
    public class BlogsModel(ILogger<BlogsModel> logger, BlogPresenter presenter) : PageModel
    {
        private readonly ILogger<BlogsModel> _logger = logger;

        private readonly BlogPresenter _presenter = presenter;

        public BlogsVModel ViewModel { get; private set; } = new();

        public async Task OnGetAsync([FromQuery] int page = 1, [FromQuery] string search = "")
        {
            ViewModel.Search = search;
            ViewModel.PageIndex = page;
            ViewModel.PageSize = 5;
            await _presenter.SetView<BlogPresenter>(ViewModel)
                .GetBlogsAsync();
        }

        public async Task<IActionResult> OnPostNextPageIndexAsync([FromBody] BlogsVModel model)
        {
            await _presenter.SetView<BlogPresenter>(model)
                .NextPageIndexAsync();
            ViewModel = model;
            return Partial("_BlogsView", model);
        }

        public async Task<IActionResult> OnPostReloadAsync([FromBody] BlogsVModel model)
        {
            await _presenter.SetView<BlogPresenter>(model)
                .GetBlogsAsync();
            ViewModel = model;
            return Partial("_BlogsView", model);
        }

        public async Task<IActionResult> OnPostPrevPageIndexAsync([FromBody] BlogsVModel model)
        {
            await _presenter.SetView<BlogPresenter>(model)
                .PrevPageIndexAysnc();
            ViewModel = model;
            return Partial("_BlogsView", model);
        }
    }

}
