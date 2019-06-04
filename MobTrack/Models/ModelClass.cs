using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;

namespace IOCLAndroidApp.Models
{
    #region Validate User
    public class LoginDetail
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
    public class LoginRequest
    {
        public string UserId { get; set; }
        public string Password { get; set; }
    }
    public class LoginResponse
    {
        public List<LoginDetail> LoginDetails { get; set; }
        public string Response { get; set; }
        public string ErrorMessage { get; set; }
    }

    #endregion

    #region ActiveTags
    public class ActiveTagCountRequest
    {
        public string UserId { get; set; }
        public string VehicleType { get; set; }
    }
    public class ActiveTagCountResponse
    {
        public int TotalTagCount { get; set; }
        public string Response { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class ActiveTagRequest
    {
        public string TagId { get; set; }
        public string UserId { get; set; }
        public string MobileNo { get; set; }
        public string VehicleType { get; set; }
        public string VehicleNo { get; set; }
        public string Name { get; set; }
    }
    public class ActiveTagResponse
    {
        public string Response { get; set; }
        public string ErrorMessage { get; set; }
    }

    #endregion

    #region Item
    public class Item
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string BranchCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Group { get; set; }
        public string ItemSubGroup { get; set; }
        public string ItemName { get; set; }
        public string Color { get; set; }
        public string IMEI1 { get; set; }
        public string IMEStock { get; set; }
        public string BranchName { get; set; }
        public string WHName { get; set; }
    }

    #endregion

    #region Mapping
    public class Mapping
    {
        [PrimaryKey]
        public string IMEI { get; set; }
        public string RfidBarcode { get; set; }
    }

    #endregion

    #region Mapping
    public class StockTake
    {
        [PrimaryKey]
        public string RfidBarcode { get; set; }
    }

    #endregion
}