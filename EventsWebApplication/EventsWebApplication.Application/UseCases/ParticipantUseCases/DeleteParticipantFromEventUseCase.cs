﻿using AutoMapper;
using EventsWebApplication.Application.UseCases.ParticipantUseCases.Interface;
using EventsWebApplication.Domain.Interfaces;

namespace EventsWebApplication.Application.UseCases.ParticipantUseCases;

public class DeleteParticipantFromEventUseCase : IDeleteParticipantFromEventUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteParticipantFromEventUseCase(IUnitOfWork unitOfWork) 
    {
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(Guid userId, Guid eventId, CancellationToken cancellationToken)
    {
        var userEventConnection = await _unitOfWork.EventRepository.GetUserEventConnection(userId, eventId, cancellationToken);

        if (userEventConnection == null)
        {
            throw new KeyNotFoundException("User is not a participant of the event");
        }

        _unitOfWork.EventRepository.DeleteParticipantFromEvent(userEventConnection);
        await _unitOfWork.EventRepository.Commit(cancellationToken);
    }
}