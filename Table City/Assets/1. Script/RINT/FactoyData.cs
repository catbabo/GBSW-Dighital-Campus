using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoyData : MonoBehaviour
{
    public Factory data;

    [Header("저장된 자원")]
    public Asset asset;

    public void SetFactoryItem(Asset count)
    {
        asset.ChangeData(AssetData.wood, count.wood);
        asset.ChangeData(AssetData.uranium, count.uranium);
        asset.ChangeData(AssetData.stone, count.stone);
        asset.ChangeData(AssetData.steel, count.steel);
        asset.ChangeData(AssetData.semiconductor, count.semiconductor);
        asset.ChangeData(AssetData.rubber, count.rubber);
        asset.ChangeData(AssetData.mithril, count.mithril);
        asset.ChangeData(AssetData.glass, count.glass);
        asset.ChangeData(AssetData.floatingStone, count.floatingStone);
        asset.ChangeData(AssetData.electricity, count.electricity);
        asset.ChangeData(AssetData.cloth, count.cloth);
        asset.ChangeData(AssetData.coal, count.coal);

        for (int i = 0; i < data.upGrade.Length; i++)
        {
            if (!data.upGrade[data.lv].InputWorthMoreThanThis(AssetData.wood, asset.wood)) continue;
            else if (!data.upGrade[data.lv].InputWorthMoreThanThis(AssetData.uranium, asset.uranium)) continue;
            else if (!data.upGrade[data.lv].InputWorthMoreThanThis(AssetData.stone, asset.stone)) continue;
            else if (!data.upGrade[data.lv].InputWorthMoreThanThis(AssetData.steel, asset.steel)) continue;
            else if (!data.upGrade[data.lv].InputWorthMoreThanThis(AssetData.semiconductor, asset.semiconductor)) continue;
            else if (!data.upGrade[data.lv].InputWorthMoreThanThis(AssetData.rubber, asset.rubber)) continue;
            else if (!data.upGrade[data.lv].InputWorthMoreThanThis(AssetData.mithril, asset.mithril)) continue;
            else if (!data.upGrade[data.lv].InputWorthMoreThanThis(AssetData.glass, asset.glass)) continue;
            else if (!data.upGrade[data.lv].InputWorthMoreThanThis(AssetData.floatingStone, asset.floatingStone)) return;
            else if (!data.upGrade[data.lv].InputWorthMoreThanThis(AssetData.electricity, asset.electricity)) return;
            else if (!data.upGrade[data.lv].InputWorthMoreThanThis(AssetData.cloth, asset.cloth)) return;
            else if (!data.upGrade[data.lv].InputWorthMoreThanThis(AssetData.coal, asset.coal)) return;

            data.lv += 1;
        }

    }

}
