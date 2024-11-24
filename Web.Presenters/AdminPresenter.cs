using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Web.Managers;
using Web.Managers.DbTables;
using Web.Presenters.IViews;

namespace Web.Presenters
{
    public class AdminPresenter(AdminModel adminModel, ProjectModel projectModel, SkillModel skillModel)
    {
        private readonly AdminModel _adminModel = adminModel;
        
        private readonly ProjectModel _projectModel = projectModel;

        private readonly SkillModel _skillModel = skillModel;

        public static string ComputeSHA256Hash(string rawData)
        {
            // 创建 SHA256 算法对象
            using (SHA256 sha256 = SHA256.Create())
            {
                // 将输入字符串转换为字节数组
                byte[] bytes = Encoding.UTF8.GetBytes(rawData);

                // 计算哈希值
                byte[] hashBytes = sha256.ComputeHash(bytes);

                // 将字节数组转换为十六进制字符串
                StringBuilder builder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    builder.Append(b.ToString("x2")); // 转为小写十六进制
                }
                return builder.ToString();
            }
        }

        public async Task<bool> CheackLoginAsync(string password)
        {
            var adm = await GetAdminAsync();
            if (adm != null && !string.IsNullOrEmpty(adm.Password))
            {
                return ComputeSHA256Hash(password) == adm.Password;
            };
            return true;
        }

        public async Task<string> GetSkillStringAsync()
        {
            return await _skillModel.GetAllSKillStringAsync();
        }


        public async Task<List<SkillData>> GetSkillDatasAsync()
        {
            return (await _skillModel.GetAllSKill()).Select(p => new SkillData
            {
                Id = p.Id.ToHex(),
                Name = p.Name ?? "",
                Level = p.Level ?? ""
            }).ToList();
        }
        public async Task UpdateSkill(string[] skills)
        {
            await _skillModel.RemoveAllAsync();
            foreach (var item in skills)
            {
                var skill = item.Split('-');
                if (skill.Length >= 2)
                    await _skillModel.AddSKillAsync(skill[0], skill[1]);
            }
        }

        public async Task<AdminTable?> GetAdminAsync()
        {
            return await _adminModel.GetAdminAsync();
        }

        public async Task UpdateAdminAsync(AdminTable admin)
        {
            await _adminModel.UpdateAdminAsync(admin);
        }

        public async Task<List<ProjectData>> GetProjectDatasAsync()
        {
            return (await _projectModel.GetAllAsync())
                .Select(p => new ProjectData
                {
                    Id = p.Id.ToHex(),
                    Name = p.Name ?? "",
                    Type = p.Type ?? "",
                    Url = p.Url ?? ""
                })
                .ToList();
        }

        public async Task AddProjectAsync(ProjectData data)
        {
            await _projectModel.AddAsync(new()
            {
                Name = data.Name,
                Type = data.Type,
                Url = data.Url
            });
        }

        public async Task DeleteProjectAsync(ProjectData data)
        {
            await _projectModel.DeleteAsync(new()
            {
                Id = data.Id.ToID(),
            });
        }
    }
}
