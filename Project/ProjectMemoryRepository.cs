using Gisfpp_projects.Project.Repositories;
using System.Collections.Generic;


namespace Gisfpp_projects_test.Project
{
    internal class ProjectMemoryRepository : IProjectRepository
    {
        private List<Gisfpp_projects.Project.Model.Project> _dbProjects;

        public ProjectMemoryRepository()
        {
            _dbProjects = new List<Gisfpp_projects.Project.Model.Project>();
        }

        public int Create(Gisfpp_projects.Project.Model.Project entity)
        {
            int id = new Random().Next();
            entity.Id = id;
            _dbProjects.Add(entity);
            return id;
        }

        public void Delete(Gisfpp_projects.Project.Model.Project entity)
        {
            throw new NotImplementedException();
        }

        public Gisfpp_projects.Project.Model.Project? FindById(int id)
        {
            try
            {
                return _dbProjects.Find(x => x.Id == id);
            }
            catch (Exception)
            {
                return null;
            }            
        }

        public IEnumerable<Gisfpp_projects.Project.Model.Project> GetAll()
        {
            return _dbProjects;
        }

        public void Update(Gisfpp_projects.Project.Model.Project entity)
        {
            throw new NotImplementedException();
        }
    }
}
