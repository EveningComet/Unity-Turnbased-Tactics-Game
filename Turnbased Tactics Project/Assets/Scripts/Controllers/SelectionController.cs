using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TurnbasedGame.Units;
using TurnbasedGame.UI;

/// <summary>
/// In charge of handling what happens when a <see cref="Unit"/> is selected.
/// </summary>
public class SelectionController : MonoBehaviour
{
    /// <summary>
    /// The primary quick unit info panel. This gets displayed when a unit is selected.
    /// </summary>
    [SerializeField] private QuickUnitInfoPanel primaryQUIP = null;

    /// <summary>
    /// This displays the target of a selected unit.
    /// </summary>
    [SerializeField] private QuickUnitInfoPanel secondaryQUIP = null;

    [SerializeField] private AbilityHotPanel abilityHotPanel = null;

    /// <summary>
    /// Thing that will display chance of success of an attack/ability and stuff like how much damage to deal/heal/etc.
    /// </summary>
    [SerializeField] private SuccessRateIndicator successRateIndicator = null;

    public Unit CurrentlySelectedUnit { get; private set; }

    private void Start()
    {
        HidePrimary();
        HideSecondary();
        HideSuccessIndicator();
        HideAbilityHotPanel();
    }

    public void SelectUnit(Unit selectedUnit)
    {
        CurrentlySelectedUnit = selectedUnit;
#if UNITY_EDITOR
        Debug.LogFormat("SelectionController :: Selected unit {0}.", CurrentlySelectedUnit.gameObject.name);
#endif
    }

    public void DeselectUnit()
    {
        CurrentlySelectedUnit = null;
    }

    #region QuickUnitInfoPanel Methods
    /// <summary>
    /// Show or refresh the primary <see cref="QuickUnitInfoPanel"/> based on the passed <see cref="Unit"/>.
    /// </summary>
    public void ShowPrimary(Unit u)
    {
        primaryQUIP.Display(u);
    }

    /// <summary>
    /// Register the primary <see cref="QuickUnitInfoPanel"/> to the relevant events of the passed <see cref="Unit"/>.
    /// </summary>
    public void RegisterUnitToPrimary(Unit u)
    {
        primaryQUIP.RegisterToUnit(u);
    }

    /// <summary>
    /// Unregister the primary <see cref="QuickUnitInfoPanel"/> from the relevant events of the passed <see cref="Unit"/>.
    /// Be aware that this will throw an error if the passed <see cref="Unit"/> was not previously registered.
    /// </summary>
    public void UnregisterUnitFromPrimary(Unit u)
    {
        primaryQUIP.UnregisterUnit(u);
    }

    public void HidePrimary()
    {
        primaryQUIP.gameObject.SetActive(false);
    }

    /// <summary>
    /// Show or refresh the secondary <see cref="QuickUnitInfoPanel"/> based on the passed <see cref="Unit"/>.
    /// </summary>
    public void ShowSecondary(Unit u)
    {
        secondaryQUIP.Display(u);
    }

    /// <summary>
    /// Register the secondary <see cref="QuickUnitInfoPanel"/> to the relevant events of the passed <see cref="Unit"/>.
    /// </summary>
    public void RegisterUnitToSecondary(Unit u)
    {
        secondaryQUIP.RegisterToUnit(u);
    }

    /// <summary>
    /// Unregister the secondary <see cref="QuickUnitInfoPanel"/> from the relevant events of the passed <see cref="Unit"/>.
    /// Be aware that this will throw an error if the passed <see cref="Unit"/> was not previously registered.
    /// </summary>
    public void UnregisterUnitFromSecondary(Unit u)
    {
        secondaryQUIP.UnregisterUnit(u);
    }

    public void HideSecondary()
    {
        secondaryQUIP.gameObject.SetActive(false);
    }
    #endregion

    #region AbilityHotPanel Methods
    public void SetAbilityHotPanelController(TurnbasedGame.Inputs.MouseController mouseController)
    {
        abilityHotPanel.SetMyController(mouseController);
    }

    /// <summary>
    /// Activate the <see cref="AbilityHotPanel"/> and display the passed <see cref="Unit"/>'s
    /// abilities.
    /// </summary>
    public void ShowAbilityHotPanel(Unit unitToShow)
    {
        abilityHotPanel.Display(unitToShow);
        abilityHotPanel.gameObject.SetActive(true);
    }

    public void HideAbilityHotPanel()
    {
        abilityHotPanel.gameObject.SetActive(false);
        abilityHotPanel.Clear();
    }
    #endregion

    #region SuccessRateIndicator Methods
    public void UpdateSuccessRateText(int percentToShow, int amountToShow)
    {
        successRateIndicator.Show(percentToShow, amountToShow);
        successRateIndicator.gameObject.SetActive(true);
    }

    public void HideSuccessIndicator()
    {
        successRateIndicator.gameObject.SetActive(false);
    }
    #endregion
}
