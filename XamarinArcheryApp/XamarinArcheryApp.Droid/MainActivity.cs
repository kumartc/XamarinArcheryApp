﻿using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

namespace XamarinArcheryApp.Droid
{
  [Activity(Label = "XamarinArcheryApp", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
  public class MainActivity : FormsApplicationActivity
  {
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      Forms.Init(this, bundle);

      //Hackish way of inspecting the VM Exceptions during debug
      AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
      {
      };

      LoadApplication(new App());
    }
  }
}

