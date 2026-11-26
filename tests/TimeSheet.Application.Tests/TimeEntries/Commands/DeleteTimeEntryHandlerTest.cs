using System.Runtime.InteropServices;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.TimeEntryRequests;
using TimeSheet.Application.TimeEntry.Commands.DeleteTimeEntry;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;
using TimeSheet.Domain.Enums;
using TimeSheet.Domain.Exceptions;

namespace TimeSheet.Application.Tests.TimeEntrys.Commands;



[TestFixture]
public class DeleteTimeEntryHandlerTests
{
    private DeleteTimeEntryTestContext context=null!;

    [SetUp]
    public void SetUp()
    {
            context=new DeleteTimeEntryTestContext();
    }
       
    [Test]
    public async Task Handle_ValidCommand()
    {
        context.WithTimeEntryExisting();
        await context.Handler.Handle(context.DeleteValidCommand(), CancellationToken.None);

        context.TimeEntryRepositoryMock.Verify(r=>r.DeleteAsync(context.timeEntryId), Times.Once);

    }

    [Test]
    public void Handle_NotOwner()
    {
        context.WithTimeEntryExisting()
               .WithDifferentUser();

        Assert.ThrowsAsync<ForbiddenException>(() =>
            context.Handler.Handle(context.DeleteValidCommand(), CancellationToken.None));

        context.VerifyTimeEntryNotDeleted();
    }

    [Test]
    public async Task Handle_TimeEntryNotFound()
    {
        context.WithTimeEntryNotFound();

        Assert.ThrowsAsync<NotFoundException>(()=>
            context.Handler.Handle(context.DeleteValidCommand(), CancellationToken.None)
        );

        context.VerifyTimeEntryNotDeleted();

    }
}
public class DeleteTimeEntryTestContext
{   
    public Guid memberId{get; }=Guid.NewGuid();
    public Guid timeEntryId{get; }=Guid.NewGuid();
    public Mock<ITimeEntryRepository> TimeEntryRepositoryMock { get; } = new();
    public Mock<ICurrentUserService> CurrentUserServiceMock {get; }= new();
    public DeleteTimeEntryHandler Handler{get; }
    public DeleteTimeEntryTestContext()
    {
        CurrentUserServiceMock
                .Setup(s=> s.UserId)
                .Returns(memberId);
        Handler = new DeleteTimeEntryHandler(TimeEntryRepositoryMock.Object, CurrentUserServiceMock.Object);
    }
    public DeleteTimeEntryCommand DeleteValidCommand()
    {   
        return new DeleteTimeEntryCommand(timeEntryId);
    }

  
    public DeleteTimeEntryTestContext WithTimeEntryExisting()
    {
        TimeEntryRepositoryMock
            .Setup(r => r.GetByIdAsync(timeEntryId))
            .ReturnsAsync(new TimeEntryModel(timeEntryId, new DateOnly(2026, 11, 25), 
             7.5, 0.5, "Opis",
             project: Guid.NewGuid(),
             category: Guid.NewGuid(),
             teammember: memberId));

        return this;
    }

    public DeleteTimeEntryTestContext WithDifferentUser()
    {
        CurrentUserServiceMock
            .Setup(s => s.UserId)
            .Returns(Guid.NewGuid());
        return this;
    }

    public DeleteTimeEntryTestContext WithTimeEntryNotFound()
    {
        TimeEntryRepositoryMock
            .Setup(r => r.GetByIdAsync(timeEntryId))
            .ReturnsAsync((TimeEntryModel?)null);
        return this;
    }

    public void VerifyTimeEntryNotDeleted()
    {
        TimeEntryRepositoryMock.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
    }

}