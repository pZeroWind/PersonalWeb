using Web.Models;

namespace Web.Presenters
{
    public class BasePresenter<T> where T : IModel, new() 
    {
        protected T Model { get; private set; }

        public BasePresenter() 
        {
            Model = new T();
        }
    }
}
