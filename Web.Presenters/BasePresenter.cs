namespace Web.Presenters
{
    public class BasePresenter<T, S>(T model)
        where T : class 
        where S : class
    {
        protected T Model => model;

        protected S? View {  get; private set; }

        public TR SetView<TR>(S view) where TR : BasePresenter<T, S>
        {
            View = view;
            return (TR)this;
        }
    }
}
