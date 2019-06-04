using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IOCLAndroidApp;
using IOCLAndroidApp.Models;
using SQLite;

namespace SatoScanningApp.DAL
{
    class MappingDb
    {
        public string ValidateIMEI(string Imei)
        {
            try
            {
                string Msg = "Y";
                using (var connection = new SQLiteConnection(clsGlobal.DBFilePath))
                {
                    //Check already mapp or not
                    var mapping = connection.Find<Mapping>(Imei);
                    if (mapping == null)
                    {
                        //Check whether IMEI is valid or not
                        var Item = connection.Query<Item>("SELECT * FROM Item WHERE IMEI1 = ?", Imei);
                        if(Item==null || Item.Count==0)
                            return "N~ This is not valid IMEI";
                    }
                    else
                        return "N~IMEI already mapped";
                }
                return Msg;
            }
            catch (SQLiteException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex) { throw ex; }
        }

        public string MapRfid(Mapping mapping)
        {
            try
            {
                string Msg = ValidateIMEI(mapping.IMEI);
                if (Msg.Split('~')[0] == "Y")
                {
                    using (var connection = new SQLiteConnection(clsGlobal.DBFilePath))
                    {
                        //Check barcode already mapp or not
                        var Item = connection.Query<Item>("SELECT * FROM Mapping WHERE RfidBarcode = ?", mapping.RfidBarcode);
                        if (Item.Count == 0)
                        {
                            connection.Insert(mapping);
                            return "Y";
                        }
                        else
                            return "N~Barcode already mapped";

                    }
                }
                return Msg;
            }
            catch (SQLiteException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex) { throw ex; }
        }
    }
}