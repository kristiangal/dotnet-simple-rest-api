using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RestAPI.DTO;
using RestAPI.Models;
using RestAPI.Repositories;
using RestAPI.Services;

namespace RestAPI.Tests.Services;

public class NoteServiceTests
{
    // NoteService
    // •	create note
    // •	Repository Add method is called
    // •	Unit-of-work SaveChangesAsync is called
    // •	Optional: check Created timestamp is set

    private readonly IRepository<Note> _repositoryMock = Substitute.For<IRepository<Note>>();
    private readonly IUnitOfWork _unitOfWorkMock = Substitute.For<IUnitOfWork>();
    private readonly ILogger<NoteService> _loggerMock = Substitute.For<ILogger<NoteService>>();
    private readonly INoteService _service;

    public NoteServiceTests()
    {
        _service = new NoteService(_repositoryMock, _unitOfWorkMock, _loggerMock);
    }

    [Fact]
    public async Task CreateNote_ShouldAddNoteForLoggedInUser()
    {
        var requestDto = new CreateNoteDto("Test title", "Content");
        var loggedInUserId = 1;

        var noteResponse = await _service.CreateNote(requestDto, loggedInUserId);
        
        Assert.Equal(noteResponse.UserId, loggedInUserId);
    }

    [Fact]
    public async Task CreateNote_ShouldCallTheAddMethod()
    {
        var requestDto = new CreateNoteDto("Test title", "Content");
        var loggedInUserId = 1;

        await _service.CreateNote(requestDto, loggedInUserId);

        await _repositoryMock.Received().Add(Arg.Is<Note>(note => note.UserId == loggedInUserId && note.Title == requestDto.Title));
    }
    
    [Fact]
    public async Task CreateNote_ShouldCallTheSaveChangesAsyncMethod()
    {
        var requestDto = new CreateNoteDto("Test title", "Content");
        var loggedInUserId = 1;

        await _service.CreateNote(requestDto, loggedInUserId);

        await _unitOfWorkMock.Received().SaveChangesAsync();
    }

    [Fact]
    public async Task GetNoteById_ShouldReturnNull_IfNoteDoesNotExistForUser()
    {
        var noteId = 1;
        var loggedInUserId = 1;

        _repositoryMock.Find(Arg.Any<Expression<Func<Note, bool>>>())
            .Returns(new List<Note>());
        
        var returnedNote = await _service.GetNoteById(noteId, loggedInUserId);
        
        Assert.Null(returnedNote);
    }
}