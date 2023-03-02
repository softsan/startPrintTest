using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using AndroidX.Core.Content;
using Android;
using AndroidX.Core.App;
using System.Collections.Generic;
using Xamarin.Essentials;

namespace testStarSdk.Droid
{
    [Activity(Label = "testStarSdk", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
            await Permissions.RequestAsync<BLEPermission>();
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        void CheckPermission()
        {
            var permissionList = new List<string>();

            if (ContextCompat.CheckSelfPermission(this, "android.permission.BLUETOOTH_CONNECT") == Permission.Granted)
            {
                if(Build.VERSION.SdkInt <=  BuildVersionCodes.R)
                {

                }
                else
                {
                    permissionList.Add("android.permission.BLUETOOTH_CONNECT");
                    ActivityCompat.RequestPermissions(this, permissionList.ToArray(), 2);
                }
                // Manifest.Permission.BluetoothConnect)
            }
        }
    }


    public class BLEPermission : Xamarin.Essentials.Permissions.BasePlatformPermission
    {
        public override (string androidPermission, bool isRuntime)[] RequiredPermissions => new List<(string androidPermission, bool isRuntime)>
        {
            ("android.permission.BLUETOOTH_CONNECT",true)
            //(Android.Manifest.Permission.BluetoothScan, true),
            //(Android.Manifest.Permission.BluetoothConnect, true)
        }.ToArray();
    }
}
