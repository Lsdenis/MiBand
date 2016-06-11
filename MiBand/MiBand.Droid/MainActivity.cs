using System;
using System.Collections.Generic;
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
    [Activity(Label = "MiBand", Icon = "@drawable/icon", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        public static BluetoothDevice Device = null;
        public ScanCallback ScanCallback;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            ScanCallback = new ScanCallbackClass() { OnSuccess = OnSuccess };
            ActivateMiBand();
        }

        private void ActivateMiBand()
        {
            MiBandDevice = new IMiBand(this);
            //            MiBandDevice1 = new IMiBand(Android.App.Application.Context);
            IMiBand.StartScan(ScanCallback);
        }

        public IMiBand MiBandDevice1 { get; set; }

        private void OnSuccess(BluetoothDevice bluetoothDevice)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                IMiBand.StopScan(ScanCallback);


                var a = new IMiBand(this);

                a.Pair(new Callback() { OnConnected = OnConnected });

                a.Connect(bluetoothDevice, new Callback() { OnConnected = OnConnected });

                //                MiBandDevice.Connect(bluetoothDevice, new Callback() { OnConnected = OnConnected });
                //                MiBandDevice1.Connect(bluetoothDevice, new Callback() { OnConnected = OnConnected });
            });
        }

        private void OnConnected()
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                MiBandDevice.StartVibration(VibrationMode.VibrationWithLed);
            });
        }

        public IMiBand MiBandDevice { get; set; }
    }

    public class Callback : IActionCallback
    {
        public Action OnConnected { get; set; }

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
            OnConnected?.Invoke();
        }
    }

    public class ScanCallbackClass : ScanCallback
    {
        public Action<BluetoothDevice> OnSuccess { get; set; }

        public override void OnScanResult(ScanCallbackType callbackType, ScanResult result)
        {
            base.OnScanResult(callbackType, result);

            if (result.Device.Address == "C8:0F:10:33:9D:41")
            {
                MainActivity.Device = result.Device;
                OnSuccess?.Invoke(result.Device);
            }
        }
    }

}

