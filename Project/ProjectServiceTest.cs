using Gisfpp_projects.Project.Services;
using Gisfpp_projects.Project.Model;
using Gisfpp_projects.Project.Model.Dto;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Gisfpp_projects.Shared;

namespace Gisfpp_projects_test.Project
{
    public class ProjectServiceTest
    {
        private readonly ProjectService _sut;

        public ProjectServiceTest()
        {
            // Creación del repo
            var repo = new ProjectMemoryRepository();

            // Creación del mapper
            IMapper mapper = new Mapper( new MapperConfiguration( cfg => {
                cfg.CreateMap<ProjectDTO, Gisfpp_projects.Project.Model.Project>();
                cfg.CreateMap<Gisfpp_projects.Project.Model.Project, ProjectDTO>();
            }) );

            _sut = new ProjectService(repo, mapper);
            
        }

        /* Template signature method test
            public void UnitOfWork_StateUnderTest_ExpectedBehavior(){}
        */
        [Fact]
        public void CreateProject_Valid_Success() {
            // Given
            var newProjectValid = new ProjectDTO();
            newProjectValid.Title = "Proyecto de ejemplo";
            newProjectValid.Type = TypeProject.COMPANY;
            newProjectValid.Start = DateTime.Today;
            newProjectValid.End = DateTime.Today.AddMonths(12);

            // When
            var projectCreated = _sut.CreateProject(newProjectValid);
            var projectRecovered = _sut.GetProjectById((int)projectCreated.Id!);


            // Then
            Assert.Equal(newProjectValid.Title, projectRecovered!.Title);
            Assert.Equal(newProjectValid.Type, projectRecovered.Type);
            Assert.Equal(StateProject.GENERATED, projectRecovered.State);
            Assert.Equal(newProjectValid.Start, projectRecovered.Start);
        }

        [Fact]
        public void CreateProject_WithoutTitle_Failure() {
            
            var newProjectWithoutTitle = new ProjectDTO();
            newProjectWithoutTitle.Title = "";
            newProjectWithoutTitle.Type = TypeProject.COMPANY;
            newProjectWithoutTitle.Start = DateTime.Today;
            newProjectWithoutTitle.End = DateTime.Today.AddMonths(12);

            try
            {
                _sut.CreateProject(newProjectWithoutTitle);
                Assert.True(false);
            }
            catch (ValidationException exc) {
                Assert.Equal(MessagesConstant.MSG_PROJECT_WITHOU_TITLE, exc.Message);                    
            }

        }

        [Fact]
        public void CreateProject_titleGreaterThan80_Failure()
        {
            var newProjectInvalid = new ProjectDTO();
            newProjectInvalid.Title = "Este proyecto inválido tiene un título mayor a 80 caracteres. XXXXXXXXXXXXXXXXXXXX";
            newProjectInvalid.Type = TypeProject.INTERN;
            newProjectInvalid.State = StateProject.GENERATED;
            newProjectInvalid.Start = DateTime.Today;
            newProjectInvalid.End = DateTime.Today.AddMonths(12);

            try
            {
                _sut.CreateProject(newProjectInvalid);
                Assert.True(false);
            }
            catch (ValidationException exc) 
            {
                Assert.Equal(MessagesConstant.MSG_TITLE_GREATER_80, exc.Message);                
            }            
        }

        [Fact]
        public void CreateProject_StartIsNull_Failure()
        {
            var newProjectInvalid = new ProjectDTO();
            newProjectInvalid.Title = "Este proyecto es inválido";
            newProjectInvalid.Type = TypeProject.INTERN;
            newProjectInvalid.State = StateProject.GENERATED;
            newProjectInvalid.Start = null;
            newProjectInvalid.End = DateTime.Today.AddMonths(12);

            try
            {
                _sut.CreateProject(newProjectInvalid);
                Assert.True(false);
            }
            catch (ValidationException exc)
            {
                Assert.Equal(MessagesConstant.MSG_STAR_IS_NULL, exc.Message);
            }
        }

        [Fact]
        public void CreateProject_EndIsNull_Failure()
        {
            var newProjectInvalid = new ProjectDTO();
            newProjectInvalid.Title = "Este proyecto es inválido";
            newProjectInvalid.Type = TypeProject.INTERN;
            newProjectInvalid.State = StateProject.GENERATED;
            newProjectInvalid.Start = DateTime.Today;
            newProjectInvalid.End = null;

            try
            {
                _sut.CreateProject(newProjectInvalid);
                Assert.True(false);
            }
            catch (ValidationException exc)
            {
                Assert.Equal(MessagesConstant.MSG_END_IS_NULL, exc.Message);
            }
        }

        [Fact]
        public void CreateProject_StartGreaterThanFinish_Failure() 
        {
            var newProjectInvalid = new ProjectDTO();
            newProjectInvalid.Title = "Este proyecto es inválido";
            newProjectInvalid.Type = TypeProject.INTERN;
            newProjectInvalid.State = StateProject.GENERATED;
            newProjectInvalid.Start = DateTime.Today;
            newProjectInvalid.End = DateTime.Today.AddMonths(-5);

            try
            {
                _sut.CreateProject(newProjectInvalid);
                Assert.True(false);
            }
            catch(ValidationException exc) 
            {
                Assert.Equal(MessagesConstant.MSG_START_GREATER_EQUEL_THAN_FINISH, exc.Message);   
            }            
        }

        [Fact]
        public void CreateProject_StartSameAsEnd_Failure()
        {
            var newProjectInvalid = new ProjectDTO();
            newProjectInvalid.Title = "Este proyecto es inválido";
            newProjectInvalid.Type = TypeProject.INTERN;
            newProjectInvalid.State = StateProject.GENERATED;
            newProjectInvalid.Start = DateTime.Today;
            newProjectInvalid.End = DateTime.Today;

            try 
            {
                _sut.CreateProject(newProjectInvalid);
                Assert.True(false);
            }
            catch (ValidationException exc) 
            {
                Assert.Equal(MessagesConstant.MSG_START_GREATER_EQUEL_THAN_FINISH, exc.Message);
            }
        }
    }
}
