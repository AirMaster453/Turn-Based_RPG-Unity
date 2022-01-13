using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PsychesBound
{
    /// <summary>
    /// For any object that wants to hold extra data that will be used by things such as abilities
    /// 
    /// </summary>
    public interface IHoldExtraData 
    {
        object GetExtraData(string key);

        void AddExtraData(string key, object value);

        void SetExtraData(string key, object value);

        bool RemoveExtraData(string key);

    }
}