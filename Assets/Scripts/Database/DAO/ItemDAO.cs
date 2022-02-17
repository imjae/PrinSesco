using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Amazon;
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

public enum ITEM_CATEGORY
{
    ALL,
    USE,
    EQUIP,
    PON,
    MATERIAL
}

public class ItemDAO : OriginalDAO
{
    public void LoadAllItemListExcuteCallback(Action<List<Item>> callback)
    {
        ddbm.context.QueryAsync<Item>("ITEM", QueryOperator.BeginsWith, (response) =>
        {
            // 에러뜨면 로그 출력후 함수 끝내기
            if (PrintExceptionLog(response)) return;

            response.Result.GetNextSetAsync(asyncItem => callback(asyncItem.Result));

        }, null, "ITEM#");
    }

    public void LoadStorageExcuteCallbackByCharacter(string characterName, ITEM_CATEGORY category, Action<List<Item>> callback)
    {
        ddbm.context.QueryAsync<Item>($"CHARACTER#{characterName}", QueryOperator.BeginsWith, (response) =>
        {
            Debug.Log($"STORAGE#{((category == ITEM_CATEGORY.ALL) ? "" : category.ToString())}");
            
            // 에러뜨면 로그 출력후 함수 끝내기
            if (PrintExceptionLog(response)) return;

            response.Result.GetNextSetAsync(asyncItem => callback(asyncItem.Result));

            // Debug.Log($"ITEM#{characterName}");

        }, null, $"STORAGE#{((category == ITEM_CATEGORY.ALL) ? "" : category.ToString().ToLower())}");
    }

    public bool PrintExceptionLog<T>(AmazonDynamoDBResult<AsyncSearch<T>> response)
    {
        if (response.Exception != null)
        {
            Debug.LogError(response.Exception);
            return true;
        }

        return false;
    }
}
