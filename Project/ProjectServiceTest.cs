using Gisfpp_projects.Project.Services;
using Gisfpp_projects.Project.Model;
using Gisfpp_projects.Project.Model.Dto;

namespace Gisfpp_projects_test.Project
{
    public class ProjectServiceTest
    {
        private readonly ProjectService _sut;

        public ProjectServiceTest()
        {
            this._sut = new ProjectService();
        }

        /* Template signature method test
            public void UnitOfWork_StateUnderTest_ExpectedBehavior(){}
        */
        [Fact]
        public void CreateProject_Valid_Success() {
            // Given
            var newProjectValid = new ProjectDTO("Proyecto de ejemplo", 
                                                    TypeProject.COMPANY,
                                                    StateProject.GENERATED,
                                                    DateTime.Today,
                                                    DateTime.Today.AddMonths(12));
            
            // When
            var projectCreated = _sut.CreateProject(newProjectValid);
            var projectRecovered = _sut.GetProjectById(projectCreated.Id);


            // Then
            Assert.Equal(newProjectValid.Title, projectRecovered.Title);
            Assert.Equal(newProjectValid.Type, projectRecovered.Type);
            Assert.Equal(newProjectValid.State, projectRecovered.State);
            Assert.Equal(newProjectValid.Start, projectRecovered.Start);
        }
        [Fact]
        public void CreateProject_WithoutTitle_Failure() {
            var newProjectWithoutTitle = new ProjectDTO("", 
                                                        TypeProject.INTERN,
                                                        StateProject.GENERATED,
                                                        DateTime.Today,
                                                        DateTime.Today.AddMonths(12));

            try
            {
                _sut.CreateProject(newProjectWithoutTitle);
                Assert.True(false);
            }
            catch (ArgumentException exc) {
                Assert.Equal(exc.Message, ProjectService.MSG_PROJECT_WITHOU_TITLE);                    
            }

        }

        [Fact]
        public void CreateProject_titleGreaterThan80_Failure()
        {
            var newProjectInvalid = new ProjectDTO("Este proyecto inválido tiene un título mayor a 80 caracteres. XXXXXXXXXXXXXXXXXXXX",
                                                    TypeProject.INTERN,
                                                    StateProject.GENERATED,
                                                    DateTime.Today,
                                                    DateTime.Today.AddMonths(12));

            try
            {
                _sut.CreateProject(newProjectInvalid);
                Assert.True(false);
            }
            catch (ArgumentException exc) 
            {
                Assert.Equal(exc.Message, ProjectService.MSG_TITLE_GREATER_80);   
            }            
        }

        [Fact]
        public void CreateProject_StatusOtherThanGenerated_Failure() 
        {
            var newProjectInvalid = new ProjectDTO("State other than generated",
                                                    TypeProject.INTERN,
                                                    StateProject.SUSPENDED,
                                                    DateTime.Today,
                                                    DateTime.Today.AddMonths(12));
            try
            {
                _sut.CreateProject(newProjectInvalid);
                Assert.True(!false);
            }
            catch (ArgumentException exc) 
            {
                Assert.Equal(exc.Message, ProjectService.MSG_STATE_DISTINCT_GENERATED);    
            }
        }

        [Fact]
        public void CreateProject_StartIsNull_Failure()
        {
            var newProjectInvalid = new ProjectDTO("Start greater than finish",
                                                    TypeProject.INTERN,
                                                    StateProject.GENERATED,
                                                    null,
                                                    null);
            try
            {
                _sut.CreateProject(newProjectInvalid);
                Assert.True(false);
            }
            catch (ArgumentException exc)
            {
                Assert.Equal(exc.Message, ProjectService.MSG_STAR_IS_NULL);
            }
        }

        [Fact]
        public void CreateProject_EndIsNull_Failure()
        {
            var newProjectInvalid = new ProjectDTO("Start greater than finish",
                                                    TypeProject.INTERN,
                                                    StateProject.GENERATED,
                                                    DateTime.Today,
                                                    null);
            try
            {
                _sut.CreateProject(newProjectInvalid);
                Assert.True(false);
            }
            catch (ArgumentException exc)
            {
                Assert.Equal(exc.Message, ProjectService.MSG_END_IS_NULL);
            }
        }

        [Fact]
        public void CreateProject_StartGreaterThanFinish_Failure() 
        {
            var newProjectInvalid = new ProjectDTO("Start greater than finish",
                                                    TypeProject.INTERN,
                                                    StateProject.GENERATED,
                                                    DateTime.Today, 
                                                    DateTime.Today.AddMonths(-5));
            

            try
            {
                _sut.CreateProject(newProjectInvalid);
                Assert.True(false);
            }
            catch(ArgumentException exc) 
            {
                Assert.Equal(exc.Message, ProjectService.MSG_START_GREATER_EQUEL_THAN_FINISH);   
            }            
        }

        [Fact]
        public void CreateProject_StartSameAsEnd_Failure()
        {
            var newProjectInvalid = new ProjectDTO("Start greater than finish",
                                                    TypeProject.INTERN,
                                                    StateProject.GENERATED,
                                                    DateTime.Today,
                                                    DateTime.Today);
            try 
            {
                _sut.CreateProject(newProjectInvalid);
                Assert.True(false);
            }
            catch (ArgumentException exc) 
            {
                Assert.Equal(exc.Message, ProjectService.MSG_START_GREATER_EQUEL_THAN_FINISH);
            }
        }
    }
}
