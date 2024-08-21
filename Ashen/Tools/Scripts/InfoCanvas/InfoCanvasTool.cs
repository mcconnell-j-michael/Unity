using Ashen.ToolSystem;
using System.Collections.Generic;

public class InfoCanvasTool : A_ConfigurableTool<InfoCanvasTool, InfoCanvasToolConfiguration>
{
    public BarCanvasManager barCanvasManager;
    public StatusEffectSymbolsManager statusEffectSymbolsManager;

    public override void Initialize()
    {
        base.Initialize();
        List<ResourceValue> defaultResourceValues = Config.DefaultResourceValues;
        BarInfo[] barInfos = Config.DerivedBarConfigurations;
        barCanvasManager.defaultResourceValues = defaultResourceValues;
        barCanvasManager.barInfos = barInfos;
        barCanvasManager.initialValue = Config.BarInitialValue;
        barCanvasManager.growthRate = Config.BarGrowthValue;
        barCanvasManager.toolManager = toolManager;

        statusEffectSymbolsManager.initialValue = Config.SymbolInitialValue;
        statusEffectSymbolsManager.growthRate = Config.SymbolGrowthValue;
    }
}
