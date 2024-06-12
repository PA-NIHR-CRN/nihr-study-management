using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Configuration;
using NIHR.StudyManagement.Domain.Constants;
using NIHR.StudyManagement.Domain.Models;
using NIHR.StudyManagement.Domain.Services;

namespace NIHR.StudyManagement.Domain.Tests.Services
{
    /// <summary>
    /// Unit test class for StudyRecordOutboxProcessor
    /// </summary>
    [TestClass]
    public class GovernmentResearchIdentifierServiceTests
    {
        Mock<IStudyRegistryRepository> _studyRegistryRepositoryMock;
        Mock<IStudyRecordOutboxRepository> _studyRecordOutboxRepositoryMock;
        Mock<IStudyEventMessagePublisher> _studyEventMessagePublisherMock;
        Mock<IUnitOfWork> _unitOfWorkMock;
        Mock<IFhirMapper> _fhirMapperMock;
        Mock<IOptions<StudyManagementSettings>> _settingsMock;

        GovernmentResearchIdentifierService _serviceUnderTest;
        RegisterStudyRequest _registerStudyRequest;
        StudyManagementSettings _settingValues;

        const string DEFAULT_SYSTEM_NAME = "unittest";
        const string DEFAULT_ROLE_NAME = "unittester";

        [TestMethod]
        public async Task RegiserStudyAsync_TaskReturnsCompleted()
        {
            InitialiseService();

            var processTask = _serviceUnderTest.RegisterStudyAsync(_registerStudyRequest);

            await processTask;

            Assert.IsTrue(processTask.IsCompletedSuccessfully);
        }

        [TestMethod]
        public async Task RegiserStudyAsync_InvokesUnitOfWorkCommitAsync()
        {
            InitialiseService();

            var processTask = _serviceUnderTest.RegisterStudyAsync(_registerStudyRequest);

            await processTask;

            _unitOfWorkMock.Verify(mock => mock.CommitAsync(), Times.Once);
        }


        [TestMethod]
        public async Task RegiserStudyAsync_InvokesStudyRecordOutboxRepositoryMethodAddToOutbox()
        {
            _fhirMapperMock.Setup(mock => mock.MapToResearchStudyBundleAsJson(It.IsAny<GovernmentResearchIdentifier>()))
                .Returns("abc");

            InitialiseService();

            var processTask = _serviceUnderTest.RegisterStudyAsync(_registerStudyRequest);

            await processTask;

            _studyRecordOutboxRepositoryMock.Verify(mock => mock.AddToOutboxAsync(
                It.Is<AddToOuxboxRequest>(p =>
                p.EventType == GrisNsipEventTypes.StudyRegistered
                && p.Payload == "abc"
                && p.SourceSystem == DEFAULT_SYSTEM_NAME), It.IsAny<CancellationToken>()));
        }

        [TestInitialize]
        public void TestInitialize()
        {
            SetupMockDefaults();
        }

        private void InitialiseProcessorAndAssert(Action assert)
        {
            InitialiseService();

            assert();
        }

        private void InitialiseService()
        {
            _serviceUnderTest = new GovernmentResearchIdentifierService(_studyRegistryRepositoryMock.Object,
                _settingsMock.Object,
                _studyEventMessagePublisherMock.Object,
                _unitOfWorkMock.Object,
                _fhirMapperMock.Object);
        }

        private void SetupMockDefaults()
        {
            _studyRegistryRepositoryMock = new Mock<IStudyRegistryRepository>();
            _studyRecordOutboxRepositoryMock = new Mock<IStudyRecordOutboxRepository>();
            _studyEventMessagePublisherMock = new Mock<IStudyEventMessagePublisher>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _fhirMapperMock = new Mock<IFhirMapper>();
            _settingsMock = new Mock<IOptions<StudyManagementSettings>>();

            _registerStudyRequest = new RegisterStudyRequest();

            _settingValues = new StudyManagementSettings() { DefaultRoleName = DEFAULT_ROLE_NAME, DefaultLocalSystemName = DEFAULT_SYSTEM_NAME };

            _settingsMock.SetupGet(mock => mock.Value).Returns(_settingValues);

            _unitOfWorkMock.Setup(mock => mock.StudyRecordOutboxRepository).Returns(_studyRecordOutboxRepositoryMock.Object);
            _unitOfWorkMock.Setup(mock => mock.StudyRegistryRepository).Returns(_studyRegistryRepositoryMock.Object);
        }
    }
}
