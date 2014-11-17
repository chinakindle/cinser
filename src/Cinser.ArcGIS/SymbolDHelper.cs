using System;
using ESRI.ArcGIS.Client.Symbols;
using System.Windows.Controls;
using System.Windows;
using System.Collections;

namespace Cinser.ArcGIS
{

    /// <summary>
    /// 
    /// </summary>
    public class SymbolDHelper
    {
        private static SymbolDHelper _SymbolDHelper = null;


        private ResourceDictionary _ResourceDictionary = null;


        public SymbolDHelper()
        {
            _ResourceDictionary = new ResourceDictionary();
            _ResourceDictionary.Source = new Uri("/Cinser.ArcGIS;component/SymbolD.xaml", UriKind.RelativeOrAbsolute);
        }


        public static Symbol GetSymbolByName(string pName)
        {
            if (_SymbolDHelper == null)
            {
                _SymbolDHelper = new SymbolDHelper();
            }
            return _SymbolDHelper._ResourceDictionary[pName] as Symbol;
        }


        public static MarkerSymbol GetDefaultAlarmMarkerSymbol()
        {
            if (_SymbolDHelper == null)
            {
                _SymbolDHelper = new SymbolDHelper();
            }
            return _SymbolDHelper._ResourceDictionary["DefaultAlarmMarkerSymbol"] as MarkerSymbol;
        }


        public static SimpleFillSymbol GetFillSymbol()
        {
            SimpleFillSymbol mySimpleFillSymbol = new SimpleFillSymbol();
            mySimpleFillSymbol.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
            return mySimpleFillSymbol;
        }
    }
}
