#### 目录
1. [一.UI视图管理器的类图设计](#ui_design)
2. [二.具体代码实现](#ui_imp)
    1. [2.1 ViewManager类](#ViewManager)
    2. [2.2 ViewDefiner类](#ViewDefiner)
    3. [2.3 IViewPresenter接口](#IViewPresenter)
<br>

#### 一.UI视图管理器的类图设计 {#ui_design}
<img style="width:100%;" src="images/ui1.svg">
<br>
由上图，我们将进行以下操作：<br>
1.所有UI视图通过装载继承了ViewDefiner的具体脚本在OnInitialize中绑定与其对应的IViewPresent的具体实现<br>
2.在IViewPresent的具体实现中，依赖一个或多个类作为Model存在<br>
3.在OnInitialize中将他们添加到ViewManager<br>
一切准备就绪后，通过全局单例的ViewManager我们可以在任意地点对UI进行控制，且避免了对UI元素的直接操控。<br>
<br>
注意：<br>
1.ViewPresent只负责Model与UI视图的交互，尽量不要在当中写入复杂逻辑<br>
2.Model作为具体的逻辑层存在<br>
3.本方案属于MVP模式，若有想法或不习惯这种形式的可以修改成MVVM或MVC或者其他方案进行尝试<br>
<br>

#### 二.具体代码实现 {#ui_imp}
##### 1.ViewManager类 {#ViewManager}
``` C#
public class ViewManager : Singleton<ViewManager>
{
    private readonly Dictionary<string, IViewPresenter> _uiVModelById = new Dictionary<string, IViewPresenter>();

    private readonly Dictionary<string, List<IViewPresenter>> _uiVModelByClass = new Dictionary<string, List<IViewPresenter>>();

    /// <summary>
    /// 添加定义器
    /// </summary>
    public void AddDefiner<T>(ViewDefiner<T> definer) where T : IViewPresenter, new()
    {
        if (!string.IsNullOrEmpty(definer.Id))
        {
            if (_uiVModelById.ContainsKey(definer.Id))
            {
                GameLogManager.Instance.Error($"添加UIDefiner时错误 ID:[{definer.Id}]已被定义");
                return;
            }
            _uiVModelById.Add(definer.Id, definer.Presenter);
        }

        if (!string.IsNullOrEmpty(definer.Class))
        {
            if (!_uiVModelByClass.TryGetValue(definer.Class, out var list))
            {
                list = new List<IViewPresenter>();
                _uiVModelByClass[definer.Class] = list;
            }
            list.Add(definer.Presenter);
        }
    }

    /// <summary>
    /// 移除定义器
    /// </summary>
    public void RemoveDefiner<T>(ViewDefiner<T> definer) where T : IViewPresenter, new()
    {
        if (!string.IsNullOrEmpty(definer.Id))
            _uiVModelById.Remove(definer.Id);

        if (!string.IsNullOrEmpty(definer.Class) && _uiVModelByClass.TryGetValue(definer.Class, out var list))
        {
            list.Remove(definer.Presenter);
            if (list.Count == 0) _uiVModelByClass.Remove(definer.Class);
        }
    }

    /// <summary>
    /// 按Id查找
    /// </summary>
    public T FindById<T>(string id) where T : IViewPresenter
    {
        return _uiVModelById.TryGetValue(id, out var vm) && vm is T result ?
            result : default;
    }

    /// <summary>
    /// 按Class查找UI
    /// </summary>
    public IEnumerable<T> FindByClass<T>(string className) where T : IViewPresenter
    {
        if (_uiVModelByClass.TryGetValue(className, out var vms))
        {
            foreach (var vm in vms)
            {
                if (vm is T result)
                    yield return result;
            }
        }
    }
}
```
<br>

##### 2.ViewDefiner类 {#ViewDefiner}
``` C#
 public abstract class ViewDefiner<T> : MonoBehaviour where T : IViewPresenter, new()
 {
     public string Id;

     public string Class;

     public T Presenter;

     protected abstract void OnInitialize();

     private void Start()
     {
         Presenter = new T();
         Presenter.InitPresenter(gameObject);
         ViewManager.Instance.AddDefiner(this);
         OnInitialize();
     }

     private void OnDestroy()
     {
         ViewManager.Instance.RemoveDefiner(this);
     }
 }
```
<br>

##### 3.IViewPresenter接口 {#IViewPresenter}
``` C#
public interface IViewPresenter
{
    void InitPresenter(GameObject view);
}
```