using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IOCLAndroidApp;
using IOCLAndroidApp.Models;
using SQLite;
namespace SatoScanningApp.DAL
{
    class ItemDb
    {
        public bool CreateDatabase()
        {
            try
            {
                string folderPath = Path.Combine(clsGlobal.FilePath, clsGlobal.FileFolder);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                using (var connection = new SQLiteConnection(clsGlobal.DBFilePath))
                {
                    var tableName = connection.GetTableInfo("Mapping");
                    if (tableName.Count == 0)
                        connection.CreateTable<Mapping>();

                    tableName = connection.GetTableInfo("Item");
                    if (tableName.Count == 0)
                        connection.CreateTable<Item>();

                    tableName = connection.GetTableInfo("StockTake");
                    if (tableName.Count == 0)
                        connection.CreateTable<StockTake>();

                    return true;
                }
            }
            catch (SQLiteException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex) { throw ex; }
        }

        public bool AddItem(Item item)
        {
            try
            {
                using (var connection = new SQLiteConnection(clsGlobal.DBFilePath))
                {
                    connection.Insert(item);
                    return true;
                }
            }
            catch (SQLiteException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex) { throw ex; }
        }

        public bool DeleteItems()
        {
            try
            {
                using (var connection = new SQLiteConnection(clsGlobal.DBFilePath))
                {
                    int Count = connection.DeleteAll<Item>();
                    return true;
                }
            }
            catch (SQLiteException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex) { throw ex; }
        }
        public List<Item> GetAllItem()
        {
            try
            {
                using (var connection = new SQLiteConnection(clsGlobal.DBFilePath))
                {
                    return connection.Table<Item>().ToList();
                }
            }
            catch (SQLiteException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}