namespace Lockec.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Input;
    using Xamarin.Forms;
    using ZXing;
    using ZXing.Mobile;
    using ZXing.Net.Mobile.Forms;

    public class BarcodeViewModel : BaseViewModel
    {
        
        private string _result;

        public string Result
        {
            get { return this._result; }
            set { SetValue(ref this._result, value); }
        }

        public ICommand BarcodeScannerCommand
        {
            get { return new Command(BarcodeScanner); }
        }

        public BarcodeViewModel()
        {
           
        }

        private async void BarcodeScanner()
        {
            var options = new MobileBarcodeScanningOptions
            {
                PossibleFormats = new List<BarcodeFormat>
                {
                    BarcodeFormat.QR_CODE,
                    BarcodeFormat.CODE_128,
                    BarcodeFormat.EAN_13
                }
            };

            var page = new ZXingScannerPage(options) { Title = "Escanear Código QR" };
            var closeItem = new ToolbarItem { Text = "Cerrar" };
            closeItem.Clicked += (object sender, EventArgs e) =>
            {
                page.IsScanning = false;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Application.Current.MainPage.Navigation.PopModalAsync();
                });
            };

            page.ToolbarItems.Add(closeItem);

            page.OnScanResult += (result) =>
            {
                page.IsScanning = false;
                //AQUI PUEDE IR LA IMPLEMENTACION DE LO QUE HACE EL LECTOR QR.

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                    if (string.IsNullOrEmpty(result.Text))
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "¡Ups!",
                            "No fue posible leer este código QR. Inténtalo de nuevo.",
                            "Ok");
                        return;
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert(
                            "¡Yeah!",
                            "Su servicio ha culminado con éxito.",
                            "Ok");
                        return;
                    }
                });
            };

            await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(page)
            {
                BarTextColor = Color.White,
                BarBackgroundColor = Color.FromRgb(94, 122, 187)
            }, true) ;
        }

       

    }
}
