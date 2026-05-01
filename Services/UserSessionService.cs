using eventease.Models;
using System;

namespace eventease.Services;

public class UserSessionService
{
    public User? CurrentUser { get; private set; }
    public bool IsLoggedIn => CurrentUser != null;

    public event Action? OnUserChanged;

    public void RegisterUser(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Name and email are required.");

        CurrentUser = new User
        {
            Id = Guid.NewGuid().GetHashCode(), // Better ID generation
            Name = name,
            Email = email,
            AttendingEventIds = new List<int>()
        };
        OnUserChanged?.Invoke();
    }

    public void AttendEvent(int eventId)
    {
        if (CurrentUser == null) return;
        if (!CurrentUser.AttendingEventIds.Contains(eventId))
        {
            CurrentUser.AttendingEventIds.Add(eventId);
            OnUserChanged?.Invoke();
        }
    }

    public void UnattendEvent(int eventId)
    {
        if (CurrentUser != null)
        {
            CurrentUser.AttendingEventIds.Remove(eventId);
            OnUserChanged?.Invoke();
        }
    }

    public bool IsAttending(int eventId)
    {
        return CurrentUser?.AttendingEventIds.Contains(eventId) ?? false;
    }

    public void Logout()
    {
        CurrentUser = null;
        OnUserChanged?.Invoke();
    }
}