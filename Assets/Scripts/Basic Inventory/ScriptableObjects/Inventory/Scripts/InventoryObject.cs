using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using ScriptableObjects.Items.Scripts;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptableObjects.Inventory.Scripts
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
    public class InventoryObject : ScriptableObject
    {
        public string savePath;
        private ItemDatabaseObject _database;
        public Inventory Container { get; set; }

        public void AddItem(InventoryItem inventoryItem, int amount)
        {
            foreach (var inventorySlot in Container.Items
                         .Where(inventorySlot => inventorySlot.inventoryItem == inventoryItem))
            {
                inventorySlot.AddAmount(amount);
                return;
            }

            Container.Items.Add(new InventorySlot(inventoryItem.Id, inventoryItem, amount));
        }

        public void Save()
        {
            var saveData = JsonUtility.ToJson(this, true);
            var bf = new BinaryFormatter();
            var file = File.Create(string.Concat(Application.persistentDataPath, savePath));
            bf.Serialize(file, saveData);
            file.Close();
        }

        public void Load()
        {
            var path = string.Concat(Application.persistentDataPath, savePath);
            if (File.Exists(path))
            {
                var bf = new BinaryFormatter();
                var file = File.Open(path, FileMode.Open);
                JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
                file.Close();
            }
        }

    //     public void OnBeforeSerialize()
    //     {
    //         // throw new System.NotImplementedException();
    //     }
    //
    //     public void OnAfterDeserialize()
    //     {
    //         foreach (var inventorySlot in Container.Items)
    //         {
    //             inventorySlot.item = _database.idItem[inventorySlot.id];
    //         }
    //     }
    }

    [Serializable]
    public class Inventory
    {
        public List<InventorySlot> Items { get; set; } = new();
    }
    
    [Serializable]
    public class InventorySlot
    {
        public int id;
        [FormerlySerializedAs("item")] public InventoryItem inventoryItem;
        public int amount;

        public InventorySlot(int id, InventoryItem inventoryItem, int amount)
        {
            this.id = id;
            this.inventoryItem = inventoryItem;
            this.amount = amount;
        }

        public void AddAmount(int value)
        {
            amount += value;
        }
    }
}