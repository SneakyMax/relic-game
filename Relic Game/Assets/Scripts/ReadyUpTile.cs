using System;
using UnityEngine;
using System.Linq;
using Assets.Scripts;
using UnityEngine.UI;

public class ReadyUpTile : MonoBehaviour
{
    public int PlayerNumber = -1;

    public PlayersDefinition PlayersDefinition;

    public ReadyUpController ReadyUpController;

    public bool IsReady { get; private set; }

    public void Start()
    {
        if (PlayerNumber < 1)
            throw new InvalidOperationException("PlayerNumber on ready up screen not set");

        if (PlayersDefinition == null)
            throw new InvalidOperationException("No players definition");

        if (ReadyUpController == null)
            throw new InvalidOperationException("No ready up controller.");

        var definition = PlayersDefinition.Players.FirstOrDefault(x => x.PlayerNumber == PlayerNumber);
        if (definition.PlayerNumber == 0)
            throw new InvalidOperationException("Couldn't find player " + PlayerNumber);

        GetComponent<Image>().color = definition.Color;

        RefreshData();
    }

    public void Update()
    {
        if (Input.GetButtonDown("buttonA" + PlayerNumber))
        {
            ToggleIsReady();
        }

        if (Input.GetButtonDown("Start" + PlayerNumber))
            ReadyUpController.TryStart();
    }

    private void ToggleIsReady()
    {
        IsReady = !IsReady;
        ReadyUpController.ReadyUp(PlayerNumber, IsReady);

        RefreshData();
    }

    private void RefreshData()
    {
        var text = GetComponentInChildren<Text>();

        if (text == null)
            throw new InvalidOperationException("No text in ready up tile.");

        if (IsReady)
        {
            text.text = "Ready!";
        }
        else
        {
            text.text = "Press A...";
        }
    }
}
