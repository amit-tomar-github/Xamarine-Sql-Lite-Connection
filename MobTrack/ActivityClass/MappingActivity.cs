using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using IOCLAndroidApp;
using IOCLAndroidApp.Models;
using SatoScanningApp.DAL;

namespace SatoScanningApp.ActivityClass
{
    [Activity(Label = "Mapping", WindowSoftInputMode = SoftInput.StateHidden, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MappingActivity : Activity
    {
        clsGlobal clsGLB;
        MappingDb mappingDb;

        TextView txtMsg;
        EditText txtImei, txtRfid;
        public MappingActivity()
        {
            try
            {
                clsGLB = new clsGlobal();
                mappingDb = new MappingDb();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            try
            {
                base.OnCreate(savedInstanceState);
                SetContentView(Resource.Layout.activity_mapping);

                txtMsg = FindViewById<TextView>(Resource.Id.txtMsg);
                txtMsg.Text = "";

                txtImei = FindViewById<EditText>(Resource.Id.txtImei);
                txtImei.KeyPress += TxtImei_KeyPress;

                txtRfid = FindViewById<EditText>(Resource.Id.txtRfid);
                txtRfid.KeyPress += TxtRfid_KeyPress;

                Button btnBack = FindViewById<Button>(Resource.Id.btnBack);
                btnBack.Click += (e, a) =>
                {
                    this.Finish();
                };

                txtImei.RequestFocus();
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        private void TxtRfid_KeyPress(object sender, View.KeyEventArgs e)
        {
            try
            {
                if (e.Event.Action == KeyEventActions.Down)
                {
                    if (e.KeyCode == Keycode.Enter)
                    {
                        txtMsg.Text = "";
                        if (string.IsNullOrEmpty(txtImei.Text.Trim()))
                        {
                            clsGLB.ShowMessage("Scan IMEI Barcode", this, MessageTitle.INFORMATION);
                            txtImei.RequestFocus();
                            return;
                        }
                        if (string.IsNullOrEmpty(txtRfid.Text.Trim()))
                        {
                            clsGLB.ShowMessage("Scan Rfid Barcode", this, MessageTitle.INFORMATION);
                            txtRfid.RequestFocus();
                            return;
                        }
                        string ReturnMsg = mappingDb.MapRfid(new Mapping { IMEI=txtImei.Text.Trim(),RfidBarcode=txtRfid.Text.Trim()});
                        if (ReturnMsg.Split('~')[0] == "Y")
                        {
                            txtImei.Text = "";
                            txtRfid.Text = "";
                            txtImei.RequestFocus();
                            txtMsg.Text = "Mapped successfully";
                        }
                        else
                        {
                            clsGLB.ShowMessage(ReturnMsg.Split('~')[1], this, MessageTitle.INFORMATION);
                            txtRfid.Text = "";
                            txtRfid.RequestFocus();
                        }
                    }
                    else
                        e.Handled = false;
                }
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        private void TxtImei_KeyPress(object sender, View.KeyEventArgs e)
        {
            try
            {
                if (e.Event.Action == KeyEventActions.Down)
                {
                    if (e.KeyCode == Keycode.Enter)
                    {
                        txtMsg.Text = "";
                        if (!string.IsNullOrEmpty(txtImei.Text.Trim()))
                        {
                            string ReturnMsg = mappingDb.ValidateIMEI(txtImei.Text.Trim());
                            if (ReturnMsg.Split('~')[0] == "Y")
                            {
                                txtRfid.RequestFocus();
                            }
                            else
                            {
                                clsGLB.ShowMessage(ReturnMsg.Split('~')[1], this, MessageTitle.INFORMATION);
                                txtImei.Text = "";
                                txtImei.RequestFocus();
                            }
                        }
                        else
                        {
                            clsGLB.ShowMessage("Scan IMEI Barcode", this, MessageTitle.INFORMATION);
                            txtImei.RequestFocus();
                        }
                    }
                    else
                        e.Handled = false;
                }
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        public override void OnBackPressed()
        {
        }
    }
}