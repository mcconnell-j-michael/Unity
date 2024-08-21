using Ashen.PartySystem;
using Ashen.StateMachineSystem;
using Ashen.ToolSystem;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerInputState : SingletonScriptableObject<PlayerInputState>, I_GameState
{
    [NonSerialized]
    public A_PartyManager sourceParty;
    [NonSerialized]
    public ToolManager currentlySelected;
    [NonSerialized]
    public int turn = 0;
    [NonSerialized]
    public MenuPlayerInputManager inputManager;

    private Dictionary<ToolManager, InitialPlayerChoiceState> previousChoices;

    private InputProcessorState currentState;
    private A_PartyManager playerParty;
    private A_PartyUIManager playerUiManager;
    private PartyResourceTracker resourceTracker;

    public void Initialize()
    {
        previousChoices = new Dictionary<ToolManager, InitialPlayerChoiceState>();
        currentState = InputProcessorState.INITIAL;
    }

    private void Reset()
    {
        sourceParty = PlayerPartyHolder.Instance.partyManager;
        ActionOptionsManager.Instance.previouslySelected = null;
    }

    public IEnumerator RunState(GameStateRequest request, GameStateResponse response)
    {
        while (currentState != InputProcessorState.NULL)
        {
            InputProcessorState curState = currentState;
            currentState = InputProcessorState.NULL;
            switch (curState)
            {
                case InputProcessorState.INITIAL:
                    InitialState();
                    break;
                case InputProcessorState.PROCESS_CHARACTER:
                    yield return ProcessCharacterState(request);
                    break;
                case InputProcessorState.RESTART:
                    yield return RestartState(request);
                    break;
                case InputProcessorState.MOVE_PREVIOUS:
                    yield return MovePreviousState(request);
                    break;
                case InputProcessorState.FINALIZE_ACTION:
                    yield return FinalizeActionState(request);
                    break;
                case InputProcessorState.REQUEST_PROCEED:
                    yield return RequestProceedWIthCombatState(request);
                    break;
                case InputProcessorState.PROCEED_WITH_COMBAT:
                    yield return ProceedWithCombatRoundState(request, response);
                    break;
                case InputProcessorState.MOVE_NEXT_CHARACTER:
                    yield return MoveNextCharacter(request);
                    break;
                case InputProcessorState.MOVE_PREVIOUS_CHARACTER:
                    yield return MovePreviousCharacter(request);
                    break;
            }
        }
        currentState = InputProcessorState.INITIAL;
    }

    private void InitialState()
    {
        Reset();

        inputManager = MenuPlayerInputManager.Instance;
        inputManager.Enable();

        playerParty = PlayerPartyHolder.Instance.partyManager;
        playerUiManager = playerParty.GetPartyUIManager();
        resourceTracker = PlayerPartyHolder.Instance.partyManager.partyToolManager.Get<PartyResourceTracker>();

        resourceTracker.ResetReservedTotals();

        CombatLog.Instance.ClearMessages();
        CombatLog.Instance.AddMessage("What will you do?");

        ActionOptionsManager.Instance.gameObject.SetActive(true);
        currentlySelected = GetNextAvailable(playerParty, null);
        if (currentlySelected != null)
        {
            A_CharacterSelector manager = playerUiManager.GetCharacterSelector(playerParty.GetPosition(currentlySelected));
            manager.TurnSelectionStart();
            currentState = InputProcessorState.PROCESS_CHARACTER;
        }
    }

    private IEnumerator ProcessCharacterState(GameStateRequest request)
    {
        Reset();
        GameStateManager internalPlayerInput = CreateInstance<GameStateManager>();
        InitialPlayerChoiceState initialState = null;
        //if (!previousChoices.TryGetValue(currentlySelected, out initialState))
        //{
        initialState = new InitialPlayerChoiceState();
        //}
        internalPlayerInput.initialState = initialState;
        GameStateResponse newGameStateResponse = new GameStateResponse();
        yield return internalPlayerInput.RunState(request, newGameStateResponse);
    }

    private IEnumerator RestartState(GameStateRequest request)
    {
        ChangeTurn(playerParty, playerUiManager, currentlySelected, GetNextAvailable(playerParty, null));
        playerParty.GetCurrentBattleContainer().ClearProcessors();
        currentlySelected = GetNextAvailable(playerParty, null);
        CombatTool ct = currentlySelected.Get<CombatTool>();

        RollbackAll();
        resourceTracker.ResetReservedTotals();
        currentState = InputProcessorState.PROCESS_CHARACTER;
        yield break;
    }

    private IEnumerator MovePreviousState(GameStateRequest request)
    {
        if (GetActionCount() <= 0)
        {
            currentState = InputProcessorState.MOVE_PREVIOUS_CHARACTER;
        }
        else
        {
            //DecreaseActionCount();
            CombatTool ct = currentlySelected.Get<CombatTool>();
            ct.Rollback(GetActionCount() - 1);
            currentState = InputProcessorState.PROCESS_CHARACTER;
        }

        yield break;
    }

    private IEnumerator MoveNextCharacter(GameStateRequest request)
    {
        ToolManager next = GetNextAvailable(playerParty, currentlySelected);
        if (next != null)
        {
            ChangeTurn(playerParty, playerUiManager, currentlySelected, next);
            currentlySelected = next;
            currentState = InputProcessorState.PROCESS_CHARACTER;
        }
        else
        {
            ChangeTurn(playerParty, playerUiManager, currentlySelected, null);
            currentlySelected = null;
            currentState = InputProcessorState.REQUEST_PROCEED;
        }
        yield break;
    }

    private IEnumerator MovePreviousCharacter(GameStateRequest request)
    {
        ToolManager previous = GetPreviousAvailable(playerParty, currentlySelected);
        if (previous != null)
        {
            ChangeTurn(playerParty, playerUiManager, currentlySelected, previous);
            currentlySelected = previous;
        }
        currentState = InputProcessorState.PROCESS_CHARACTER;
        yield break;
    }

    private IEnumerator FinalizeActionState(GameStateRequest request)
    {
        CombatTool ct = currentlySelected.Get<CombatTool>();
        ct.Commit();
        currentState = InputProcessorState.PROCESS_CHARACTER;
        yield break;
    }

    private IEnumerator RequestProceedWIthCombatState(GameStateRequest request)
    {
        GameStateManager manager = CreateInstance<GameStateManager>();
        NavigateConfirmationDialogue state = new NavigateConfirmationDialogue("Proceed with combat?", new ProceedWithCombatRoundState(), new ExitState());
        GameStateResponse newGameStateResponse = new GameStateResponse();
        manager.initialState = state;
        yield return manager.RunState(request, newGameStateResponse);
        if (currentState == InputProcessorState.NULL)
        {
            ToolManager current = GetPreviousAvailable(playerParty, null);
            if (current != null)
            {
                ChangeTurn(playerParty, playerUiManager, currentlySelected, current);
                currentlySelected = current;
            }
            currentState = InputProcessorState.PROCESS_CHARACTER;
        }
    }

    private IEnumerator ProceedWithCombatRoundState(GameStateRequest request, GameStateResponse response)
    {
        ActionOptionsManager.Instance.gameObject.SetActive(false);
        resourceTracker.ResetReservedTotals();
        yield return null;
        Reset();
        response.nextState = EnemyInputState.Instance;
        currentState = InputProcessorState.NULL;
    }

    public void RequestRestart()
    {
        currentState = InputProcessorState.RESTART;
    }

    public void RequestMovePrevious()
    {
        currentState = InputProcessorState.MOVE_PREVIOUS;
    }

    public void RequestMoveNextAction()
    {
        currentState = InputProcessorState.FINALIZE_ACTION;
    }

    public void RequestMoveNextCharacter()
    {
        currentState = InputProcessorState.MOVE_NEXT_CHARACTER;
    }

    public void RequestMovePreviousCharacter()
    {
        currentState = InputProcessorState.MOVE_PREVIOUS_CHARACTER;
    }

    public void RequestProceedWithCombat()
    {
        currentState = InputProcessorState.PROCEED_WITH_COMBAT;
    }

    private ToolManager GetNextAvailable(A_PartyManager playerParty, ToolManager current)
    {
        ToolManager next;
        if (!current)
        {
            next = playerParty.GetFirst();
        }
        else
        {
            next = playerParty.GetNext(current);
        }

        if (!next)
        {
            return null;
        }

        FacultyTool fTool = next.Get<FacultyTool>();
        while (next && !fTool.Can(Faculties.Instance.CHOOSE_ACTION))
        {
            next = playerParty.GetNext(next);
            if (next)
            {
                fTool = next.Get<FacultyTool>();
            }
        }
        return next;
    }

    private ToolManager GetPreviousAvailable(A_PartyManager playerParty, ToolManager current)
    {
        ToolManager last;
        if (!current)
        {
            last = playerParty.GetLast();
        }
        else
        {
            last = playerParty.GetPrevious(current);
        }

        if (!last)
        {
            return null;
        }

        FacultyTool fTool = last.Get<FacultyTool>();
        while (last && !fTool.Can(Faculties.Instance.CHOOSE_ACTION))
        {
            last = playerParty.GetPrevious(last);
            if (last)
            {
                fTool = last.Get<FacultyTool>();
            }
        }
        return last;
    }

    public int GetActionCount()
    {
        CombatTool ct = currentlySelected.Get<CombatTool>();
        return ct.Count();
    }

    private void RollbackAll()
    {
        PlayerPartyManager manager = PlayerPartyHolder.Instance.partyManager;
        foreach (PartyPosition pos in manager.GetActivePositions())
        {
            ToolManager tm = manager.GetToolManager(pos);
            CombatTool ct = tm.Get<CombatTool>();
            ct.Rollback();
        }
    }

    private void ChangeTurn(A_PartyManager party, A_PartyUIManager playerUiManager, ToolManager lastTurn, ToolManager nextTurn)
    {
        if (lastTurn != null)
        {
            A_CharacterSelector manager = playerUiManager.positionToManager[party.GetPosition(lastTurn)];
            manager.TurnSelectionEnd();
        }
        if (nextTurn != null)
        {
            A_CharacterSelector manager = playerUiManager.positionToManager[party.GetPosition(nextTurn)];
            manager.TurnSelectionStart();
        }
    }

    enum InputProcessorState
    {
        INITIAL, PROCESS_CHARACTER, RESTART, MOVE_PREVIOUS, FINALIZE_ACTION, REQUEST_PROCEED,
        PROCEED_WITH_COMBAT, NULL, MOVE_NEXT_CHARACTER, MOVE_PREVIOUS_CHARACTER
    }
}
