using GameSparks.Api.Requests;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour {

	
    public void BuyVirtualGoodsRequest(string itemId, Action<bool> callbackRes, Action<string> callbackErr) {
        Debug.Log("[GS] @BuyVirtualGoodsRequest: " + itemId);

        new BuyVirtualGoodsRequest()
            .SetQuantity(1)
            .SetCurrencyShortCode("DINAR")
            .SetShortCode(itemId)
            .Send(response => {
                if (response.HasErrors) {
                    Debug.LogError("[GS] *Err @BuyVirtualGoodsRequest with ItemId " + itemId);
                    return;
                }
                else {
                    Debug.Log("[GS] @BuyVirtualGoodsRequest: Succesfully bought " + itemId);
                }
            });


    }

}
