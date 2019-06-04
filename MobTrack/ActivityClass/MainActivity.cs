using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content.PM;
using Android.Views;
using IOCLAndroidApp;
using Android.Content;
using System;
using System.IO;
using SatoScanningApp.ActivityClass;
using System.Collections.Generic;
using Android.Media;
using SatoScanningApp.DAL;
using IOCLAndroidApp.Models;

namespace SatoScanningApp
{
    [Activity(Label = "MobTrack", MainLauncher = true, WindowSoftInputMode = SoftInput.StateHidden, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : AppCompatActivity
    {
        clsGlobal clsGLB;
        ItemDb itemDb;
        TextView txtItemCount, txtMsg;

        public MainActivity()
        {
            try
            {
                clsGLB = new clsGlobal();
                itemDb = new ItemDb();
                //Set DBFilePath
                string folderPath = Path.Combine(clsGlobal.FilePath, clsGlobal.FileFolder);
                clsGlobal.DBFilePath = Path.Combine(folderPath, clsGlobal.DBFileName);
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
                // Set our view from the "main" layout resource
                SetContentView(Resource.Layout.activity_main);

                txtItemCount = FindViewById<TextView>(Resource.Id.txtItemCount);
                txtMsg = FindViewById<TextView>(Resource.Id.txtMsg);
                txtMsg.Text = "";

                Button btnLoadItem = FindViewById<Button>(Resource.Id.btnLoadItem);
                btnLoadItem.Click += BtnLoadItem_ClickAsync;

                Button btnMapping = FindViewById<Button>(Resource.Id.btnMapping);
                btnMapping.Click += BtnMapping_Click;

                itemDb.CreateDatabase();
                ShowItemCount();
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        private void BtnMapping_Click(object sender, EventArgs e)
        {
            try
            {
                OpenActivity(typeof(MappingActivity));
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        private async void BtnLoadItem_ClickAsync(object sender, EventArgs e)
        {
            var progressDialog = ProgressDialog.Show(this, "", "Updating Item Master...", true);
            StreamReader sr = null;
            try
            {
                string folderPath = Path.Combine(clsGlobal.FilePath, clsGlobal.FileFolder);
                string filename = Path.Combine(folderPath, clsGlobal.ItemFileName);

                if (File.Exists(filename))
                {
                    //Before upload new file remove all data
                    itemDb.DeleteItems();
                    sr = new StreamReader(filename);
                    //skip the header
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        string Data = await sr.ReadLineAsync();
                        string[] Arr = Data.Split(',');
                        itemDb.AddItem(new Item
                        {
                            BranchCode = Arr[0].Trim(),
                            City = Arr[1].Trim(),
                            State = Arr[2].Trim(),
                            Group = Arr[3].Trim(),
                            ItemSubGroup = Arr[4].Trim(),
                            ItemName = Arr[5].Trim(),
                            Color = Arr[6].Trim(),
                            IMEI1 = Arr[7].Trim(),
                            IMEStock = Arr[8].Trim(),
                            BranchName = Arr[9].Trim(),
                            WHName = Arr[10].Trim()
                        });
                    }

                    sr.Close();
                    sr.Dispose();
                    sr = null;

                    ShowItemCount();
                }
                else
                    clsGLB.ShowMessage("Item master file not found", this, MessageTitle.INFORMATION);
            }
            catch (Exception ex)
            { throw ex; }
            finally
            {
                progressDialog.Hide();
                if (sr != null)
                {
                    sr.Close();
                    sr.Dispose();
                    sr = null;
                }
            }
        }

        #region Methods

        private void ShowItemCount()
        {
            try
            {
                txtItemCount.Text = "Item Count : " + itemDb.GetAllItem().Count;
            }
            catch (Exception ex)
            {
                clsGLB.ShowMessage(ex.Message, this, MessageTitle.ERROR);
            }
        }

        public void ShowConfirmBox(string msg, Activity activity)
        {
            Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(activity);
            builder.SetTitle("Message");
            builder.SetMessage(msg);
            builder.SetCancelable(false);
            builder.SetPositiveButton("Yes", handllerOkButton);
            builder.SetNegativeButton("No", handllerCancelButton);
            builder.Show();
        }
        void handllerOkButton(object sender, DialogClickEventArgs e)
        {
            this.FinishAffinity();
        }
        void handllerCancelButton(object sender, DialogClickEventArgs e)
        {

        }
        public void OpenActivity(Type t)
        {
            try
            {
                Intent MenuIntent = new Intent(this, t);
                StartActivity(MenuIntent);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        public override void OnBackPressed()
        {
            ShowConfirmBox("Do you want to exit", this);
        }
    }
}