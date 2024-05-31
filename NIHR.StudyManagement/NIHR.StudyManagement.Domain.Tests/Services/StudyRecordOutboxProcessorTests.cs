using Microsoft.Extensions.Logging;
using Moq;
using NIHR.StudyManagement.Domain.Abstractions;
using NIHR.StudyManagement.Domain.Constants;
using NIHR.StudyManagement.Domain.Models;
using NIHR.StudyManagement.Domain.Services;

namespace NIHR.StudyManagement.Domain.Tests.Services
{
    /// <summary>
    /// Unit test class for StudyRecordOutboxProcessor
    /// </summary>
    [TestClass]
    public class StudyRecordOutboxProcessorTests
    {
        Mock<IStudyRecordOutboxRepository> _studyRecordOutboxRepositoryMock;
        Mock<IStudyEventMessagePublisher> _studyEventMessagePublisherMock;
        Mock<ILogger<StudyRecordOutboxProcessor>> _loggerMock;
        StudyRecordOutboxProcessor _processorUnderTest;
        CancellationTokenSource _cancellationTokenSource;
        int _cancellationTokenSourceCancelAfter = 10000;
        StudyRecordOutboxItem _studyRecordOutboxItem;

        [TestMethod]
        public void ProcessorIsNotNull()
        {
            InitialiseProcessorAndAssert(() => {
                Assert.IsTrue(_processorUnderTest is not null);
            });
        }

        [TestMethod]
        public async Task ProcessAsync_TaskReturnsCompleted()
        {
            InitialiseProcessor();

            var processTask = _processorUnderTest.ProcessAsync(_cancellationTokenSource.Token);

            await processTask;

            Assert.IsTrue(processTask.IsCompletedSuccessfully);
        }

        [TestMethod]
        public async Task ProcessAsync_InvokesStudyRecordOutboxRepositoryMethodUpdateOutboxItemStatusAsync()
        {
            // Override setup defaults
            _studyRecordOutboxItem = new StudyRecordOutboxItem(1, "hello", "unit test", "testEvent", null, null, Constants.OutboxStatus.Created);

            _studyRecordOutboxRepositoryMock.SetupSequence(mock => mock.GetNextUnprocessedOutboxItem()).Returns(_studyRecordOutboxItem);

            InitialiseProcessor();

            var processTask = _processorUnderTest.ProcessAsync(_cancellationTokenSource.Token);

            await processTask;

            _studyRecordOutboxRepositoryMock.Verify(mock => mock.UpdateOutboxItemStatusAsync (_studyRecordOutboxItem.Id,
                    OutboxStatus.Processing,
                    It.IsAny<DateTime>(),
                    null)
            ,Times.Once);

            _studyRecordOutboxRepositoryMock.Verify(mock => mock.UpdateOutboxItemStatusAsync(_studyRecordOutboxItem.Id,
                OutboxStatus.CompletedSuccessfully,
                null,
                It.IsAny<DateTime>())
            , Times.Once);
        }

        [TestMethod]
        public async Task ProcessAsync_InvokesStudyRecordOutboxRepositoryMethodCommitAsync()
        {
            // Override setup defaults
            _studyRecordOutboxItem = new StudyRecordOutboxItem(1, "hello", "unit test", "testEvent", null, null, Constants.OutboxStatus.Created);

            _studyRecordOutboxRepositoryMock.SetupSequence(mock => mock.GetNextUnprocessedOutboxItem()).Returns(_studyRecordOutboxItem);

            InitialiseProcessor();

            var processTask = _processorUnderTest.ProcessAsync(_cancellationTokenSource.Token);

            await processTask;

            _studyRecordOutboxRepositoryMock.Verify(mock => mock.CommitAsync(), Times.AtLeast(2));
        }

        [TestMethod]
        public async Task ProcessAsync_InvokesStudyEventMessagePublisherMethodPublish()
        {
            // Override setup defaults
            _studyRecordOutboxItem = new StudyRecordOutboxItem(1, "hello", "unit test", "testEvent", null, null, Constants.OutboxStatus.Created);

            _studyRecordOutboxRepositoryMock.SetupSequence(mock => mock.GetNextUnprocessedOutboxItem()).Returns(_studyRecordOutboxItem);

            InitialiseProcessor();

            var processTask = _processorUnderTest.ProcessAsync(_cancellationTokenSource.Token);

            await processTask;

            _studyEventMessagePublisherMock.Verify(mock => mock.Publish(_studyRecordOutboxItem.Payload, It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            SetupMockDefaults();
        }

        private void InitialiseProcessorAndAssert(Action assert)
        {
            InitialiseProcessor();

            assert();
        }

        private void InitialiseProcessor()
        {
            _processorUnderTest = new StudyRecordOutboxProcessor(_studyRecordOutboxRepositoryMock.Object,
                _studyEventMessagePublisherMock.Object,
                _loggerMock.Object);
        }

        private void SetupMockDefaults()
        {
            _studyRecordOutboxRepositoryMock = new Mock<IStudyRecordOutboxRepository>();
            _studyEventMessagePublisherMock = new Mock<IStudyEventMessagePublisher>();
            _loggerMock = new Mock<ILogger<StudyRecordOutboxProcessor>>();

            _cancellationTokenSource = new CancellationTokenSource(_cancellationTokenSourceCancelAfter);

            _studyRecordOutboxItem = null;

            _studyRecordOutboxRepositoryMock.Setup(mock => mock.GetNextUnprocessedOutboxItem()).Returns(_studyRecordOutboxItem);
        }
    }
}
