using Fluxor;

namespace eventease.Store;

public record EventState(List<EventModel> Events, bool IsLoading, string? ErrorMessage)
{
    public EventState() : this(new List<EventModel>(), false, null) { }
}

public class EventModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateTime Date { get; set; }
    public string? Location { get; set; }
}

public class EventFeature : Feature<EventState>
{
    public override string GetName() => "Events";

    protected override EventState GetInitialState() => new()
    {
        Events = new List<EventModel>
        {
            new() { Id = 1, Name = "Tech Conference", Date = new DateTime(2026, 5, 15, 10, 0, 0), Location = "Convention Center" },
            new() { Id = 2, Name = "Music Festival", Date = new DateTime(2026, 6, 20, 18, 0, 0), Location = "Central Park" },
            new() { Id = 3, Name = "Art Exhibition", Date = new DateTime(2026, 7, 10, 14, 0, 0), Location = "Art Gallery" }
        }
    };
}

public static class EventActions
{
    public record LoadEvents;
    public record LoadEventsSuccess(List<EventModel> Events);
    public record LoadEventsFailure(string Error);
    public record AddEvent(EventModel Event);
}

public class EventReducers
{
    [ReducerMethod]
    public static EventState ReduceLoadEvents(EventState state, EventActions.LoadEvents action) =>
        state with { IsLoading = true, ErrorMessage = null };

    [ReducerMethod]
    public static EventState ReduceLoadEventsSuccess(EventState state, EventActions.LoadEventsSuccess action) =>
        state with { Events = action.Events, IsLoading = false };

    [ReducerMethod]
    public static EventState ReduceLoadEventsFailure(EventState state, EventActions.LoadEventsFailure action) =>
        state with { IsLoading = false, ErrorMessage = action.Error };

    [ReducerMethod]
    public static EventState ReduceAddEvent(EventState state, EventActions.AddEvent action) =>
        state with { Events = state.Events.Append(action.Event).ToList() };
}