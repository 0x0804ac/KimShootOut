using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Multiplayer;
using UnityEngine;


public sealed class SessionLobbyTutorial : MonoBehaviour
{
    private const int MaxPlayers = 10;

    private readonly List<ISessionInfo> rooms = new List<ISessionInfo>();
    private ISession currentSession;

    private string roomName = "Test Room";
    private string joinCode = "";
    private string status = "Initializing Unity Services...";
    private bool ready;
    private bool busy;
    private Vector2 scrollPosition;

    private async void Start()
    {
        busy = true;
        try
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }

            ready = true;
            status = "Signed in: " + AuthenticationService.Instance.PlayerId;
            await QueryRoomsInternalAsync();
        }
        catch (Exception ex)
        {
            status = "Initialization failed: " + ex.Message;
            Debug.LogException(ex);
        }
        finally
        {
            busy = false;
        }
    }

    // All UI operations go through this method to prevent duplicate requests.
    private async void Run(Func<Task> operation)
    {
        if (!ready || busy)
            return;

        busy = true;
        try
        {
            await operation();
        }
        catch (Exception ex)
        {
            status = "Request failed: " + ex.Message;
            Debug.LogException(ex);
        }
        finally
        {
            busy = false;
        }
    }

    private void CreateRoom()
    {
        if (currentSession != null)
            return;

        Run(async () =>
        {
            var options = new SessionOptions
            {
                Name = string.IsNullOrWhiteSpace(roomName) ? "Untitled Room" : roomName.Trim(),
                MaxPlayers = MaxPlayers,
                IsPrivate = false,
                IsLocked = false
                // No WithRelayNetwork(): this demo creates only a lobby session.
            };

            var session = await MultiplayerService.Instance.CreateSessionAsync(options);
            SetCurrentSession(session);
            status = "Room created. Invite code: " + session.Code;
        });
    }

    private void RefreshRooms()
    {
        Run(QueryRoomsInternalAsync);
    }

    private async Task QueryRoomsInternalAsync()
    {
        var query = new QuerySessionsOptions { Count = 25 };
        var results = await MultiplayerService.Instance.QuerySessionsAsync(query);

        rooms.Clear();
        rooms.AddRange(results.Sessions);
        status = "Found " + rooms.Count + " joinable public room(s).";
    }

    private void JoinById(string sessionId)
    {
        if (currentSession != null || string.IsNullOrWhiteSpace(sessionId))
            return;

        Run(async () =>
        {
            var session = await MultiplayerService.Instance.JoinSessionByIdAsync(sessionId);
            SetCurrentSession(session);
            status = "Joined room: " + session.Name;
        });
    }

    private void JoinByCode()
    {
        if (currentSession != null || string.IsNullOrWhiteSpace(joinCode))
            return;

        Run(async () =>
        {
            var session = await MultiplayerService.Instance.JoinSessionByCodeAsync(joinCode.Trim());
            SetCurrentSession(session);
            status = "Joined by code: " + session.Name;
        });
    }

    private void LeaveRoom()
    {
        if (currentSession == null)
            return;

        Run(async () =>
        {
            ISession leaving = currentSession;
            await leaving.LeaveAsync();
            if (ReferenceEquals(currentSession, leaving))
                SetCurrentSession(null);
            status = "Left the room.";
            await QueryRoomsInternalAsync();
        });
    }

    private void DeleteRoom()
    {
        if (currentSession == null || !currentSession.IsHost)
            return;

        Run(async () =>
        {
            ISession deleting = currentSession;
            await deleting.AsHost().DeleteAsync();
            if (ReferenceEquals(currentSession, deleting))
                SetCurrentSession(null);
            status = "Deleted the room.";
            await QueryRoomsInternalAsync();
        });
    }

    private void SetCurrentSession(ISession session)
    {
        if (currentSession != null)
        {
            currentSession.Changed -= OnSessionChanged;
            currentSession.Deleted -= OnSessionDeleted;
        }

        currentSession = session;

        if (currentSession != null)
        {
            currentSession.Changed += OnSessionChanged;
            currentSession.Deleted += OnSessionDeleted;
        }
    }

    private void OnSessionChanged()
    {
        // Session properties such as PlayerCount are updated by the SDK.
        // OnGUI displays currentSession's latest values on its next repaint.
    }

    private void OnSessionDeleted()
    {
        SetCurrentSession(null);
        status = "This room was deleted by the host.";
    }

    private void OnDestroy()
    {
        // Unsubscribe when the scene object is destroyed.
        // Use the Leave button for an explicit backend leave request.
        SetCurrentSession(null);
    }

    // The built-in immediate-mode UI keeps this tutorial to one C# file.
    private void OnGUI()
    {
        float width = Mathf.Max(250f, Screen.width - 32f);
        float height = Mathf.Max(200f, Screen.height - 32f);
        GUILayout.BeginArea(new Rect(16f, 16f, width, height), GUI.skin.box);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        GUILayout.Label("UNITY SESSIONS - LOBBY ONLY");
        GUILayout.Label(status);
        if (busy)
            GUILayout.Label("Processing...");

        GUILayout.Space(12f);
        if (currentSession == null)
        {
            GUILayout.Label("Create room (up to 10 players)");
            roomName = GUILayout.TextField(roomName, 60);

            GUI.enabled = ready && !busy;
            if (GUILayout.Button("Create public room", GUILayout.Height(32f)))
                CreateRoom();

            GUILayout.Space(12f);
            GUILayout.Label("Join with invite code");
            joinCode = GUILayout.TextField(joinCode, 40);
            GUI.enabled = ready && !busy && !string.IsNullOrWhiteSpace(joinCode);
            if (GUILayout.Button("Join by code", GUILayout.Height(32f)))
                JoinByCode();

            GUILayout.Space(12f);
            GUI.enabled = ready && !busy;
            if (GUILayout.Button("Refresh public rooms", GUILayout.Height(32f)))
                RefreshRooms();

            GUI.enabled = true;
            GUILayout.Label("Available rooms: " + rooms.Count);
            foreach (var info in rooms)
            {
                GUILayout.BeginHorizontal(GUI.skin.box);
                GUILayout.Label(info.Name + " (" + "1" + "/" + info.MaxPlayers + ")", GUILayout.MinWidth(130f));
                GUI.enabled = ready && !busy;
                string roomId = info.Id;
                if (GUILayout.Button("Join", GUILayout.Width(85f)))
                    JoinById(roomId);
                GUI.enabled = true;
                GUILayout.EndHorizontal();
            }
        }
        else
        {
            GUILayout.Label("CURRENT ROOM");
            GUILayout.Label("Name: " + currentSession.Name);
            GUILayout.Label("ID: " + currentSession.Id);
            GUILayout.Label("Code: " + currentSession.Code);
            GUILayout.Label("Players: " + currentSession.PlayerCount + "/" + currentSession.MaxPlayers);
            GUILayout.Label("Role: " + (currentSession.IsHost ? "Host" : "Member"));

            GUI.enabled = ready && !busy;
            if (GUILayout.Button("Leave room", GUILayout.Height(32f)))
                LeaveRoom();

            GUI.enabled = ready && !busy && currentSession.IsHost;
            if (GUILayout.Button("Delete room (Host only)", GUILayout.Height(32f)))
                DeleteRoom();
        }

        GUI.enabled = true;
        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }
}
