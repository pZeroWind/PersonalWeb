using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Managers;
using Web.Presenters.IViews;

namespace Web.Presenters
{
    public class SkillPresenter(SkillModel model) : BasePresenter<SkillModel, IAboutView>(model)
    {
    }
}
