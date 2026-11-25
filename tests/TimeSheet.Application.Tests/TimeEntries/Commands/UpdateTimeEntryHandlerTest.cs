using System.Runtime.InteropServices;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Moq;
using Timesheet.Domain.Entities;
using TimeSheet.Application.Common.Exceptions;
using TimeSheet.Application.Requests.TimeEntryRequests;
using TimeSheet.Application.TimeEntry.Commands.UpdateTimeEntry;
using TimeSheet.Core.Repositories;
using TimeSheet.Core.Security;
using TimeSheet.Domain.Enums;
using TimeSheet.Domain.Exceptions;

namespace TimeSheet.Application.Tests.TimeEntrys.Commands;



[TestFixture]
public class UpdateTimeEntryHandlerTests
{
    private UpdateTimeEntryTestContext context=null!;

    [SetUp]
    public void SetUp()
    {
        context=new UpdateTimeEntryTestContext();
    }
       
    [Test]
    public async Task Handle_ValidCommand()
    {
        context.WithProjectExisting()
               .WithCategoryExisting()
               .WithTimeEntryExisting();
        
        await context.Handler.Handle(context.UpdateValidCommand(), CancellationToken.None);
        await context.Handler.Handle(context.UpdateValidCommand(), CancellationToken.None);

        var updated = context.UpdatedTimeEntry;
        Assert.That(updated, Is.Not.Null);
        Assert.That(updated!.Hours, Is.EqualTo(UpdateTimeEntryTestContext.Hours));
        Assert.That(updated.Description, Is.EqualTo(UpdateTimeEntryTestContext.NewDescription));
        Assert.That(updated.ProjectId, Is.EqualTo(context.newProjectId));
        Assert.That(updated.CategoryId, Is.EqualTo(context.newCategoryId));
        Assert.That(updated.TeamMemberId, Is.EqualTo(context.memberId));
           
    }

    [Test]
    public async Task Handle_TimeEntryNotFound()
    {
        context.WithTimeEntryNotFound()
               .WithCategoryExisting()
               .WithProjectExisting();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.UpdateValidCommand(),CancellationToken.None));    
        
        context.VerifyTimeEntryNotUpdated();
    }
   
    [Test]
    public async Task Handle_ProjectNotFound()
    {
        context.WithTimeEntryExisting()
               .WithCategoryExisting()
               .WithProjectNotFound();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.UpdateValidCommand(),CancellationToken.None));    
        
        context.VerifyTimeEntryNotUpdated();
    }

    [Test]
     public async Task Handle_CategoryNotFound()
    {
        context.WithProjectExisting()
               .WithCategoryNotFound()
               .WithTimeEntryExisting();

        Assert.ThrowsAsync<NotFoundException>(() =>
            context.Handler.Handle(context.UpdateValidCommand(),CancellationToken.None));    
        
        context.VerifyTimeEntryNotUpdated();
    }

    [Test]
     public async Task Handle_InvalidHours()
    {
        context.WithProjectExisting()
               .WithCategoryExisting()
               .WithTimeEntryExisting();

        Assert.ThrowsAsync<DomainException>(() =>
            context.Handler.Handle(context.UpdateCommandWithHours(30),CancellationToken.None));    
        
        context.VerifyTimeEntryNotUpdated();
    }



}
public class UpdateTimeEntryTestContext
{   
    public const string NewDescription="Izmenjeno";
    public const double Hours=7.5;
    public Guid timeEntryId{get; }=Guid.NewGuid();
    public Guid oldCategoryId{get; }=Guid.NewGuid();
    public Guid oldProjectId{get; }=Guid.NewGuid();
    public Guid newCategoryId{get; }=Guid.NewGuid();
    public Guid newProjectId{get; }=Guid.NewGuid();
    public Guid clinetId{get; }=Guid.NewGuid();
    public Guid memberId{get; }=Guid.NewGuid();

    public Mock<ITimeEntryRepository> TimeEntryRepositoryMock { get; } = new();
    public Mock<ICategoryRepository> CategoryRepositoryMock { get; } = new();
    public Mock<IProjectRepository> ProjectRepositoryMock { get; } = new();
    public UpdateTimeEntryHandler Handler { get; }
    public TimeEntryModel? UpdatedTimeEntry { get; private set;}
    public UpdateTimeEntryTestContext()
    {
        TimeEntryRepositoryMock
            .Setup(r=>r.UpdateAsync(It.IsAny<TimeEntryModel>()))
            .Callback<TimeEntryModel>(te=>UpdatedTimeEntry=te);

        Handler = new UpdateTimeEntryHandler(TimeEntryRepositoryMock.Object,
        ProjectRepositoryMock.Object,
        CategoryRepositoryMock.Object);
    }
    public UpdateTimeEntryCommand UpdateValidCommand()
    {   
        return UpdateCommandWithHours(Hours);
    }

    public UpdateTimeEntryCommand UpdateCommandWithHours(double hours)
    {
        return new UpdateTimeEntryCommand(timeEntryId, new UpdateTimeEntryRequest
        {
           Date=new DateOnly(2026, 11, 25),
           Hours=hours,
           Overtime=0.5,
           Description=NewDescription,
           ProjectId=newProjectId,
           CategoryId=newCategoryId,
        });
    }

  
    public UpdateTimeEntryTestContext WithTimeEntryExisting()
    {
        TimeEntryRepositoryMock
            .Setup(r => r.GetByIdAsync(timeEntryId))
            .ReturnsAsync(new TimeEntryModel(timeEntryId, new DateOnly(2026, 11, 25), 7.5, 0.5, "Staro", 
             project: oldProjectId,
             category: oldCategoryId,
             teammember: memberId));
        return this;
    }

    public UpdateTimeEntryTestContext WithTimeEntryNotFound()
    {
        TimeEntryRepositoryMock
            .Setup(r => r.GetByIdAsync(timeEntryId))
            .ReturnsAsync((TimeEntryModel?)null);
        return this;
    }

    public UpdateTimeEntryTestContext WithProjectExisting()
    {
        ProjectRepositoryMock
            .Setup(r=> r.GetByIdAsync(newProjectId))
            .ReturnsAsync(new ProjectModel(newProjectId, "ProjectName", "Descripion", ProjectStatus.Active, clinetId, memberId));
        return this;
    }
     public UpdateTimeEntryTestContext WithProjectNotFound()
    {
        ProjectRepositoryMock
            .Setup(r=> r.GetByIdAsync(newProjectId))
            .ReturnsAsync((ProjectModel?)null);
        return this;
    }

    
    public UpdateTimeEntryTestContext WithCategoryExisting()
    {
        CategoryRepositoryMock
            .Setup(r=> r.GetByIdAsync(newCategoryId))
            .ReturnsAsync(new CategoryModel(newCategoryId, "Design"));
        return this;
    }
     public UpdateTimeEntryTestContext WithCategoryNotFound()
    {
        CategoryRepositoryMock
            .Setup(r=> r.GetByIdAsync(newCategoryId))
            .ReturnsAsync((CategoryModel?)null);
        return this;
    }


    public void VerifyTimeEntryNotUpdated()
    {
        TimeEntryRepositoryMock.Verify(r => r.UpdateAsync(It.IsAny<TimeEntryModel>()), Times.Never);
    }

}