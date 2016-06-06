using System;
using System.Linq;
using Android.App;
using Android.Bluetooth;
using Android.Bluetooth.LE;
using Android.Content.PM;
using Android.Widget;
using Android.OS;
using Com.Zhaoxiaodan.Miband;
using Com.Zhaoxiaodan.Miband.Model;
using Xamarin.Forms;
using IMiBand = Com.Zhaoxiaodan.Miband.MiBand;
using Object = Java.Lang.Object;

namespace MiBand.Droid
{
    [Activity(Label = "MiBand", Icon = "@drawable/icon", MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static BluetoothDevice Device = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

            ActivateMiBand();
        }

        private void ActivateMiBand()
        {   
            var a = new IMiBand(this);
            var ad = BluetoothAdapter.DefaultAdapter;
            var device = ad.BondedDevices;

            IMiBand.StartScan(new ScanCallbackClass());

            while (Device == null)
            {
                
            }

//            foreach (var bluetoothDevice in ad.BondedDevices)
//            {
//                var c = bluetoothDevice.Name;
//            }

            a.Connect(Device, new Callback());

            a.StartVibration(VibrationMode.Vibration10TimesWithLed);
        }
    }

    public class Callback : IActionCallback
    {
        public void Dispose()
        {
        }

        public IntPtr Handle { get; }

        public void OnFail(int p0, string p1)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(Android.App.Application.Context, "Error", ToastLength.Short);
            });
        }

        public void OnSuccess(Object p0)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(Android.App.Application.Context, "Connected", ToastLength.Short);
            });
        }
    }

    public class ScanCallbackClass : ScanCallback
    {
        public override void OnScanResult(ScanCallbackType callbackType, ScanResult result)
        {
            base.OnScanResult(callbackType, result);

            MainActivity.Device = result.Device;
        }
    }

}

